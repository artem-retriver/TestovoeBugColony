using System;
using UniRx;

public class EventBus
{
    private readonly Subject<BugController> _bugDied = new Subject<BugController>();

    public IObservable<BugController> OnBugDied => _bugDied;

    public void PublishBugDied(BugController bug) => _bugDied.OnNext(bug);
}

public interface IEatable { }