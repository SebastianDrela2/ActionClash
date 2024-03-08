namespace RandomFight.ConsoleUtils
{
    public struct TurnStats
    {
        public int TotalDamage;
        public int TrackedHp;
        public int TrackedArmor;

        public TurnStats(int totalDamage, int trackedHp, int trackedArmor)
        {
            TotalDamage = totalDamage;
            TrackedHp = trackedHp;
            TrackedArmor = trackedArmor;
        }
    }
}
