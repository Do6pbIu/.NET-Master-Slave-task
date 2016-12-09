using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage;

namespace MasterSlave
{
    [Serializable]
    public class Message
    {
        public Command Command { get; private set; }

        public User User { get; private set; }

        public Message(Command command, User user)
        {
            if (user == null) throw new ArgumentNullException();
            Command = command;
            User = user;
        }
    }

    public enum Command
    {
        Create,
        Read,
        Update,
        Delete
    }    
}
