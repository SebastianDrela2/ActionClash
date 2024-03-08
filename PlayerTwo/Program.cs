namespace PlayerTwo
{
    internal class Program
    {
        static void Main()
        {
            ConsoleHelper.SetCurrentFont("Comic Sans", 20);

            var playerTwo = new PlayerTwo();            
            playerTwo.SetUpPlayer();
        }      
    }
}
