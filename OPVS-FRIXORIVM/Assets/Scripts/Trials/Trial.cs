using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public abstract class Trial : ScriptableObject
{
    [FormerlySerializedAs("Name")]
    public string TrialName;

    public string Description;
    
    public virtual void OnStartTrial()
    {
        IsCompleted = false;
        Winners.Clear();
    }

    public abstract void OnEndTrial();

    public abstract void OnUpdate();

    public abstract void OnTimeUp();

    public bool IsCompleted { get; set; }

    public List<PlayerData> Winners { get; } = new();
}
