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
            var attackInformation = new AttackInformation();

            var totalDamage = attackInformation.Damage - enemyCharachter.Armor;
            charachter.HealthPoints -= totalDamage;

            Console.WriteLine($"Got hit with {attackInformation.Type}!");
            Thread.Sleep(1000);
            Console.WriteLine($"Took {totalDamage} left {charachter.HealthPoints}");

            if (enemyCharachter.HealthPoints < 0)
            {
                Console.WriteLine("PlayerTwo won");

                return false;
            }

            if (charachter.HealthPoints < 0)
            {
                Console.WriteLine("PlayerTwo Lost");

                streamWriter.WriteLine(json);
                streamWriter.Flush();

                return false;
            }

            return true;
        }
    }
}
