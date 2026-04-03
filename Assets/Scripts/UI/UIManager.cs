using UnityEngine;
using UnityEngine.UI;
using UniRx;
using VContainer;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text deadWorkersText;
    [SerializeField] private Text deadPredatorsText;
    
    private int _deadWorkers;
    private int _deadPredators;
    
    [Inject] private EventBus _eventBus;
    
    private void Start()
    {
        if (_eventBus == null)
        {
            Debug.LogError("UIManager: EventBus is not injected!");
            return;
        }
        
        if (deadWorkersText == null || deadPredatorsText == null)
        {
            Debug.LogError("UIManager: UI Text references are missing! Assign them in Inspector.");
            return;
        }
    
        _eventBus.OnBugDied.Subscribe(bug =>
        {
            if (bug.Type == BugType.Worker)
            {
                _deadWorkers++;
            }
            else
            {
                _deadPredators++;
            }
            
            UpdateUI();
        }).AddTo(this);
    }
    
    private void UpdateUI()
    {
        deadWorkersText.text = $"Dead Workers: {_deadWorkers}";
        deadPredatorsText.text = $"Dead Predators: {_deadPredators}";
    }
}