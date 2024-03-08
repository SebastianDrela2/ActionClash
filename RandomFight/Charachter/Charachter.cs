namespace RandomFight.Charachter
{
    public class Charachter
    {
        private const int HealAmmount = 20;

        private readonly char _blackSquare = '\u25A0';
        private readonly int _originalHp;          
        private readonly string _playerName;

        private int _trackedHp;
        private int _trackedArmor;

        public int HealthPoints;                
        public int Armor;
        public string AttackType;
        public int Damage;

        public Charachter(string playerName)
        {
            var random = new Random();

            HealthPoints = random.Next(1, 1000);                       
            Armor = random.Next(1, 5);

            _originalHp = HealthPoints;            
            _playerName = playerName;
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

            _trackedHp = HealthPoints;
            _trackedArmor = Armor;

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

        private void Heal() => HealthPoints += HealAmmount;

        private void IncreaseArmor() => Armor += 1;

        public void DisplayTurnResults(Charachter enemyCharachter, int totalDamage)
        {
            Console.Clear();
            Console.WriteLine($"PlayerName: {_playerName}");
            Console.WriteLine($"Total Hp: {_originalHp}");
            Console.WriteLine($"Armor: {Armor} ");
            Console.WriteLine();

            DisplaySpecialRoll();

            Console.WriteLine($"Got hit with {enemyCharachter.AttackType}!");            
            Console.WriteLine($"Took {totalDamage} left {HealthPoints} HP");

            Console.WriteLine();
            RenderHpBar();                       
        }

        private void DisplaySpecialRoll()
        {
            if (_trackedHp < HealthPoints)
            {
                Console.WriteLine($"Healed for {HealAmmount}");
            }
            else if (_trackedArmor < Armor)
            {
                Console.WriteLine($"Increased armor by 1!");
            }
        }

        private void RenderHpBar()
        {
            var hpBarLength = HealthPoints / 10;

            Console.Write("HP: ");
            for (var i = 0; i < hpBarLength; i++)
            {
               Console.Write(_blackSquare);
            }
        }
    }
}
