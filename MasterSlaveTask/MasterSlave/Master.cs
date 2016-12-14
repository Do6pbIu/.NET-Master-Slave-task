using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserStorage;

namespace MasterSlave
{
    public sealed class Master
    {
        private IUserService _service;

        private string _pathToUsers;

        private List<Address> _address;
        
        #region Constructors
        public Master(IIdGenerator idGenerator, IUserValidator userValidator, string path)
        {
            if ((idGenerator == null) || (userValidator == null) || (path == null)) throw new ArgumentNullException();
            _service = new UserService(idGenerator, userValidator, true);
            _pathToUsers = path;
            _service.LoadFromFile(_pathToUsers);            
        }

        public Master(IIdGenerator idGenerator, IUserValidator userValidator)
        {
            _pathToUsers = "users.soap";
            if ((idGenerator == null) || (userValidator == null)) throw new ArgumentNullException();
            _service = new UserService(idGenerator, userValidator, true);
            //_slaves.Add(new TcpClient("localhost", 51111));
        }

        public Master(IIdGenerator idGenerator, IUserValidator userValidator, List<Address> addrs)
        {
            _pathToUsers = "users.soap";
            if ((idGenerator == null) || (userValidator == null)) throw new ArgumentNullException();
            _service = new UserService(idGenerator, userValidator, true);
            _address = addrs;
        }
        #endregion

        #region UserServiceState
        public void ChangeValidator(IUserValidator userValidator)
        {
            if (userValidator == null) throw new ArgumentNullException();
            _service.ChangeUserValidator(userValidator);
        }

        public void ChangeIdGenerator(IIdGenerator idGenerator)
        {
            if (idGenerator == null) throw new ArgumentNullException();
            _service.ChangeIdGenerator(idGenerator);
        }

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

        #region UpdateUsers
        public void AddUser(User newUser)
        {
            try
            {
                string id = _service.AddUser(newUser);
                if (id != null)
                {
                    Message message = new Message(Command.Create, newUser);
                    NotifySlaves(message);
                }
            }
            catch (Exception ex)
            {
                //write exception to logFile
            }
        }

        public void CreateUser(string firstName, string lastName, Gender sex)
        {
            try
            {
                string id = _service.CreateUser(firstName, lastName, sex);
                if (id != null)
                {
                    User newUser = _service.GetUserById(id);
                    Message message = new Message(Command.Create, newUser);
                    NotifySlaves(message);
                }
            }
            catch (Exception ex)
            {
                //write exception to logFile
            }
        }

        public void DeleteUser(User dUser)
        {
            try
            {
                _service.DeleteUser(dUser);
                Message message = new Message(Command.Delete, dUser);
                NotifySlaves(message);                
            }
            catch (Exception ex)
            {
                //write exception to logFile
            }
        }
        #endregion

        public void EstablishConnectionWithSlaves()
        {
            _address = new List<Address>();
            _address.Add(new Address("localhost", 51111));
            _address.Add(new Address("localhost", 51112));
        }

        private void NotifySlave(Message message, Address addr)
        {
            using (TcpClient slave = new TcpClient(addr.HostName,addr.Port))
            using (NetworkStream n = slave.GetStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(n, message);
            }
        }
                
        private void NotifySlaves(Message message)
        {
            foreach (Address addr in _address)
            {
                try
                {
                    Task.Run(() => NotifySlave(message, addr));
                    //NotifySlave(message, addr);
                }
                catch (Exception ex)
                {
                    //write excetrion to logFile
                }
            }            
        }
    }    
}
