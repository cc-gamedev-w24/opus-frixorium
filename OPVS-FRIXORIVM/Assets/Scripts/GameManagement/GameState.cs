using UnityEngine;

[CreateAssetMenu(menuName = "Game/Game State")]
public class GameState : ScriptableObject
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField]
    private Trial[] _availableTrials;

    public Trial CurrentTrial { get; private set; }
    
    public void StartGame()
    {
        CurrentTrial = GetRandomTrial();
        CurrentTrial.OnStartTrial();
    }

    public void Update()
    {
        if (CurrentTrial.IsCompleted)
        {
            CurrentTrial.OnEndTrial();
            NextTrial();
            CurrentTrial.OnStartTrial();
        }
    }

    public void NextTrial()
    {
        CurrentTrial = GetRandomTrial();
    }

    private Trial GetRandomTrial()
    {
        return _availableTrials[Random.Range(0, _availableTrials.Length)];
    }
}
