using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GamePlay
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerPrefab;
        private bool _canSpawn;
        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _container = diContainer;
        }

        private void Awake()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManagerOnOnLoadEventCompleted;
        }

        private void SceneManagerOnOnLoadEventCompleted(string scenename, LoadSceneMode loadscenemode,
            List<ulong> clientscompleted, List<ulong> clientstimedout)
        {
            _canSpawn = true;
        }

        public IEnumerator SpawnPlayer(Teams team, Vector3[] position)
        {
            yield return new WaitUntil(() => _canSpawn);

            for (int i = 0; i < position.Length; i++)
            {
                if (team == Teams.Blue)
                {
                    position[i].x *= -1;
                }

                var player = _container.InstantiatePrefabForComponent<PlayerTeam>(playerPrefab, position[i], quaternion.identity, null);
                player.SetTeam(team);
                player.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}