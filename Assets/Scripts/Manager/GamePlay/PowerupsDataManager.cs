using System.Collections.Generic;
using Scriptables;
using UnityEngine;

namespace Manager.GamePlay
{
    public class PowerupsDataManager : MonoBehaviour
    {
        [SerializeField] private PowerupDataSO[] powerupData;
        private readonly Dictionary<string, PowerupDataSO> _dictionary = new();

        private void Awake()
        {
            for (var i = 0; i < powerupData.Length; i++)
            {
                _dictionary.Add(powerupData[i].Name, powerupData[i]);
            }
        }

        public PowerupDataSO GetPowerupData(string name) => _dictionary[name];
    }
}