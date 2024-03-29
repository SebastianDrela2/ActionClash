﻿using Newtonsoft.Json;
using RandomFight.ConsoleUtils;
using RandomFight.Match;
using RandomFight.Player;
using System.IO.Pipes;

namespace PlayerOne
{
    internal class PlayerOne
    {
        private readonly GameHost _gameHost = new GameHost();

        public void SetUpPlayer()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            using var namedPipeServerStream = new NamedPipeServerStream("PlayerOne");
            using var namedPipeClientStream = new NamedPipeClientStream("PlayerTwo");

            Console.WriteLine($"Waiting for PlayerTwo...");

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

                var charachter = new Charachter("PlayerOne");

                Console.WriteLine($"PlayerOne HP: {charachter.HealthPoints} Armor: {charachter.Armor}");

                while (true)
                {
                    var (json, handle) = Handle(streamReader, charachter);

                    if (!handle)
                    {
                        break;
                    }

                    streamWriter.WriteLine(json);
                    streamWriter.Flush();
                }
            }
            catch (Exception ex) when (_gameHost.IsPipeBrokenRelatedException(ex))
            {
                // pipe breaks hell goes lose, ignore
            }
        }
        
        private (string, bool) Handle(StreamReader streamReader, Charachter charachter)
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
                   

            var json = JsonConvert.SerializeObject(charachter);

            return GetHandleResult(charachter, enemyCharachter, json);
        }

        private (string, bool) GetHandleResult(Charachter charachter, Charachter enemyCharachter, string json)
        {
            if (enemyCharachter.HealthPoints < 0)
            {
                _gameHost.SendMatchResult("PlayerOne Won");

                return (string.Empty, false);
            }

            if (charachter.HealthPoints < 0)
            {
                _gameHost.SendMatchResult("PlayerOne Lost");

                return (string.Empty, false);
            }

            return (json, true);
        }
    }
}
