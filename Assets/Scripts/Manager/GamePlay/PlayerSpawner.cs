using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;
using View.Player;

namespace Manager.GamePlay
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private PlayerTeam playerPrefab;
        private bool _canSpawn;
        private IObjectResolver _objectResolver;

        [Inject]
        private void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
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

        public void SpawnPlayer(Roles role, Vector3[] position)
        {
            //await UniTask.WaitUntil(() => _canSpawn);
            if (NetworkManager.Singleton.IsServer)
            {
                SpawnPlayers(role, position);
            }
            else
            {
                SpawnPlayersServerRpc(role, position);
            }
        }

        private void SpawnPlayers(Roles role, Vector3[] position)
        {
            for (int i = 0; i < position.Length; i++)
            {
                if (role == Roles.Blue)
                {
                    position[i].x *= -1;
                }

                //TODO: check if it works in multiplayer; not it's not
                var player = _objectResolver.Instantiate(playerPrefab, position[i], quaternion.identity);
                player.SetTeam(role);
                player.GetComponent<NetworkObject>().Spawn();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnPlayersServerRpc(Roles role, Vector3[] position, ServerRpcParams rpcParams = default)
        {
            SpawnPlayers(role, position);
        }
    }
}