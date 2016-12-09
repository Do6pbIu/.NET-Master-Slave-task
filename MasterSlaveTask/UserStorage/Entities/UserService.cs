using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UserStorage 
{
    [Serializable]
    public class UserService : IUserService
    {
        [NonSerialized]
        private IList<User> users;

        private bool isMaster;

        //this field is need for soap serialisation
        private User[] userArray;

        //this property should be removed after testing!
        public IList<User> Users { get { return users; } }

        [NonSerialized]
        private IIdGenerator idGenerator;

        [NonSerialized]
        private IUserValidator userValidator;        
        
        private string lastId;

        #region Constructors

        public UserService(IIdGenerator generator, IUserValidator validator, IList<User> existingUsers, bool master = false)
        {
            if (existingUsers == null || generator == null || validator == null)
                throw new ArgumentException();
            users = existingUsers;
            idGenerator = generator;
            userValidator = validator;
            isMaster = master;
        }

        public UserService(IIdGenerator generator, IUserValidator validator, bool master = false)
        {
            if (generator == null || validator == null)
                throw new ArgumentException();
            users = new List<User>();
            idGenerator = generator;
            userValidator = validator;
            isMaster = master;
        }

        /// <summary>
        /// Public default cousnturcor for XLM serialization
        /// </summary>
        public UserService() { }
        #endregion

        #region CRUDUser
        /// <summary>
        /// Adds created user to storage
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns>User's id if he was valid and added</returns>
        public string AddUser(User newUser)
        {
            if (userValidator.UserIsValid(newUser))
            {
                users.Add(newUser);
                return newUser.Id;
            }
            else
                return null;
        }

        /// <summary>
        /// Creates new user and adds him to storage
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="sex"></param>
        /// <returns>User's id if he was added to storage</returns>
        public string CreateUser(string firstName, string lastName, Gender sex)
        {
            if (!isMaster) throw new InvalidOperationException();
            User newUser = new User(firstName, lastName, idGenerator.NextId(), sex);
            lastId = newUser.Id;
            return (AddUser(newUser));
        }

        /// <summary>
        /// Removes specified user from storage
        /// </summary>
        /// <param name="dUser"></param>
        public void DeleteUser(User dUser)
        {
            if (!isMaster) throw new InvalidOperationException();
            if (dUser == null) throw new ArgumentNullException();
            users.Remove(dUser);
        }

        /// <summary>
        /// Searches users with specified cinditions
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public IEnumerable<User> SearchForUser(Func<User, bool> match) => users.Where(match);

        /// <summary>
        /// Searches user with specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(string id)
        {
            if (id == null) throw new ArgumentException();
            User user = users.Where(u => u.Id == id).FirstOrDefault();
            return user;
        }

        #endregion

        #region ServiceState
        /// <summary>
        /// Changes the strategy to generate and Id
        /// </summary>
        /// <param name="newGenerator"></param>
        public void ChangeIdGenerator(IIdGenerator newGenerator)
        {
            if (newGenerator == null)
                throw new ArgumentException();
            else
            {
                newGenerator.LastId = lastId;
                idGenerator = newGenerator;
            }
        }

        /// <summary>
        /// Chenges the rules of validation
        /// </summary>
        /// <param name="newValidator"></param>
        public void ChangeUserValidator(IUserValidator newValidator)
        {
            if (newValidator == null) throw new ArgumentException();
            else
                userValidator = newValidator;
        }        

        /// <summary>
        /// Stores state to file in disk using SoapFormatter
        /// </summary>
        /// <param name="path">Path to a file</param>
        public void SaveStateToFile(string path)
        {
            if (path == null) throw new ArgumentException();
            userArray = users.ToArray();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                SoapFormatter formatter = new SoapFormatter();
                formatter.Serialize(fs, this);
                Console.WriteLine("Объект сериализован");
            }
        }

        /// <summary>
        /// Loads state from file using SoapFormatter
        /// </summary>
        /// <param name="path">Path to a file</param>
        public void LoadFromFile(string path)
        {
            if (path == null) throw new ArgumentException();
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                SoapFormatter formatter = new SoapFormatter();
                UserService loadedService = (UserService)formatter.Deserialize(fs);
                users = loadedService.userArray.ToList();
                lastId = loadedService.lastId;
            }
        }
        #endregion

    }
}
