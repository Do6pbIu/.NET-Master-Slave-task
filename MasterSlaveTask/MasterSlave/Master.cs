using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UserStorage;

namespace MasterSlave
{
    public sealed class Master
    {
        private IUserService _service;

        private string _pathToUsers;

        private List<Address> _address;

        private List<TcpClient> _slaves;

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
            if ((idGenerator == null) || (userValidator == null)) throw new ArgumentNullException();
            _service = new UserService(idGenerator, userValidator, true);
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
            _slaves = new List<TcpClient>();
            foreach(Address adr in _address)
            {
                try
                {
                    TcpClient client = new TcpClient(adr.HostName, adr.Port);
                    _slaves.Add(client);
                }
                catch (Exception ex)
                {
                    //write exception to logFile
                }
            }
        }

        private void NotifySlaves(Message message)
        {
            foreach (TcpClient slave in _slaves)
            {
                try
                {
                    using (NetworkStream n = slave.GetStream())
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(n, message);
                    } 
                }
                catch (Exception ex)
                {
                    //write excetrion to logFile
                }
            }
        }

    }    
}
