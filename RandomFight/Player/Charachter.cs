namespace RandomFight.Player
{
    public class Charachter
    {                                 
       
        public string PlayerName;
        public int OriginalHp;
        public int HealthPoints;        
        public int Armor;
        public string AttackType;
        public int Damage;

        public Charachter(string playerName)
        {
            var random = new Random(); 
            
            PlayerName = playerName;
            HealthPoints = random.Next(1, 1000);
            OriginalHp = HealthPoints;
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

        public void TakeTurn()
        {
            var random = new Random();
            var number = random.Next(1, 10);
           
            if (number == 1)
            {
                Heal();              
            }
            else if (number == 2)
            {
                IncreaseArmor();
            }

            PrepareNewAttack();

            Thread.Sleep(1000);
        }

        private void PrepareNewAttack()
        {
            var attackInformation = new AttackInformation();

            AttackType = attackInformation.Type;
            Damage = attackInformation.Damage;
        }

        private void Heal() => HealthPoints += 20;
        private void IncreaseArmor() => Armor += 1;       
    }
}
