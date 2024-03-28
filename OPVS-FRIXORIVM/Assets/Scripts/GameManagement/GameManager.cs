using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private AudienceServerManager _audienceServerManager;
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField]
    private GameEvent _gameStartedEvent;
    [SerializeField]
    private GameEvent _trialStartedEvent;

    public Trial CurrentTrial { get; private set; }
    public float ElapsedTime { get; private set; }

    public int RemainingRounds { get; private set; }
    
    void Start()
    {
        _gameStartedEvent.Invoke();
        RemainingRounds = _gameSettings.NumberOfRounds;
        CurrentTrial = GetRandomTrial();
        CurrentTrial.OnStartTrial();
        _trialStartedEvent.Invoke(GameEvent.GlobalChannel, CurrentTrial);
    }

    void Update()
    {
        if (!CurrentTrial.IsCompleted)
        {
            CurrentTrial.OnUpdate();
            if (ElapsedTime < _gameSettings.RoundTimeLimit) {
                ElapsedTime += Time.deltaTime;
                if (ElapsedTime >= _gameSettings.RoundTimeLimit)
                {
                    CurrentTrial.OnTimeUp();
                }
            }
            if (CurrentTrial.IsCompleted)
            {
                CurrentTrial.OnEndTrial();
                foreach (var winner in CurrentTrial.Winners)
                {
                    winner.PlayerScore += CurrentTrial.PointsAwarded;
                }
                RemainingRounds--;
                if (RemainingRounds <= 0)
                {
                    //TODO: Game Over
                    return;
                }
                StartCoroutine(NextTrial());
            }
        }
    }
    
    public IEnumerator NextTrial()
    {
        if (_audienceServerManager.Enabled && _audienceServerManager.AudienceCount > 0)
        {
            var options = Enumerable.Range(0, 3).Select(_ => GetRandomTrial());
            _audienceServerManager.SendTrialDataToServer(options);
            var received = false;
            _audienceServerManager.OnTrialSelectedEvent += selected => {
                CurrentTrial = _gameSettings.AvailableTrials.First(trial => trial.TrialName == selected);
                received = true;
            };
            while(!received) yield return 0;
        }
        else
        {
            CurrentTrial = GetRandomTrial();
        }
        
        CurrentTrial.OnStartTrial();
        ElapsedTime = 0;
        _trialStartedEvent.Invoke(GameEvent.GlobalChannel, CurrentTrial);
    }
    
    private Trial GetRandomTrial()
    {
        return _gameSettings.AvailableTrials.Where(trial => trial != CurrentTrial).ToArray()[Random.Range(0, _gameSettings.AvailableTrials.Length-1)];
    }
}
