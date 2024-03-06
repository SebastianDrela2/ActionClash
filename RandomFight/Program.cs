using RandomFight.Match;
using System.IO.Pipes;

namespace RandomFight
{
    internal class Program
    {        
        static void Main()
        {          
            var gameHost = new GameHost();
            gameHost.StartMatch();

            Console.WriteLine("Match In Progress...");

            var namedPipeServerStream = new NamedPipeServerStream("GameHost");           
            using var streamReader = new StreamReader(namedPipeServerStream);

            namedPipeServerStream.WaitForConnection();
            var matchResult = streamReader.ReadLine();
            Console.WriteLine(matchResult);

        }       
    }
}
