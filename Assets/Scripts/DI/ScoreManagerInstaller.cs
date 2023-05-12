using System;
using GamePlay;
using Zenject;

namespace DI
{
    public class ScoreManagerInstaller : MonoInstaller
    {
        private readonly ScoreManager _scoreManager = new();

        public override void InstallBindings()
        {
            Container.Bind(typeof(ScoreManager), typeof(IInitializable), typeof(IDisposable)).To<ScoreManager>().AsSingle();
            Container.QueueForInject(_scoreManager);
        }
    }
}