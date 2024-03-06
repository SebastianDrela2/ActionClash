using RandomFight.Match;

namespace RandomFight
{
    internal class Program
    {        
        static void Main()
        {          
            var gameHost = new GameHost();

            gameHost.StartMatch();           
            gameHost.AnnounceInProgress();

            var matchResult = gameHost.GetMatchResult();

            Console.WriteLine(matchResult);

        }       
    }
}
