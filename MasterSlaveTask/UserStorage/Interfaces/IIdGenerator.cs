using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    /// <summary>
    /// Generates Id for new users
    /// </summary>
    public interface IIdGenerator
    {
        /// <summary>
        /// Generates next unique Id
        /// </summary>
        /// <returns></returns>
        string NextId();

        /// <summary>
        /// Value of the last generated id
        /// </summary>
        string LastId { get; set; }         
    }
}
