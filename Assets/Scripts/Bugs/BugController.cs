using UnityEngine;
using VContainer;

public class BugController : MonoBehaviour, IEatable 
{
    [Inject] private WorldState _worldState;
    [Inject] private EventBus _eventBus;
    [Inject] private BugFactory _bugFactory;
    [Inject] private GameConfig _config;
    
    private IBugBehavior _behavior;
    private BugView _view;
    
    public BugType Type { get; private set; }
    
    public WorldState WorldState => _worldState;
    public GameConfig Config => _config;

    public bool IsInvincible => Time.time < _invincibleUntil;
    private float Speed => Type == BugType.Worker ? _config.workerSpeed : _config.predatorSpeed;
    private float _invincibleUntil;
    
    public void Initialize(BugType type, IBugBehavior behavior)
    {
        Type = type;
        _behavior = behavior;
        _view = GetComponent<BugView>();
        _view.Initialize();
        
        _behavior.Initialize(this);
        _worldState.RegisterBug(this);
        
        _invincibleUntil = Time.time + 2f;
    }
    
    private void Update()
    {
        _behavior?.OnUpdate();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        IEatable eatable = other.GetComponent<IEatable>();
        
        if (eatable != null)
        {
            _behavior.OnCollisionWithEatable(eatable);
        }
    }
    
    public void MoveTowards(Vector3 target)
    {
        _view.MoveTowards(target, Speed);
    }

    public void Eat()
    {
        Die();
    }
    
    public void Die()
    {
        _behavior.OnDeath();
        _worldState.UnregisterBug(this);
        _eventBus.PublishBugDied(this);
        Destroy(gameObject);
    }
    
    public void Split()
    {
        _behavior.OnSplit();
        Vector3 pos = transform.position;
        BugType newType1 = Type;
        BugType newType2 = Type;
    
        if (Type == BugType.Worker && _worldState.AliveBugs.Count > _config.minAliveBugsForMutation)
        {
            if (Random.value < _config.workerSplitMutationChance)
            {
                newType2 = BugType.Predator;
            }
        }
    
        _bugFactory.CreateBug(pos + Random.insideUnitSphere * 1.5f, newType1);
        _bugFactory.CreateBug(pos + Random.insideUnitSphere * 1.5f, newType2);
    
        DestroyForSplit();
    }
    
    private void DestroyForSplit()
    {
        _worldState.UnregisterBug(this);
        Destroy(gameObject);
    }
}