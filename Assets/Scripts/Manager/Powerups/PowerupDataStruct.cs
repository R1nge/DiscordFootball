using System;

namespace Manager.Powerups
{
    [Serializable]
    public struct PowerupDataStruct
    {
        public PowerupDataStruct(Powerup powerup, ulong ownerId)
        {
            Powerup = powerup;
            OwnerId = ownerId;
        }

        public Powerup Powerup;
        public ulong OwnerId;
    }
}