using Manager.GamePlay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

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
            builder.RegisterEntryPoint<RoundManager>().AsSelf();
            builder.Register<BallSpawner>(Lifetime.Singleton);
            builder.RegisterEntryPoint<ScoreManager>().AsSelf();
        }
    }
}