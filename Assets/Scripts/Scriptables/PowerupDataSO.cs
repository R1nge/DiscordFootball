using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Powerup Data", menuName = "Powerup Data")]
    public class PowerupDataSO : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string name;
        [SerializeField] private string description;

        public Sprite Icon => icon;
        public string Name => name;
        public string Description => description;
    }
}