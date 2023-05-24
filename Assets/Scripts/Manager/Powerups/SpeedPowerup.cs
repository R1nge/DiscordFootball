using Scriptables;
using UnityEngine;

namespace Manager.Powerups
{
    public class SpeedPowerup : Powerup
    {
        private readonly PowerupDataSO _powerupDataSo;

        public SpeedPowerup(PowerupDataSO powerupDataSo)
        {
            _powerupDataSo = powerupDataSo;
        }

        public override PowerupDataSO GetPowerupData() => _powerupDataSo;

        public override void Use(ulong playerId)
        {
            Debug.Log($"{playerId} : Used speed powerup");
        }
    }
}