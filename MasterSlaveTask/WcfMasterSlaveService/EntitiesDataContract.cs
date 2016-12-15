using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UserStorage;

namespace WcfMasterSlaveService
{
    [DataContract]
    public class UserDataContract
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        //I wish I knew, should we declare this enum in WCF project or we should use reference to UserStorage
        [DataMember]
        public Gender Sex { get; set; }

        [DataMember]
        public VisaDataContract[] visaCards { get; set; }
    }

    [DataContract]
    public struct VisaDataContract
    {
        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }
    }
}
