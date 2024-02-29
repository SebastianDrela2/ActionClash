using Newtonsoft.Json;
using RandomFight.Charachter;
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

                while (true)
                {
                    var message = streamReader.ReadLine();
                    var enemyCharachter = JsonConvert.DeserializeObject<Charachter>(message!)!;

                    var totalDamage = enemyCharachter.Damage - charachter.Armor;
                    charachter.HealthPoints -= totalDamage;

                    var json = JsonConvert.SerializeObject(charachter);

                    Thread.Sleep(1000);

                    Console.WriteLine($"Took {totalDamage} left {charachter.HealthPoints}");


                    if (enemyCharachter.HealthPoints < 0)
                    {
                        Console.WriteLine("You won");

                        break;
                    }

                    if (charachter.HealthPoints < 0)
                    {
                        Console.WriteLine("You Lost");

                        streamWriter.WriteLine(charachter);
                        streamWriter.Flush();

                        break;
                    }

                    streamWriter.WriteLine(json);
                    streamWriter.Flush();
                }
            }
            catch
            {

            }

            Console.ReadKey();
        }
    }
}
