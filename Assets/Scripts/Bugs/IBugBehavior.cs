public interface IBugBehavior
{
    void Initialize(BugController controller);
    void OnUpdate();
    void OnCollisionWithEatable(IEatable eatable);
    void OnSplit();
    void OnDeath();
}