using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterSlave.ConfigSection
{
    public class Element : ConfigurationElement
    {
        [ConfigurationProperty("Name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["Name"]; }
            set { base["Name"] = value; }
        }

        [ConfigurationProperty("Type", DefaultValue = "Slave", IsKey = false, IsRequired = true)]
        public string Type
        {
            get { return (string)base["Type"]; }
            set { base["Type"] = value; }
        }

        [ConfigurationProperty("BasePath", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string BasePath
        {
            get { return (string)base["BasePath"]; }
            set { base["BasePath"] = value; }
        }

        [ConfigurationProperty("LogPath", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string LogPath
        {
            get { return (string)base["LogPath"]; }
            set { base["LogPath"] = value; }
        }

        [ConfigurationProperty("IP", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string IP
        {
            get { return (string)base["IP"]; }
            set { base["IP"] = value; }
        }

        [ConfigurationProperty("Port", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Port
        {
            get { return (string)base["Port"]; }
            set { base["Port"] = value; }
        }
    }
}
