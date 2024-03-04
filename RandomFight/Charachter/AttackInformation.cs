namespace RandomFight.Charachter
{
    public class AttackInformation
    {
        public readonly string Type;
        public readonly int Damage;

        public AttackInformation()
        {
            var attackInformation = GetAttackInformation();

            Type = attackInformation.Key;
            Damage = attackInformation.Value;
        }

        private readonly Dictionary<string, int> _embeddedAttacks = new Dictionary<string, int>()
        {
            { "Rock", 2},
            { "Snowball", 5},
            { "FireBall", 10},
            { "Sword", 20},
            { "DarkMagic", 30 },       
        };

        private KeyValuePair<string,int> GetAttackInformation()
        {
            var random = new Random();
            var index = random.Next(1, _embeddedAttacks.Count);

            var kvp = _embeddedAttacks.ElementAt(index);

            return new KeyValuePair<string, int>(kvp.Key, kvp.Value);
         }

    }  
}
