namespace RandomFight.Player
{
    public class AttackInformation
    {
        private readonly Dictionary<string, int> _embeddedAttacks = new Dictionary<string, int>()
        {
            { "Rock", 5 },
            { "Snowball", 10 },
            { "FireBall", 20 },
            { "Meteor", 25 },
            { "Sword", 40 },
            { "DarkMagic", 60 }
        };

        public readonly string Type;
        public readonly int Damage;

        public AttackInformation()
        {
            var attackInformation = GetAttackInformation();

            Type = attackInformation.Key;
            Damage = attackInformation.Value;
        }
        
        private KeyValuePair<string,int> GetAttackInformation()
        {
            var random = new Random();
            var index = random.Next(0, _embeddedAttacks.Count);

            var kvp = _embeddedAttacks.ElementAt(index);

            return new KeyValuePair<string, int>(kvp.Key, kvp.Value);
         }

    }  
}
