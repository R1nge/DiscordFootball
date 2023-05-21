using Manager.GamePlay;
using VContainer;
using VContainer.Unity;

namespace Scope
{
    public class TeamSelectionScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TeamManager>(Lifetime.Singleton);
        }
    }
}