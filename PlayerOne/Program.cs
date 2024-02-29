using System.IO.Pipes;

namespace PlayerOne
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetUpPlayer();
        }

        private static void SetUpPlayer()
        {
            using var namedPipeServerStream = new NamedPipeServerStream("PlayerOne");
            using var namedPipeClientStream = new NamedPipeClientStream("PlayerTwo");

            Console.WriteLine($"Waiting for PlayerTwo...");

            namedPipeClientStream.Connect();
            namedPipeServerStream.WaitForConnection();
            Console.WriteLine("Connected.");

            using var streamReader = new StreamReader(namedPipeClientStream);
            using var streamWriter = new StreamWriter(namedPipeServerStream);

            while (true)
            {                
                var message = streamReader.ReadLine();
                Thread.Sleep(1000);

                Console.WriteLine($"{message}");

                streamWriter.WriteLine("Response 2");
                streamWriter.Flush();               
            }
        }
    }
}
