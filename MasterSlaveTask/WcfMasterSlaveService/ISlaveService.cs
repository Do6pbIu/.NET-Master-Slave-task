using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfMasterSlaveService
{
    [ServiceContract]
    interface ISlaveService
    {
        [OperationContract]
        UserDataContract GetUserById(string id);

        [OperationContract]
        UserDataContract SearchForUser(Func<UserDataContract, bool> match);
    }
}
