using Manager.GamePlay;
using Services;
using VContainer;
using VContainer.Unity;

namespace Scope
{
    public class TeamSelectionScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TeamManager>(Lifetime.Singleton);
            builder.Register<TeamManagerUIService>(Lifetime.Singleton);
        }
    }
}