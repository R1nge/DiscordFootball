using UnityEngine;

namespace View.Player
{
    public class PlayerTeam : MonoBehaviour
    {
        [SerializeField] private Roles role;

        public Roles GetTeam() => role;
        
        public void SetTeam(Roles newRole) => role = newRole;
    }
}