using RandomFight.Match;

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

            var matchResult = gameHost.GetMatchResult();

            Console.WriteLine(matchResult);

        }       
    }
}
