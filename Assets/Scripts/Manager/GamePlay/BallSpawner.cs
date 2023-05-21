using System;
using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using View;

namespace Manager.GamePlay
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private Ball ballPrefab;
        [SerializeField] private Vector3[] positions;
        private RoundManager _roundManager;
        private IObjectResolver _objectResolver;

        [Inject]
        private void Construct(IObjectResolver objectResolver, RoundManager roundManager)
        {
            _objectResolver = objectResolver;
            _roundManager = roundManager;
        }

        private void Awake()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            _roundManager.OnPreStartEvent += SpawnBall;
        }

        private void SpawnBall()
        {
            SpawnBall(_roundManager.GetLastWonTeamRoles());
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
                case Roles.None:
                    SpawnBall(positions[1]);
                    break;
                case Roles.Red:
                    SpawnBall(positions[0]);
                    break;
                case Roles.Blue:
                    SpawnBall(positions[2]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SpawnBall(Vector3 position)
        {
            var inst = _objectResolver.Instantiate(ballPrefab, position, Quaternion.identity);
            inst.GetComponent<NetworkObject>().Spawn();
        }

        private void OnDestroy()
        {
            _roundManager.OnPreStartEvent -= SpawnBall;
        }
    }
}