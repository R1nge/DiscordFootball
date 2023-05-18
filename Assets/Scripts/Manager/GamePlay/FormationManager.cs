using System;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using VContainer;
using View.Player;

namespace Manager.GamePlay
{
    public class FormationManager : NetworkBehaviour
    {
        [SerializeField] private Positions[] positions;
        private PlayerSpawner _playerSpawner;
        private TeamManager _teamManager;
        private RoundManager _roundManager;

        [Inject]
        private void Construct(TeamManager teamManager, PlayerSpawner playerSpawner, RoundManager roundManager)
        {
            _teamManager = teamManager;
            _playerSpawner = playerSpawner;
            _roundManager = roundManager;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SelectFormationServerRpc(int index, ulong playerId)
        {
            Vector3[] positionsVectors = new Vector3[4];

            for (int i = 0; i < positionsVectors.Length; i++)
            {
                positionsVectors[i] = positions[index].positionsArray[i].position;
                //positionsVectors[i] = new Vector3(3 * i, 1, 3 * i);
            }

            //Remove players
            //Can make players of opponent's team invisible, then delete with exclusion, make visible
            var players = FindObjectsByType<PlayerMovement>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            var len = players.Length;
            // for (int i = 0; i < len; i++)
            // {
            //     Destroy(players[i].gameObject);
            //     //use despawn
            // }

            var team = _teamManager.GetTeam(playerId);
            if (team == null)
            {
                Debug.LogError("FormationManager: Team is null", this);
                return;
            }
            
            switch (team.Roles)
            {
                case Roles.Red:
                    _playerSpawner.SpawnPlayer(Roles.Red, positionsVectors);
                    break;
                case Roles.Blue:
                    _playerSpawner.SpawnPlayer(Roles.Blue, positionsVectors);
                    break;
                case Roles.Spectator:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _roundManager.StartRound();
        }
    }
}