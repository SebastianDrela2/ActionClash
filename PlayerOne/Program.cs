namespace PlayerOne
{
    internal class Program
    {       
        static void Main()
        {
            ConsoleHelper.SetCurrentFont("Comic Sans", 20);

            var playerOne = new PlayerOne();            
            playerOne.SetUpPlayer();
        }              
    }
}
