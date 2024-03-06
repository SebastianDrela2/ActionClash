using Newtonsoft.Json;
using RandomFight.Charachter;
using RandomFight.Match;
using System.IO.Pipes;

namespace PlayerTwo
{
    public class PlayerTwo
    {
        private readonly GameHost _gameHost = new GameHost();
        public void SetUpPlayer()
        {
            using var namedPipeServerStream = new NamedPipeServerStream("PlayerTwo");
            using var namedPipeClientStream = new NamedPipeClientStream("PlayerOne");

            Console.WriteLine($"Waiting for PlayerOne...");

            namedPipeClientStream.Connect();
            namedPipeServerStream.WaitForConnection();

            Console.WriteLine("Connected.");

            try
            {
                using var streamReader = new StreamReader(namedPipeClientStream);
                using var streamWriter = new StreamWriter(namedPipeServerStream);

                var charachter = new Charachter();
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
            catch
            {
                // pipe breaks hell goes lose, ignore
            }

            Console.ReadKey();
        }

        private bool Handle(StreamReader streamReader, Charachter charachter)
        {
            var message = streamReader.ReadLine();
            var enemyCharachter = JsonConvert.DeserializeObject<Charachter>(message!)!;
            var currentCharachterHp = charachter.HealthPoints;

            charachter.ManageEnemyCharachter(enemyCharachter);

            var charachterHpAfterAttack = charachter.HealthPoints;
            var totalDamage = currentCharachterHp - charachterHpAfterAttack;

            charachter.DisplayAttackResults(enemyCharachter, totalDamage);
            charachter.PrepareNewAttack();

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
