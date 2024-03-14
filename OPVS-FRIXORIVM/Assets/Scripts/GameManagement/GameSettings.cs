using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("General Settings")] 
    [SerializeField]
    private int _numberOfRounds = 3;
    public int NumberOfRounds
    {
        get => _numberOfRounds;
        set => _numberOfRounds = value;
    }
    
    [SerializeField]
    private float _roundTimeLimit = 10.0f;
    public float RoundTimeLimit
    {
        get => _roundTimeLimit;
        set => _roundTimeLimit = value;
    }
    
    [SerializeField]
    private bool _audienceEnabled;
    public bool AudienceEnabled
    {
        get => _audienceEnabled;
        set => _audienceEnabled = value;
    }

    [SerializeField]
    private bool _wakingUpEnabled = true;
    public bool WakingUpEnabled
    {
        get => _wakingUpEnabled;
        set => _wakingUpEnabled = value;
    }

    private void OnEnable()
    {
        WakingUpEnabled = true;
    }

    public void UpdateGameSettings(int roundCount, int roundTimer, bool isAudienceEnabled)
    {
        _numberOfRounds = roundCount;
        _audienceEnabled = isAudienceEnabled;
        _roundTimeLimit = roundTimer;
    }
}
