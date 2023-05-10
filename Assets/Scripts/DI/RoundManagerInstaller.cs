using GamePlay;
using Zenject;

namespace DI
{
    public class RoundManagerInstaller : MonoInstaller
    {
        private readonly RoundManager _roundManager = new();

        public override void InstallBindings()
        {
            Container.Bind<RoundManager>().FromNew().AsSingle();
            Container.QueueForInject(_roundManager);
        }
    }
}