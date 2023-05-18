using Manager.GamePlay;
using Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using View;

namespace Scope
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private RigidbodiesManager rigidbodiesManager;
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private FormationManager formationManager;
        [SerializeField] private TurnManager turnManager;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(rigidbodiesManager);
            builder.RegisterComponent(formationManager);
            builder.RegisterComponent(playerSpawner);
            builder.RegisterComponent(turnManager);
            //builder.Register<TeamManager>(Lifetime.Singleton);
            builder.Register<RoundManager>(Lifetime.Singleton);
            builder.Register<BallSpawner>(Lifetime.Singleton);
            builder.Register<ScoreManager>(Lifetime.Singleton);
            builder.Register<BallService>(Lifetime.Singleton);
            builder.Register<GoalService>(Lifetime.Singleton);
            builder.Register<FormationUIService>(Lifetime.Singleton);
            builder.Register<ScoreUIService>(Lifetime.Singleton);
            //builder.Register<TeamManagerUIService>(Lifetime.Singleton);
            builder.Register<TimerUIService>(Lifetime.Singleton);
        }
    }
}