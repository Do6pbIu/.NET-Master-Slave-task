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
    public class SlaveService : ISlaveService
    {
        private static List<Slave> _slaves;

        static SlaveService()
        {
            _slaves = new List<Slave>();
            ConfigSettings config = new ConfigSettings();
            foreach (var item in config.ServiceElemetns)
            {                
                if (item.Type == "Slave")
                {
                    AppDomainSetup appDomainSetup = new AppDomainSetup
                    {
                        ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                        PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"SlaveDomain port {item.Port} ip {item.IP}")
                    };
                    AppDomain domain = AppDomain.CreateDomain($"SlaveDomain port {item.Port} ip {item.IP}", null, appDomainSetup);
                    Assembly assembly = Assembly.Load("MasterSlave");

                    Slave slave = (Slave)domain.CreateInstanceAndUnwrap(assembly.FullName, typeof(Slave).FullName, true,
                            BindingFlags.Default, null, args: new object[] 
                                                 {new Address(item.IP, Convert.ToInt32(item.Port))}, 
                            culture: null,
                            activationAttributes: null);
                }
            }
        }

        public UserDataContract GetUserById(string id)
        {
            User user = _slaves.FirstOrDefault().GetUserById(id);
            return Mapper.UserToContract(user);
        }

        //I will write it later if I will have time
        public UserDataContract SearchForUser(Func<UserDataContract, bool> match)
        {
            return null;
        }
    }
}
