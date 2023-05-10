using GamePlay;
using Zenject;

namespace DI
{
    public class TeamManagerInstaller : MonoInstaller
    {
        private readonly TeamManager _teamManager = new();

        public override void InstallBindings()
        {
            Container.Bind<TeamManager>().FromNew().AsSingle();
            Container.QueueForInject(_teamManager);
        }
    }
}