using UnityEngine;

namespace Player
{
    public class PlayerTeam : MonoBehaviour
    {
        [SerializeField] private Teams team;

        public Teams GetTeam() => team;
        
        public void SetTeam(Teams newTeam) => team = newTeam;
    }
}