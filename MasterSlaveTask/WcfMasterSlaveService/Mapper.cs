using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage;

namespace WcfMasterSlaveService
{
    public static class Mapper
    {
        public static UserDataContract UserToContract(User user)
        {
            return new UserDataContract
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Sex = user.Sex,
                //visaCards = user.visaCards
            };
        }

        public static VisaDataContract VisaToContract(VisaRecord visa)
        {
            return new VisaDataContract
            {
                Country = visa.Country,
                StartDate=visa.StartDate,
                EndDate=visa.EndDate                
            };
        }

        public static User ContractToUser(this UserDataContract user)
        {
            return new User(user.FirstName, user.LastName, user.Id, user.Sex);
        }

        public static VisaRecord VisaToContract(this VisaDataContract visa)
        {
            return new VisaRecord(visa.Country, visa.StartDate, visa.EndDate);
        }
    }
}
