using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class PredatorBehavior : IBugBehavior
{
    private BugController _controller;
    private CancellationTokenSource _lifeCts;
    private int _mealsEaten;

    public void Initialize(BugController controller)
    {
        _controller = controller;
        _mealsEaten = 0;

        _lifeCts = new CancellationTokenSource();
        StartLifeTimer().Forget();
    }

    private async UniTaskVoid StartLifeTimer()
    {
        await UniTask.Delay((int)(_controller.Config.predatorLifetime * 1000), cancellationToken: _lifeCts.Token);
        
        if (!_lifeCts.IsCancellationRequested)
        {
            _controller.Die();
        }
    }

    public void OnUpdate()
    {
        Transform target = _controller.WorldState.GetClosestEatableTarget(_controller, true, true, true);
        
        if (target != null)
        {
            _controller.MoveTowards(target.position);

            float dist = Vector3.Distance(_controller.transform.position, target.position);
            
            if (dist < 0.5f)
            {
                IEatable eatable = target.GetComponent<IEatable>();
                if (eatable != null)
                {
                    OnCollisionWithEatable(eatable);
                }
            }
        }
    }

    public void OnCollisionWithEatable(IEatable eatable)
    {
        if (eatable is ResourceView resource)
        {
            resource.Eat();
            _mealsEaten++;

            if (_mealsEaten >= _controller.Config.mealsToSplitPredator)
            {
                _controller.Split();
            }
        }
        else if (eatable is BugController bug && bug != _controller)
        {
            if (bug.IsInvincible)
            {
                return;
            }
            
            bug.Eat();
            _mealsEaten++;

            if (_mealsEaten >= _controller.Config.mealsToSplitPredator)
            {
                _controller.Split();
            }
        }
    }

    public void OnSplit() => _lifeCts?.Cancel();
    public void OnDeath() => _lifeCts?.Cancel();
}