using Newtonsoft.Json;
using RandomFight.Charachter;
using System.IO;
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

                    var handle = Handle(streamReader, streamWriter, charachter, json);

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

        private static bool Handle(StreamReader streamReader, StreamWriter streamWriter, Charachter charachter, string json)
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
                var namedPipeServerStream = new NamedPipeClientStream("GameHost");
                using var hostResponseWriter = new StreamWriter(namedPipeServerStream);

                namedPipeServerStream.Connect();
                hostResponseWriter.WriteLine("PlayerTwo Won");
                hostResponseWriter.Flush();
                Thread.Sleep(1000);

                return false;
            }

            if (charachter.HealthPoints < 0)
            {
                var namedPipeServerStream = new NamedPipeClientStream("GameHost");
                using var hostResponseWriter = new StreamWriter(namedPipeServerStream);

                namedPipeServerStream.Connect();
                hostResponseWriter.WriteLine("PlayerTwo lost");
                hostResponseWriter.Flush();
                Thread.Sleep(1000);

                return false;
            }

            return true;
        }
    }
}
