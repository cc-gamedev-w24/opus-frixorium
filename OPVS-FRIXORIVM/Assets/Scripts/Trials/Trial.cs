using System.Collections.Generic;
using UnityEngine;
public abstract class Trial : ScriptableObject
{
    public string Name;

    public string Description;
    
    protected abstract IPredicate _winCondition { get; }

    public abstract void OnStartTrial();
    
    public abstract void OnEndTrial();

    public bool IsCompleted => _winCondition.Evaluate();

    public List<PlayerData> Winners { get; } = new();
}
