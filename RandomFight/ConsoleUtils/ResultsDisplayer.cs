using RandomFight.Player;

namespace RandomFight.ConsoleUtils
{
    public class ResultsDisplayer
    {
        private readonly char _blackSquare = '\u25A0'; 
        
        private readonly Charachter _charachter;
        private readonly Charachter _enemyCharachter;
        private readonly TurnStats _turnStats;
        private readonly int _hpBarLength;

        public ResultsDisplayer(Charachter charachter, Charachter enemyCharachter, TurnStats turnStats)
        {
            _charachter = charachter;
            _enemyCharachter = enemyCharachter;
            _turnStats = turnStats;
            _hpBarLength = charachter.HealthPoints / 10;
        }

        public void DisplayTurnResults()
        {
            Console.Clear();
            Console.WriteLine($"PlayerName: {_charachter.PlayerName}");
            Console.WriteLine($"Total Hp: {_charachter.OriginalHp}");
            Console.WriteLine($"Armor: {_charachter.Armor} ");
            Console.WriteLine();
            
            Console.WriteLine($"Got hit with {_enemyCharachter.AttackType}!");
            Console.WriteLine($"Took {_turnStats.TotalDamage} left {_charachter.HealthPoints} HP");
          
            RenderHpBar();
            DisplaySpecialRoll();
        }

        private void DisplaySpecialRoll()
        {
            if (_turnStats.TrackedHp < _charachter.HealthPoints)
            {
                Console.WriteLine($"Healed for {_charachter.HealAmmount}");
            }
            else if (_turnStats.TrackedArmor < _charachter.Armor)
            {
                Console.WriteLine($"Increased armor by 1!");
            }
        }

        private void RenderHpBar()
        {
            Console.WriteLine();
            Console.Write("HP: ");

            for (var i = 0; i < _hpBarLength; i++)
            {
                Console.Write(_blackSquare);
            }

            Console.WriteLine();
        }
    }
}
