using Scriptables;

namespace Manager.Powerups
{
    public abstract class Powerup
    {
        public abstract PowerupDataSO GetPowerupData();

        public abstract void Use(ulong playerId);
    }
}