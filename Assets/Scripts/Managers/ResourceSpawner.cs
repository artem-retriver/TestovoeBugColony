using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;
using System;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class ResourceSpawner
{
    [Inject] private GameConfig _config;
    [Inject] private WorldState _worldState;
    [Inject] private ResourceContainerTransform _resourceContainer;
    [Inject] private IObjectResolver _resolver;

    public async UniTask StartSpawningAsync()
    {
        while (true)
        {
            try
            {
                await UniTask.Delay((int)(_config.resourceSpawnInterval * 1000));
                if (_worldState.ActiveResources.Count < _config.maxResources)
                {
                    SpawnResource();
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"ResourceSpawner error: {exception.Message}");
                break;
            }
        }
    }
    
    private void SpawnResource()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(-_config.spawnArea.x, _config.spawnArea.x), 
            0, 
            Random.Range(-_config.spawnArea.y, _config.spawnArea.y)
        );

        GameObject obj = Object.Instantiate(_config.resourcePrefab, randomPos, Quaternion.identity, _resourceContainer.Value);
        ResourceView resource = obj.GetComponent<ResourceView>();
        
        _resolver.Inject(resource);
        
        _worldState.RegisterResource(resource);
    }
}