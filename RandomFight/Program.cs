using RandomFight.Match;
using System.Diagnostics;

namespace RandomFight
{
    internal class Program
    {        
        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            var gameHost = new GameHost();

            gameHost.StartMatch();           
            gameHost.AnnounceInProgress();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var matchResult = gameHost.GetMatchResult();

            stopWatch.Stop();

            var matchTimeInSeconds = (int) stopWatch.Elapsed.TotalSeconds;

            Console.WriteLine($"Match lasted {matchTimeInSeconds} seconds.");
            Console.WriteLine(matchResult);            

        }       
    }
}
