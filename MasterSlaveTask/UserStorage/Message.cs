using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    [Serializable]
    public class Message
    {
        public Command command { get; private set; }

        public User user { get; private set; }
    }

    public enum Command
    {
        Create,
        Read,
        Update,
        Delete
    }
}
