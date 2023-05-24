using System;
using System.Collections.Generic;
using Manager.Powerups;

namespace Manager.GamePlay
{
    public class PowerupManager
    {
        public event Action<string, ulong> OnPowerupChangedEvent;
        public event Action<ulong> OnPowerupUsedEvent;
        private readonly List<PowerupDataStruct> _powerups = new();

        public void AddPowerup(ulong playerID, Powerup powerup)
        {
            var powerupData = new PowerupDataStruct
            {
                Powerup = powerup,
                OwnerId = playerID
            };

            if (_powerups.Contains(powerupData))
            {
                for (var i = 0; i < _powerups.Count; i++)
                {
                    if (_powerups[i].OwnerId == playerID)
                    {
                        _powerups[i] = powerupData;
                        break;
                    }
                }
            }
            else
            {
                _powerups.Add(powerupData);
            }

            OnPowerupChangedEvent?.Invoke(powerup.GetPowerupData().Name, playerID);
        }

        public void UsePowerup(ulong playerID)
        {
            for (var i = 0; i < _powerups.Count; i++)
            {
                if (_powerups[i].OwnerId == playerID)
                {
                    _powerups[i].Powerup.Use(playerID);
                    OnPowerupUsedEvent?.Invoke(playerID);
                    _powerups.Remove(_powerups[i]);
                    break;
                }
            }
        }
    }
}