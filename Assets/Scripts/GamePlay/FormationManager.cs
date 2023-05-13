using System;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using Zenject;

namespace GamePlay
{
    public class FormationManager : MonoBehaviour
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

        public async UniTask SelectFormation(int index, ulong playerId)
        {
            Vector3[] positionsVectors = new Vector3[4];

            for (int i = 0; i < positionsVectors.Length; i++)
            {
                positionsVectors[i] = positions[index].positionsArray[i].position;
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
                Debug.LogError("Team is null", this);
                return;
            }
            
            switch (team.Roles)
            {
                case Roles.Red:
                    await _playerSpawner.SpawnPlayer(Roles.Red, positionsVectors);
                    break;
                case Roles.Blue:
                    await _playerSpawner.SpawnPlayer(Roles.Blue, positionsVectors);
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