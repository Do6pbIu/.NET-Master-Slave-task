using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageTest
{
    namespace TCPTraining
    {
        class Program
        {
            static void Main(string[] args)
            {
                new Thread(Server).Start();
                Thread.Sleep(500);
                Client();
                Console.ReadLine();
            }


            static void Client()
            {
                using (TcpClient client = new TcpClient("localhost", 51111))
                using (NetworkStream n = client.GetStream())
                {
                    for (int i = 0; i < 5; i++)
                    {
                        User john = new User("John", "Smith", "5", Gender.Male);
                        Message msg = new Message(Command.Create, john);

                        var formatter = new BinaryFormatter();
                        formatter.Serialize(n, msg);

                        Thread.Sleep(2000);
                    }
                }
            }

            static void Server()
            {

                TcpListener listener = new TcpListener(IPAddress.Any, 51111);
                try
                {
                    listener.Start();
                    using (TcpClient c = listener.AcceptTcpClient())
                    using (NetworkStream n = c.GetStream())
                    {
                        while (true)
                        {
                            var formatter = new BinaryFormatter();
                            //User user = (User)formatter.Deserialize(n);
                            Message msg = (Message)formatter.Deserialize(n);

                            //string msg = new BinaryReader(n).ReadString();

                            Console.WriteLine(msg.user.ToString() + " " + msg.command.ToString());
                            // if (msg == "4") break;
                        }
                    }
                }
                finally
                {
                    listener.Stop();
                }
            }

        }
    }
}
