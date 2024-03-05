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
            { "Rock", 5 },
            { "Snowball", 10 },
            { "FireBall", 20 },
            { "Sword", 40 },
            { "DarkMagic", 60 },       
        };

        private KeyValuePair<string,int> GetAttackInformation()
        {
            var random = new Random();
            var index = random.Next(0, _embeddedAttacks.Count);

            var kvp = _embeddedAttacks.ElementAt(index);

            return new KeyValuePair<string, int>(kvp.Key, kvp.Value);
         }

    }  
}
