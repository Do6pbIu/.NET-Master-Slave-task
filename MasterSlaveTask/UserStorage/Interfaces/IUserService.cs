using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    public interface IUserService
    {
        void ChangeIdGenerator(IIdGenerator newGenerator);

        void ChangeUserValidator(IUserValidator newValidator);

        void SaveStateToFile(string path);

        void LoadFromFile(string path);

        string AddUser(User newUser);

        string CreateUser(string firstName, string lastName, Gender sex);

        void DeleteUser(User dUser);

        IEnumerable<User> SearchForUser(Func<User, bool> match);

        User GetUserById(string id);
    }
}
