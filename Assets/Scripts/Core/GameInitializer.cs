using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using UniRx;

public class GameInitializer : IStartable
{
    [Inject] private BugFactory _bugFactory;
    [Inject] private ResourceSpawner _resourceSpawner;
    [Inject] private EventBus _eventBus;
    [Inject] private WorldState _worldState;
    [Inject] private GameConfig _config;

    public void Start()
    {
        for (int i = 0; i < _config.initialWorkerCount; i++)
        {
            Vector3 randomPos = GetRandomPosition();
            _bugFactory.CreateBug(randomPos, BugType.Worker);
        }
        
        _resourceSpawner.StartSpawningAsync().Forget();
        _eventBus.OnBugDied.Subscribe(_ => CheckAndRespawn());
    }
    
    private void CheckAndRespawn()
    {
        if (!_worldState.HasWorker())
        {
            Vector3 randomPos = GetRandomPosition();
            _bugFactory.CreateBug(randomPos, BugType.Worker);
        }
    }
    
    private Vector3 GetRandomPosition()
    {
        Vector3 area = _config.spawnArea;
        return new Vector3(Random.Range(-area.x, area.x), 0, Random.Range(-area.y, area.y));
    }
}