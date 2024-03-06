using System.Collections.Generic;
using UnityEngine;
public abstract class Trial : ScriptableObject
{
    [SerializeField]
    protected string Name;

    [SerializeField]
    protected string Description;
    
    protected abstract IPredicate _winCondition { get; }

    public abstract void OnStartTrial();
    
    public abstract void OnEndTrial();

    public bool IsCompleted => _winCondition.Evaluate();

    public List<PlayerData> Winners { get; } = new();
}
