using UnityEngine;

namespace Player
{
    public class PlayerTeam : MonoBehaviour
    {
        [SerializeField] private Roles role;

        public Roles GetTeam() => role;
        
        public void SetTeam(Roles newRole) => role = newRole;
    }
}