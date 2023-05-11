using GamePlay;
using UnityEngine;
using Zenject;

namespace DI
{
    public class BallSpawnerInstaller : MonoInstaller
    {
        [SerializeField] private BallSpawner ballSpawner;

        public override void InstallBindings()
        {
            var inst = Container.InstantiatePrefabForComponent<BallSpawner>(ballSpawner);
            Container.Bind<BallSpawner>().FromInstance(inst).AsSingle();
            Container.QueueForInject(ballSpawner);
        }
    }
}