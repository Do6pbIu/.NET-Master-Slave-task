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
        static void Main(string[] args)
        {
            User john = new User("John", "Smith", "1", Gender.Male);
            User bill = new User("Bill", "Smith", "2", Gender.Male);
            User barrak = new User("Barrak", "Smith", "3", Gender.Male);
            User george = new User("george", "Smith", "4", Gender.Male);
            User stan = new User("Stan", "Smith", "5", Gender.Male);

            Validator validator = new Validator();
            Generator generator = new Generator();

            Slave slave = new Slave(generator, validator);
            Master master = new Master(generator, validator);

            //new Thread(Server).Start();
            //master.EstablishConnectionWithSlaves();
            //slave.ListenToMaster();

            master.AddUser(john);
            master.AddUser(bill);
            master.AddUser(barrak);
            master.AddUser(george);
            master.AddUser(stan);
            Console.ReadLine();
        }
    }
}
