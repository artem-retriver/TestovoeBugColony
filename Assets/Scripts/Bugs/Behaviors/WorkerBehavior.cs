public class WorkerBehavior : IBugBehavior
{
    private BugController _controller;
    private int _eatenCount;
    private ResourceView _currentTarget;

    public void Initialize(BugController controller)
    {
        _controller = controller;
        _eatenCount = 0;
        _currentTarget = null;
    }

    public void OnUpdate()
    {
        if (_currentTarget != null)
        {
            _controller.MoveTowards(_currentTarget.transform.position);
            return;
        }
        
        ResourceView newTarget = _controller.WorldState.GetRandomNearbyResource(_controller.transform.position, 3);
        
        if (newTarget != null)
        {
            _currentTarget = newTarget;
            _controller.MoveTowards(_currentTarget.transform.position);
        }
    }

    public void OnCollisionWithEatable(IEatable eatable)
    {
        if (eatable is ResourceView resource)
        {
            if (_currentTarget == resource)
            {
                _currentTarget = null;
            }

            resource.Eat();
            _eatenCount++;
            
            if (_eatenCount >= _controller.Config.resourcesToSplitWorker)
            {
                _controller.Split();
            }
        }
    }

    public void OnSplit() { }

    public void OnDeath() { }
}