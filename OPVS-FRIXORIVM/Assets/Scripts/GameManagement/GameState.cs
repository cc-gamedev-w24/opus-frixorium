using UnityEngine;

[CreateAssetMenu(menuName = "Game/Game State")]
public class GameState : ScriptableObject
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField]
    private Trial[] _availableTrials;

    [SerializeField]
    private GameEvent _gameStartedEvent;
    [SerializeField]
    private GameEvent _trialStartedEvent;
    
    public Trial CurrentTrial { get; private set; }
    
    public void StartGame()
    {
        _gameStartedEvent.Invoke();
        CurrentTrial = GetRandomTrial();
        CurrentTrial.OnStartTrial();
        _trialStartedEvent.Invoke(GameEvent.GlobalChannel, CurrentTrial);
    }

    public void Update()
    {
        if (CurrentTrial.IsCompleted)
        {
            CurrentTrial.OnEndTrial();
            NextTrial();
            CurrentTrial.OnStartTrial();
            _trialStartedEvent.Invoke(GameEvent.GlobalChannel, CurrentTrial);
        }
    }

    public void NextTrial()
    {
        var nextTrial = GetRandomTrial();
        while (nextTrial == CurrentTrial)
        {
            nextTrial = GetRandomTrial();
        }
        CurrentTrial = nextTrial;
    }

    private Trial GetRandomTrial()
    {
        return _availableTrials[Random.Range(0, _availableTrials.Length)];
    }
}
