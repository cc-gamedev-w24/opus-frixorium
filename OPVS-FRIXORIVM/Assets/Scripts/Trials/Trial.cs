using System.Collections.Generic;
using UnityEngine;
public abstract class Trial : ScriptableObject
{
    public string TrialName;

    public string Description;

    public int PointsAwarded = 100;
    
    public virtual void OnStartTrial()
    {
        IsCompleted = false;
        Winners.Clear();
        GameObject.FindWithTag("Audio Manager").GetComponent<AudioManager>().PlaySound("trial_start");
    }

    public abstract void OnEndTrial();

    public abstract void OnUpdate();

    public abstract void OnTimeUp();

    public bool IsCompleted { get; set; }

    public List<PlayerData> Winners { get; } = new();
}
