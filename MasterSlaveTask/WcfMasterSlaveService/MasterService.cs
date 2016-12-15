using MasterSlave;
using MasterSlave.ConfigSection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserStorage;

namespace WcfMasterSlaveService
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

    public class MasterService : IMasterService
    {
        private static readonly Master _master;

        static MasterService()
        {
            List<Address> slaves = new List<Address>();

            ConfigSettings config = new ConfigSettings();

            foreach (var item in config.ServiceElemetns)
            {
                if (item.Type=="Slave")
                {
                    slaves.Add(new Address(item.IP, Convert.ToInt32(item.Port)));
                }               
            }

            var appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MasterDomain")
            };

            AppDomain domain = AppDomain.CreateDomain("MasterDomain", null, appDomainSetup);
            var assembly = Assembly.Load("MasterSlave");

            Master master = (Master)domain.CreateInstanceAndUnwrap(assembly.FullName, typeof(Master).FullName, true,
                BindingFlags.Default, null, args: new object[] 
                            { new Generator(), new Validator(), slaves}, culture: null,
                activationAttributes: null);
        }

        public void AddUser(UserDataContract user)
        {
            _master.AddUser(Mapper.ContractToUser(user));
        }

        public void CreateUser(string firstName, string lastName, Gender sex)
        {
            _master.CreateUser(firstName, lastName, sex);
        }

        public void DeleteUser(UserDataContract user)
        {
            _master.DeleteUser(Mapper.ContractToUser(user));
        }

        //master doesn't search users - slave does
        public UserDataContract SearchUserById(int id)
        {
            throw new NotImplementedException();
        }       
    }
}
