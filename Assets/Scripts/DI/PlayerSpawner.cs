using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DI
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
            StartCoroutine(SpawnPlayer(NetworkManager.Singleton.LocalClientId));
        }

        private void SceneManagerOnOnLoadEventCompleted(string scenename, LoadSceneMode loadscenemode,
            List<ulong> clientscompleted, List<ulong> clientstimedout)
        {
            _canSpawn = true;
        }

        private IEnumerator SpawnPlayer(ulong clientId)
        {
            yield return new WaitUntil(() => _canSpawn);
            var player = _container.InstantiatePrefab(playerPrefab);
            player.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        }
    }
}