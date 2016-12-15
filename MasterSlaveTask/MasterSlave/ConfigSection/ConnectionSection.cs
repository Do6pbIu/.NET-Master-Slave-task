using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterSlave.ConfigSection
{
    public class ConnectionSection : ConfigurationSection
    {
        [ConfigurationProperty("Services")]
        public ServiceCollection ServiceElement
        {
            get { return ((ServiceCollection)(base["Services"])); }
            set { base["Services"] = value; }
        }
    }
}
