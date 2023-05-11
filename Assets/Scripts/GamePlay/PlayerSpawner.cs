using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

        public async UniTask SpawnPlayer(Roles role, Vector3[] position)
        {
            await UniTask.WaitUntil(() => _canSpawn);

            for (int i = 0; i < position.Length; i++)
            {
                if (role == Roles.Blue)
                {
                    position[i].x *= -1;
                }

                //Just ignore this error, because Zenject doesn't allow to spawn a gameObject without changing its parent
                var player = _container.InstantiatePrefabForComponent<PlayerTeam>(playerPrefab, position[i], quaternion.identity, null);
                player.SetTeam(role);
                player.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}