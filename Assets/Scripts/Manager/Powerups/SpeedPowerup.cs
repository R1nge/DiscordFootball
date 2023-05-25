using Scriptables;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Manager.Powerups
{
    public class SpeedPowerup : Powerup
    {
        private readonly IObjectResolver _objectResolver;
        private readonly PowerupDataSO _powerupDataSo;

        public SpeedPowerup(IObjectResolver objectResolver, PowerupDataSO powerupDataSo)
        {
            _objectResolver = objectResolver;
            _powerupDataSo = powerupDataSo;
        }

        public override PowerupDataSO GetPowerupData() => _powerupDataSo;

        public override void Use(ulong playerId)
        {
            //TODO: add ability to place it
            Debug.Log($"{playerId} : Used speed powerup");
            var inst = _objectResolver.Instantiate(_powerupDataSo.SpawnablePrefab);
            inst.Spawn();
        }
    }
}