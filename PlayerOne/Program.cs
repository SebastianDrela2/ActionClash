using Newtonsoft.Json;
using RandomFight.Charachter;
using System.IO;
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

            try
            {
                using var streamReader = new StreamReader(namedPipeClientStream);
                using var streamWriter = new StreamWriter(namedPipeServerStream);

                var charachter = new Charachter();

                Console.WriteLine($"PlayerOne HP: {charachter.HealthPoints} Armor: {charachter.Armor}");

                while (true)
                {
                    var (json, handle) = Handle(streamReader, streamWriter, charachter);

                    if (!handle)
                    {
                        break;
                    }

                    streamWriter.WriteLine(json);
                    streamWriter.Flush();
                }
            }
            catch
            {
                // pipe breaks hell goes lose, ignore
            }

            Console.ReadKey();
        }

        private static (string, bool) Handle(StreamReader streamReader, StreamWriter streamWriter, Charachter charachter)
        {
            var message = streamReader.ReadLine();
            var enemyCharachter = JsonConvert.DeserializeObject<Charachter>(message!)!;

            var currentCharachterHp = charachter.HealthPoints;

            charachter.ManageEnemyCharachter(enemyCharachter);

            var charachterHpAfterAttack = charachter.HealthPoints;
            var totalDamage = currentCharachterHp - charachterHpAfterAttack;                   
          
            charachter.DisplayAttackResults(enemyCharachter, totalDamage);
            charachter.PrepareNewAttack();

            var json = JsonConvert.SerializeObject(charachter);


            if (enemyCharachter.HealthPoints < 0)
            {
                var namedPipeServerStream = new NamedPipeClientStream("GameHost");
                using var hostResponseWriter = new StreamWriter(namedPipeServerStream);

                hostResponseWriter.WriteLine("PlayerOne Won");
                hostResponseWriter.Flush();

                Thread.Sleep(1000);
                return (string.Empty, false);
            }

            if (charachter.HealthPoints < 0)
            {
                var namedPipeServerStream = new NamedPipeClientStream("GameHost");
                using var hostResponseWriter = new StreamWriter(namedPipeServerStream);

                hostResponseWriter.WriteLine("PlayerOne lost");
                hostResponseWriter.Flush();

                Thread.Sleep(1000);
                return (string.Empty, false);
            }

            return (json, true);
        }
    }
}
