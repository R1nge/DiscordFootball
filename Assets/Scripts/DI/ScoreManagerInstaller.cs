using GamePlay;
using Zenject;

namespace DI
{
    public class ScoreManagerInstaller : MonoInstaller
    {
        private readonly ScoreManager _scoreManager = new();

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ScoreManager>().AsSingle();
            Container.QueueForInject(_scoreManager);
        }
    }
}