using System.Diagnostics;
using System.IO.Pipes;
using System.Reflection;

namespace RandomFight.Match
{
    public class GameHost
    {
        private const string _binDebugNetPath = @"bin\Debug\net8.0";
        public void StartMatch()
        {
            var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var randomFightHeadDirectory = GetRandomFightHeadDirectory(assemblyDirectory!);

            var playerOneExePath = @$"{randomFightHeadDirectory}\PlayerOne\{_binDebugNetPath}\PlayerOne.exe";
            var playerTwoExePath = @$"{randomFightHeadDirectory}\PlayerTwo\{_binDebugNetPath}\PlayerTwo.exe";

            var processStartInfoOne = new ProcessStartInfo
            {
                FileName = playerOneExePath,
                WorkingDirectory = Path.GetDirectoryName(playerOneExePath),
                UseShellExecute = true,
                CreateNoWindow = false
            };

            var processStartInfoTwo = new ProcessStartInfo
            {
                FileName = playerTwoExePath,
                WorkingDirectory = Path.GetDirectoryName(playerTwoExePath),
                UseShellExecute = true,
                CreateNoWindow = false
            };

            Process.Start(processStartInfoOne);
            Thread.Sleep(2000);
            Process.Start(processStartInfoTwo);
        }

        private string GetRandomFightHeadDirectory(string assemblyDirectory)
        {
            while (Path.GetFileName(assemblyDirectory) != "RandomFight")
            {
                assemblyDirectory = Directory.GetParent(assemblyDirectory)!.FullName;
            }

            return Directory.GetParent(assemblyDirectory)!.FullName;
        }

        public void SendMatchResult(string message)
        {
            var namedPipeClientStream = new NamedPipeClientStream("GameHost");
            using var hostResponseWriter = new StreamWriter(namedPipeClientStream);

            namedPipeClientStream.Connect();
            hostResponseWriter.WriteLine(message);
            hostResponseWriter.Flush();
            Thread.Sleep(2000);
        }

        public string GetMatchResult()
        {
            var namedPipeServerStream = new NamedPipeServerStream("GameHost");
            using var streamReader = new StreamReader(namedPipeServerStream);

            namedPipeServerStream.WaitForConnection();
            var matchResult = streamReader.ReadLine();

            return matchResult!;
        }

        public void AnnounceInProgress()
        {
            Console.WriteLine("Match In Progress...");
        }
    }
}
