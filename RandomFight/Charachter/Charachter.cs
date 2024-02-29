namespace RandomFight.Charachter
{
    public class Charachter
    {
        public int HealthPoints;        
        public int Damage;
        public int Armor;

        public Charachter()
        {
            var random = new Random();

            HealthPoints = random.Next(1, 100);           
            Damage = random.Next(1, 100);
            Armor = random.Next(1, Damage/4);
        }
    }
}
