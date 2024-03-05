namespace RandomFight.Charachter
{
    public class Charachter
    {
        public int HealthPoints;                
        public int Armor;
        public string AttackType;
        public int Damage;

        public Charachter()
        {
            var random = new Random();

            HealthPoints = random.Next(1, 1000);                       
            Armor = random.Next(1, 5);
        }

        public void ManageEnemyCharachter(Charachter enemyCharachter)
        {
            int totalDamage = 0;
            if (enemyCharachter.Damage - enemyCharachter.Armor > 0)
            {
                totalDamage = enemyCharachter.Damage - enemyCharachter.Armor;
            }
            HealthPoints -= totalDamage;            
        }

        public void PrepareNewAttack()
        {
            var attackInformation = new AttackInformation();
            AttackType = attackInformation.Type;
            Damage = attackInformation.Damage;
        }

        public void DisplayAttackResults(Charachter enemyCharachter, int totalDamage)
        {
            Console.WriteLine($"Got hit with {enemyCharachter.AttackType}!");
            Thread.Sleep(1000);
            Console.WriteLine($"Took {totalDamage} left {HealthPoints} HP");
        }
    }
}
