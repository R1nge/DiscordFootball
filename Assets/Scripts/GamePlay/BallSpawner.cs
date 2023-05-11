using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using Zenject;

namespace GamePlay
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private Ball ballPrefab;
        [SerializeField] private Vector3[] positions;
        private DiContainer _diContainer;
        private RoundManager _roundManager;

        [Inject]
        private void Construct(DiContainer diContainer, RoundManager roundManager)
        {
            _diContainer = diContainer;
            _roundManager = roundManager;
        }

        private void Awake()
        {
            _roundManager.OnPreStartEvent += SpawnBall;
        }

        private void SpawnBall()
        {
            SpawnBall(_roundManager.GetLastWonTeam());
        }

        private void SpawnBall(Roles? lastWonTeam)
        {
            if (lastWonTeam == null)
            {
                SpawnBall(positions[1]);
                return;
            }

            switch (lastWonTeam.Value)
            {
                case Roles.Red:
                    SpawnBall(positions[0]);
                    break;
                case Roles.Blue:
                    SpawnBall(positions[2]);
                    break;
                case Roles.Spectator:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SpawnBall(Vector3 position)
        {
            //TODO: check if it works in multiplayer
            var inst = _diContainer.InstantiatePrefabForComponent<Ball>(ballPrefab, position, Quaternion.identity, null);
            _diContainer.InstantiateComponent<NetworkObject>(inst.gameObject);
            _diContainer.InstantiateComponent<NetworkTransform>(inst.gameObject);
            _diContainer.InstantiateComponent<NetworkRigidbody>(inst.gameObject);
            inst.GetComponent<NetworkObject>().Spawn();
        }

        private void OnDestroy()
        {
            _roundManager.OnPreStartEvent -= SpawnBall;
        }
    }
}