namespace RandomFight.Charachter
{
    public class Charachter
    {
        public int HealthPoints;                
        public int Armor;

        public Charachter()
        {
            var random = new Random();

            HealthPoints = random.Next(1, 1000);                       
            Armor = random.Next(1, HealthPoints/8);
        }
    }
}
