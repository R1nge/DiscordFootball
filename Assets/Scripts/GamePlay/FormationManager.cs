using System;
using Player;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace GamePlay
{
    public class FormationManager : MonoBehaviour
    {
        [SerializeField] private Positions[] positions;
        private PlayerSpawner _playerSpawner;
        private TeamManager _teamManager;

        [Inject]
        private void Construct(TeamManager teamManager)
        {
            _teamManager = teamManager;
        }
        
        private void Awake()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            SelectFormation(0, NetworkManager.Singleton.LocalClientId);
        }

        public void SelectFormation(int index, ulong playerId)
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
            for (int i = 0; i < len; i++)
            {
                Destroy(players[i].gameObject);
            }

            switch (_teamManager.GetTeam(playerId))
            {
                case Teams.Red:
                    StartCoroutine(_playerSpawner.SpawnPlayer(Teams.Red, positionsVectors));
                    break;
                case Teams.Blue:
                    StartCoroutine(_playerSpawner.SpawnPlayer(Teams.Blue, positionsVectors));
                    break;
                case Teams.Spectator:
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}