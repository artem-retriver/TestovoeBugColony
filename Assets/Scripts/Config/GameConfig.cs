using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Bug Colony/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Movement")]
    public float workerSpeed = 3f;
    public float predatorSpeed = 5f;  

    [Header("Worker")]
    public int resourcesToSplitWorker = 2;
    public float workerSplitMutationChance = 0.1f;
    public int minAliveBugsForMutation = 10;

    [Header("Predator")]
    public float predatorLifetime = 10f;
    public int mealsToSplitPredator = 3;

    [Header("Resources")]
    public int maxResources = 20;
    public float resourceSpawnInterval = 2f;
    public Vector2 spawnArea = new (20f, 20f);

    [Header("Colony")]
    public int initialWorkerCount = 1;
    
    [Header("Prefabs")]
    public GameObject resourcePrefab;
    public GameObject workerBugPrefab;
    public GameObject predatorBugPrefab;
}