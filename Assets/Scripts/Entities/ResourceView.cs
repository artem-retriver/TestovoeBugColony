using UnityEngine;
using VContainer;

public class ResourceView : MonoBehaviour, IEatable
{
    [Inject] private WorldState _worldState;
    
    public void Eat()
    {
        _worldState.UnregisterResource(this);
        Destroy(gameObject);
    }
}