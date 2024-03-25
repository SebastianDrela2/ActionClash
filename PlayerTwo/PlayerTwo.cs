using Newtonsoft.Json;
using RandomFight.ConsoleUtils;
using RandomFight.Match;
using RandomFight.Player;
using System.IO.Pipes;

namespace PlayerTwo
{
    public class PlayerTwo
    {
        private readonly GameHost _gameHost = new GameHost();
        public void SetUpPlayer()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            using var namedPipeServerStream = new NamedPipeServerStream("PlayerTwo");
            using var namedPipeClientStream = new NamedPipeClientStream("PlayerOne");

            Console.WriteLine($"Waiting for PlayerOne...");

            namedPipeClientStream.Connect();
            namedPipeServerStream.WaitForConnection();

            Console.WriteLine("Connected.");

            StartPlayer(namedPipeClientStream, namedPipeServerStream);

            Environment.Exit(0);
        }

        private void StartPlayer(NamedPipeClientStream namedPipeClientStream, NamedPipeServerStream namedPipeServerStream)
        {
            try
            {
                using var streamReader = new StreamReader(namedPipeClientStream);
                using var streamWriter = new StreamWriter(namedPipeServerStream);

                var charachter = new Charachter("PlayerTwo");
                Console.WriteLine($"PlayerTwo HP: {charachter.HealthPoints} Armor: {charachter.Armor}");

                while (true)
                {
                    var json = JsonConvert.SerializeObject(charachter);

                    streamWriter.WriteLine(json);
                    streamWriter.Flush();

                    var handle = Handle(streamReader, charachter);

                    if (!handle)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex) when (_gameHost.IsPipeBrokenRelatedException(ex))
            {
                // pipe breaks hell goes lose, ignore
            }
        }

        private bool Handle(StreamReader streamReader, Charachter charachter)
        {
            var message = streamReader.ReadLine();
            var enemyCharachter = JsonConvert.DeserializeObject<Charachter>(message!)!;
            var currentCharachterHp = charachter.HealthPoints;

            charachter.ManageEnemyCharachter(enemyCharachter);

            var charachterHpAfterAttack = charachter.HealthPoints;
            var totalDamage = currentCharachterHp - charachterHpAfterAttack;

            var trackedHp = charachter.HealthPoints;
            var trackedArmor = charachter.Armor;

            charachter.TakeTurn();

            var turnStats = new TurnStats(totalDamage, trackedHp, trackedArmor);
            var resultsDisplayer = new ResultsDisplayer(charachter, enemyCharachter, turnStats);

            resultsDisplayer.DisplayTurnResults();

            return GetHandleResult(charachter, enemyCharachter);
        }
        

        private bool GetHandleResult(Charachter charachter, Charachter enemyCharachter)
        {
            if (enemyCharachter.HealthPoints < 0)
            {
                _gameHost.SendMatchResult("PlayerTwo Won");
                return false;
            }

            if (charachter.HealthPoints < 0)
            {
                _gameHost.SendMatchResult("PlayerTwo Lost");
                return false;
            }

            return true;
        }
    }
}
