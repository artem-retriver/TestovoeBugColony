using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldState
{
    private readonly List<BugController> _aliveBugs = new List<BugController>();
    private readonly List<ResourceView> _activeResources = new List<ResourceView>();

    public IReadOnlyList<BugController> AliveBugs => _aliveBugs;
    public IReadOnlyList<ResourceView> ActiveResources => _activeResources;

    public void RegisterBug(BugController bug) => _aliveBugs.Add(bug);
    public void UnregisterBug(BugController bug) => _aliveBugs.Remove(bug);
    
    public void RegisterResource(ResourceView resource) => _activeResources.Add(resource);
    public void UnregisterResource(ResourceView resource) => _activeResources.Remove(resource);

    private BugController GetClosestBug(Vector3 position, BugController exclude = null, bool ignoreInvincible = true)
    {
        IEnumerable<BugController> query = _aliveBugs.Where(b => b != exclude);

        if (ignoreInvincible)
        {
            query = query.Where(b => !b.IsInvincible);
        }
        
        return query.OrderBy(b => Vector3.Distance(position, b.transform.position)).FirstOrDefault();
    }

    private ResourceView GetClosestResource(Vector3 position)
    {
        return _activeResources.OrderBy(r => Vector3.Distance(position, r.transform.position)).FirstOrDefault();
    }

    public Transform GetClosestEatableTarget(BugController bug, bool includeBugs, bool includeResources, bool ignoreInvincible = true)
    {
        Transform closest = null;
        float minDist = float.MaxValue;
    
        if (includeBugs)
        {
            BugController nearestBug = GetClosestBug(bug.transform.position, bug, ignoreInvincible);
            
            if (nearestBug != null)
            {
                float d = Vector3.Distance(bug.transform.position, nearestBug.transform.position);

                if (d < minDist)
                {
                    minDist = d; closest = nearestBug.transform;
                }
            }
        }
    
        if (includeResources)
        {
            ResourceView nearestRes = GetClosestResource(bug.transform.position);
            
            if (nearestRes != null)
            {
                float d = Vector3.Distance(bug.transform.position, nearestRes.transform.position);

                if (d < minDist)
                {
                    closest = nearestRes.transform;
                }
            }
        }
        return closest;
    }
    
    public bool HasWorker()
    {
        return _aliveBugs.Any(b => b.Type == BugType.Worker);
    }
    
    public ResourceView GetRandomNearbyResource(Vector3 position, int topCount = 3)
    {
        List<ResourceView> nearest = _activeResources
            .OrderBy(r => Vector3.Distance(position, r.transform.position))
            .Take(topCount)
            .ToList();

        if (nearest.Count == 0)
        {
            return null;
        }
        
        return nearest[Random.Range(0, nearest.Count)];
    }
}