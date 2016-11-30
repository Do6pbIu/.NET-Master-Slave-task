using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    [Serializable]
    public class UserService
    {
        private IList<User> users;

        [NonSerialized]
        private IIdGenerator idGenerator;

        [NonSerialized]
        private IUserValidator userValidator;

        private string lastId;
        #region Constructors
        public UserService(IIdGenerator generator, IUserValidator validator, IList<User> existingUsers)
        {
            if (existingUsers == null || generator == null || validator == null)
                throw new ArgumentException();
            users = existingUsers;
            idGenerator = generator;
            userValidator = validator;
        }

        public UserService(IIdGenerator generator, IUserValidator validator)
        {
            if (generator == null || validator == null)
                throw new ArgumentException();
            users = new List<User>();
            idGenerator = generator;
            userValidator = validator;
        }

        private UserService() { }
        #endregion

        #region CRUDuser
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
                throw new ArgumentException();
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
            users.Remove(dUser);
        }

        /// <summary>
        /// Searches users with specified cinditions
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public IEnumerable<User> SearchForUser(Func<User, bool> match) => users.Where(match);
        #endregion

        /// <summary>
        /// Changes the strategy to generate and Id
        /// </summary>
        /// <param name="newGenerator"></param>
        public void ChangeIdGenerator(IIdGenerator newGenerator)
        {
            if (newGenerator == null)
                throw new ArgumentException();
            else
                idGenerator = newGenerator;
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
    }
}
