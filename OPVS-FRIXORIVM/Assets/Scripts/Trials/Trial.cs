using System.Collections.Generic;
using UnityEngine;
public abstract class Trial : ScriptableObject
{
    public string Name;

    public string Description;
    
    protected abstract IPredicate _winCondition { get; }

    public virtual void OnStartTrial()
    {
        Winners.Clear();
    }

    public abstract void OnEndTrial();

    public bool IsCompleted => _winCondition.Evaluate();

    public List<PlayerData> Winners { get; } = new();
}
