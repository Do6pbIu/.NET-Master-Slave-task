using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UserStorage;

namespace MasterSlave
{
    public sealed class Slave
    {
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
                return _service.GetUserById(id);
            }
            catch (Exception ex)
            {
                //write exception to logFile
            }
            return null;
        }

        public IEnumerable<User> SearchForUser(Func<User, bool> match)
        {
            try
            {
                return _service.SearchForUser(match);
            }
            catch (Exception ex)
            {
                //write exception to logFile
            }
            return null;
        }

        private void ListenToMaster()
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
        }

        private void ReceiveMessage(TcpClient client)
        {
            try
            {
                using (NetworkStream n = client.GetStream())
                {
                    var formatter = new BinaryFormatter();
                    //User user = (User)formatter.Deserialize(n);
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
                        _service.AddUser(msg.User);
                        break;
                    case Command.Delete:
                        _service.DeleteUser(msg.User);
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
