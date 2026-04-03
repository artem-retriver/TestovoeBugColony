using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private GameConfig config;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Transform bugContainer;
    [SerializeField] private Transform resourceContainer;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(config);
        
        builder.RegisterComponent(uiManager);
        
        builder.Register<WorldState>(Lifetime.Singleton);
        builder.Register<EventBus>(Lifetime.Singleton);
        builder.Register<BugFactory>(Lifetime.Singleton);
        builder.Register<ResourceSpawner>(Lifetime.Singleton);
        
        builder.RegisterEntryPoint<GameInitializer>();
        
        builder.RegisterInstance(new BugContainerTransform(bugContainer));
        builder.RegisterInstance(new ResourceContainerTransform(resourceContainer));
    }
}