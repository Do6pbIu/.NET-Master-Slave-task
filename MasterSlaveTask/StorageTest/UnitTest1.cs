using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage;
using System.Collections.Generic;

namespace StorageTest
{
    [TestClass]
    public class UnitTest1
    {
        public class Validator : IUserValidator
        {
            public bool UserIsValid(User user)
            {
                if (user.Id == null || user.LastName == null || user.FirstName == null) return false;                
                return true;
            }
        }

        public class Generator : IIdGenerator
        {
            private int curId;

            public Generator() { curId = 0; }

            public string LastId
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public string NextId()
            {
                return (++curId).ToString();
            }
        }

        public static UserService getService()
        {
            Validator validator = new Validator();
            Generator generator = new Generator();
            UserService service = new UserService(generator, validator);
            User john = new User("John", "Smith", "5", Gender.Male);
            service.AddUser(john);
            service.CreateUser("John", "Skeet", Gender.Male);
            return service;
        }

        [TestMethod]
        public void AddUserTest()
        {
            UserService service = getService();
            Assert.AreEqual(2, service.Users.Count);
        }

        [TestMethod]
        public void GetUserByIDTest()
        {
            UserService service = getService();
            User john = new User("John", "Smith", "5", Gender.Male);
            Assert.AreEqual(john, service.GetUserById(john.Id));
        }

        [TestMethod]
        public void RemoveUserTest()
        {
            UserService service = getService();
            User john = new User("John", "Smith", "5", Gender.Male);
            service.DeleteUser(john);
            Assert.AreEqual(null, service.GetUserById(john.Id));
            Assert.AreEqual(1, service.Users.Count);
        }
    }
}
