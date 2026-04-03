using UnityEngine;
using VContainer;

public class BugFactory
{
    [Inject] private IObjectResolver _resolver;
    [Inject] private GameConfig _config;
    [Inject] private BugContainerTransform _bugContainer;
    
    public void CreateBug(Vector3 position, BugType type)
    {
        GameObject prefab = type == BugType.Worker ? _config.workerBugPrefab : _config.predatorBugPrefab;
            
        if (prefab == null)
        {
            Debug.LogError($"Prefab for {type} is not assigned in GameConfig!");
            return;
        }
        
        GameObject obj = Object.Instantiate(prefab, position, Quaternion.identity, _bugContainer.Value);
        
        BugController controller = obj.GetComponent<BugController>();
        _resolver.Inject(controller);
        
        IBugBehavior behavior = type == BugType.Worker ? new WorkerBehavior() : new PredatorBehavior();
        controller.Initialize(type, behavior);
    }
}