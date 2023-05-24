using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace Manager.GamePlay
{
    public class FormationManager : NetworkBehaviour
    {
        [SerializeField] private Positions[] positions;
        private PlayerSpawner _playerSpawner;
        private TeamManager _teamManager;

        [Inject]
        private void Construct(TeamManager teamManager, PlayerSpawner playerSpawner)
        {
            _teamManager = teamManager;
            _playerSpawner = playerSpawner;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SelectFormationServerRpc(int index, ulong playerId)
        {
            var positionsVectors = new Vector3[4];

            for (var i = 0; i < positionsVectors.Length; i++)
            {
                positionsVectors[i] = positions[index].positionsArray[i].position;
            }

            var team = _teamManager.GetTeam(playerId);
            if (team == null)
            {
                Debug.LogError("FormationManager: Team is null", this);
                return;
            }

            switch (team.Roles)
            {
                case Roles.Red:
                    _playerSpawner.SpawnPlayers(Roles.Red, positionsVectors, playerId);
                    break;
                case Roles.Blue:
                    _playerSpawner.SpawnPlayers(Roles.Blue, positionsVectors, playerId);
                    break;
            }
        }
    }
}