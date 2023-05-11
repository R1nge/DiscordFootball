using System;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace GamePlay
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private Ball ballPrefab;
        [SerializeField] private Vector3[] positions;
        private DiContainer _diContainer;
        
        //TODO: Depending on which team has won, spawn a ball in front of a lost team
        //3 positions: left side, center, right side;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public void SpawnBall(Roles? lastWonTeam)
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
            var inst = _diContainer.InstantiatePrefabForComponent<Ball>(ballPrefab, position, Quaternion.identity, null);
            inst.GetComponent<NetworkObject>().Spawn();
        }
    }
}