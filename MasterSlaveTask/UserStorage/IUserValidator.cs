using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    public interface IUserValidator
    {
        bool UserIsValid(User user);
    }
}
