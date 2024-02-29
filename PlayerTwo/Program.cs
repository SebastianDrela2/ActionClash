using System.IO.Pipes;
using System.Threading.Channels;

namespace PlayerTwo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetUpPlayer();
        }

        private static void SetUpPlayer()
        {
            using var namedPipeServerStream = new NamedPipeServerStream("PlayerTwo");
            using var namedPipeClientStream = new NamedPipeClientStream("PlayerOne");

            Console.WriteLine($"Waiting for PlayerOne...");

             namedPipeClientStream.Connect();
             namedPipeServerStream.WaitForConnection();

            Console.WriteLine("Connected.");

            using var streamReader = new StreamReader(namedPipeClientStream);
            using var streamWriter = new StreamWriter(namedPipeServerStream);

            while (true)
            {               
                streamWriter.WriteLine("Response 1");
                streamWriter.Flush();
               
                var message = streamReader.ReadLine();
                Thread.Sleep(1000);
                Console.WriteLine($"{message}");              
            }
        }
    }
}
