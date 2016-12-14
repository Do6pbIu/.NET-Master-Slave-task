using MasterSlave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserStorage;

namespace TryToRun
{

    public class Validator : IUserValidator
    {
        public bool UserIsValid(User user)
        {
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

    class Program
    {
        static User[] users = { new User("John", "Smith", "1", Gender.Male),
                                new User("Bill", "Smith", "2", Gender.Male),
                                new User("Barrak", "Smith", "3", Gender.Male),
                                new User("george", "Smith", "4", Gender.Male),
                                new User("Stan", "Smith", "5", Gender.Male)};

        static void Main(string[] args)
        {
            
            Validator validator = new Validator();
            Generator generator = new Generator();
            
            Slave sl1 = new Slave(generator, validator);
            Slave sl2 = new Slave(generator, validator, new Address("localhost", 51112));
            Master master = new Master(generator, validator);
            
            sl1.ListenToMaster();
            sl2.ListenToMaster();

            Thread.Sleep(1000);

            master.EstablishConnectionWithSlaves();

            Task.Run(() => PrintUsers(sl1));
            Task.Run(() => PrintUsersAsync(sl2));

            while (true)
            {
                for (int i = 0; i < 5; i++)
                {
                    master.AddUser(users[i]);
                }
                Thread.Sleep(1000);
                for (int i = 0; i < 5; i++)
                {
                    master.DeleteUser(users[i]);
                }
                Thread.Sleep(1000);
            }
            
                       
            Console.ReadLine();
        }

        static void PrintUsers(Slave slave)
        {
            while (true)
            {
                List<User> users = slave.SearchForUser(u => u.Sex == Gender.Male).ToList();
                foreach (User user in users)
                {
                    Console.WriteLine(user.ToString());
                }
                Thread.Sleep(1000);
            }
        }

        static async void PrintUsersAsync(Slave slave)
        {
            while (true)
            {
                var users = await slave.SearchForUserAsync(u => u.Sex == Gender.Male);
                List<User> lusers = users.ToList();
                foreach (User user in lusers)
                {
                    Console.WriteLine("from async slave2" + user.ToString());
                }
                Thread.Sleep(1000);
            }
        }
    }
}
