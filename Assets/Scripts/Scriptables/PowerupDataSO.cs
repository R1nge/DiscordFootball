using Unity.Netcode;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Powerup Data", menuName = "Powerup Data")]
    public class PowerupDataSO : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private NetworkObject spawnablePrefab;

        public Sprite Icon => icon;
        public string Name => name;
        public string Description => description;
        public NetworkObject SpawnablePrefab => spawnablePrefab;
    }
}