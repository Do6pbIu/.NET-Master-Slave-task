using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterSlave.ConfigSection
{
    public class ConfigSettings
    {
        public ConnectionSection ServerAppearanceConfiguration
        {
            get
            {
                return (ConnectionSection)ConfigurationManager.GetSection("serverSection");
            }
        }

        public ServiceCollection ServiceApperances
        {
            get
            {
                return this.ServerAppearanceConfiguration.ServiceElement;
            }
        }

        public IEnumerable<Element> ServiceElemetns
        {
            get
            {
                foreach (Element selement in this.ServiceApperances)
                {
                    if (selement != null)
                        yield return selement;
                }
            }
        }
    }
}
