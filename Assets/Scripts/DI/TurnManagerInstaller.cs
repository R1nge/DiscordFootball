using GamePlay;
using UnityEngine;
using Zenject;

namespace DI
{
    public class TurnManagerInstaller : MonoInstaller
    {
        [SerializeField] private TurnManager turnManagerPrefab;

        public override void InstallBindings()
        {
            var turnManager = Container.InstantiatePrefabForComponent<TurnManager>(turnManagerPrefab);
            Container.Bind<TurnManager>().FromInstance(turnManager).AsSingle();
            Container.QueueForInject(turnManagerPrefab);
        }
    }
}