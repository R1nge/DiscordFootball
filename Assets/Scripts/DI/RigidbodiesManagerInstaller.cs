using GamePlay;
using UnityEngine;
using Zenject;

namespace DI
{
    public class RigidbodiesManagerInstaller : MonoInstaller
    {
        [SerializeField] private RigidbodiesManager rigidbodiesManager;

        public override void InstallBindings()
        {
            var inst = Container.InstantiatePrefabForComponent<RigidbodiesManager>(rigidbodiesManager);
            Container.Bind<RigidbodiesManager>().FromInstance(inst).AsSingle();
            Container.QueueForInject(rigidbodiesManager);
        }
    }
}