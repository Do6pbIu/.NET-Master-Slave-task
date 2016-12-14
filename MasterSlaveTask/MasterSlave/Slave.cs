using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserStorage;

namespace MasterSlave
{
    public sealed class Slave
    {
        private ReaderWriterLockSlim  _serviceLock = new ReaderWriterLockSlim();

        private Address _adress;

        private IUserService _service;

        private string _pathToUsers;        

        #region Constructors
        public Slave(IIdGenerator idGenerator, IUserValidator userValidator, string path)
        {
            if ((idGenerator == null) || (userValidator == null) || (path == null)) throw new ArgumentNullException();
            _service = new UserService(idGenerator, userValidator, true);
            _pathToUsers = path;
            _service.LoadFromFile(_pathToUsers);
        }

        public Slave(IIdGenerator idGenerator, IUserValidator userValidator)
        {
            if ((idGenerator == null) || (userValidator == null)) throw new ArgumentNullException();
            _service = new UserService(idGenerator, userValidator, true);
            _adress = new Address("localhost", 51111);
        }

        public Slave(IIdGenerator idGenerator, IUserValidator userValidator, Address addr)
        {
            if ((idGenerator == null) || (userValidator == null)) throw new ArgumentNullException();
            _service = new UserService(idGenerator, userValidator, true);
            _adress = addr;
        }
        #endregion

        #region UserServiceState
        public void SaveToFile(string path)
        {
            if (path == null) throw new ArgumentNullException();
            _service.SaveStateToFile(path);
        }

        public void LoadFromFile(string path)
        {
            if (path == null) throw new ArgumentNullException();
            _service.LoadFromFile(path);
        }
        #endregion


        public User GetUserById(string id)
        {
            try
            {
                _serviceLock.EnterReadLock();
                return _service.GetUserById(id);
            }            
            catch (Exception ex)
            {
                //write exception to logFile
            }
            finally
            {
                _serviceLock.ExitReadLock();
            }
            return null;
        }       

        public Task<User> GetUserByIdAsync(string id)
        {
            try
            {
                _serviceLock.EnterReadLock();
                return Task.Run(() => _service.GetUserById(id));                   
            }
            catch (Exception ex)
            {
                //write exception to logFile
            }
            finally
            {
                _serviceLock.ExitReadLock();
            }
            return null;
        }

        public IEnumerable<User> SearchForUser(Func<User, bool> match)
        {
            try
            {
                _serviceLock.EnterReadLock();
                return _service.SearchForUser(match);
            }
            catch (Exception ex)
            {
                //write exception to logFile
            }
            finally
            {
                _serviceLock.ExitReadLock();
            }
            return null;
        }

        public Task<IEnumerable<User>> SearchForUserAsync(Func<User, bool> match)
        {
            try
            {
                _serviceLock.EnterReadLock();
                return Task.Run(() => _service.SearchForUser(match));
            }
            catch (Exception ex)
            {
                //write exception to logFile
            }
            finally
            {
                _serviceLock.ExitReadLock();
            }
            return null;
        }

        private void Listen()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, _adress.Port);
            listener.Start();
            try
            {
                while (true)
                {
                    ReceiveMessage(listener.AcceptTcpClient());
                }
            }
            catch (Exception ex)
            {
                //write exception to logFile
            }
            finally
            {
                listener.Stop();
            }
        }
       
        public void ListenToMaster()
        {
            Task.Run(() => Listen());
        }

        private void ReceiveMessage(TcpClient client)
        {
            try
            {
               // using (client)
                using (NetworkStream n = client.GetStream())
                {
                    var formatter = new BinaryFormatter();
                    Message msg = (Message)formatter.Deserialize(n);
                    ProcessMessage(msg);
                }
            }
            catch (Exception ex)
            {
                //write exception to logFile
            }
        }

        private void ProcessMessage(Message msg)
        {
            try
            {
                switch (msg.Command)
                {
                    case Command.Create:
                        try
                        {
                            _serviceLock.EnterWriteLock();
                            _service.AddUser(msg.User);
                        }
                        finally
                        {
                            _serviceLock.ExitWriteLock();
                        }
                        break;
                    case Command.Delete:
                        try
                        {
                            _serviceLock.EnterWriteLock();
                            _service.DeleteUser(msg.User);
                        }
                        finally
                        {
                            _serviceLock.ExitWriteLock();
                        }
                        break;
                    default:
                        throw new ArgumentException();                        
                }
            }
            catch (Exception ex)
            {
                //write exception to logFile
            }
        }
    }
}
