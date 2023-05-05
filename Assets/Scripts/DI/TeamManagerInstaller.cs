using GamePlay;
using Zenject;

namespace DI
{
    public class TeamManagerInstaller : MonoInstaller
    {
        private TeamManager _teamManager;

        public override void InstallBindings()
        {
            Container.Bind<TeamManager>().FromNew().AsSingle();
            Container.QueueForInject(_teamManager);
        }
    }
}