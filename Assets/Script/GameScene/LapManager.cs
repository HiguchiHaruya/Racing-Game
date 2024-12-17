using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class LapManager : Singleton<LapManager>
{
    ReactiveCollection<int> Triggers = new ReactiveCollection<int>();
    [SerializeField] private int totalTriggers;
    ReactiveProperty<int> _currentLap = new ReactiveProperty<int>(0);
    ReactiveProperty<bool> _isGoal = new ReactiveProperty<bool>(false);
    public IReadOnlyReactiveProperty<int> CurrentLap => _currentLap;
    private void Start()
    {
        Triggers.ObserveCountChanged()
            .Where(count => count >= totalTriggers)
            .Subscribe(_ => LapComplate())
            .AddTo(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        var trigger = other.GetComponent<TriggerID>();
        Triggers.Add(trigger.ID);
        Debug.Log($"^^{trigger.name}‚ð’Ê‰ß");

    }
    public void LapComplate()
    {
        _currentLap.Value++;
        ResetTriggers();
    }
    public void ResetTriggers()
    {
        Triggers.Clear();
    }
}
