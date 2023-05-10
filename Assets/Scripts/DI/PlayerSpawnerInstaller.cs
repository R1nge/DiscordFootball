using GamePlay;
using UnityEngine;
using Zenject;

namespace DI
{
    public class PlayerSpawnerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerSpawner playerSpawner;

        public override void InstallBindings()
        {
            var inst = Container.InstantiatePrefabForComponent<PlayerSpawner>(playerSpawner);
            Container.Bind<PlayerSpawner>().FromInstance(inst).AsSingle();
            Container.QueueForInject(playerSpawner);
        }
    }
}