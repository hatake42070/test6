using VContainer;
using VContainer.Unity;

public class OpeningLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<OpeningManager>();
        builder.RegisterComponentInHierarchy<OpeningSkip>();
    }
}
