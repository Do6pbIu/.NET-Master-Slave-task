using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using UserStorage;

namespace WcfMasterSlaveService
{    
    [ServiceContract]
    public interface IMasterService
    {
        [OperationContract]
        UserDataContract SearchUserById(int id);

        [OperationContract]
        void AddUser(UserDataContract user);

        [OperationContract]
        void CreateUser(string firstName, string lastName, Gender sex);

        [OperationContract]
        void DeleteUser(UserDataContract user);
    }        
}
