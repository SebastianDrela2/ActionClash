namespace PlayerOne
{
    internal class Program
    {       
        static void Main()
        {
            var playerOne = new PlayerOne();
            ConsoleHelper.SetCurrentFont("Comic Sans", 20);
            playerOne.SetUpPlayer();
        }              
    }
}
