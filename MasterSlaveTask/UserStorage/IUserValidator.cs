using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    /// <summary>
    /// Validates user before adding him to storage
    /// </summary>
    public interface IUserValidator
    {
        /// <summary>
        /// Validates user(unique id and allowable names)
        /// </summary>
        /// <param name="user"></param>
        /// <returns>true if user is valid, otherwise false</returns>
        bool UserIsValid(User user);        
    }
}
