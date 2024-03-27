using System.Collections;
using System.Linq;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private AudienceServerManager _audienceServerManager;
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField]
    private GameEvent _gameStartedEvent;
    [SerializeField]
    private GameEvent _trialStartedEvent;
    [SerializeField]
    private GameEvent _timerChangeEvent;

    public Trial CurrentTrial { get; private set; }
    private float _elapsedTime;
    
    void Start()
    {
        _gameStartedEvent.Invoke();
        CurrentTrial = GetRandomTrial();
        CurrentTrial.OnStartTrial();
        _trialStartedEvent.Invoke(GameEvent.GlobalChannel, CurrentTrial);
    }

    void Update()
    {
        if (!CurrentTrial.IsCompleted)
        {
            CurrentTrial.OnUpdate();
            if (_elapsedTime < _gameSettings.RoundTimeLimit) {
                _elapsedTime += Time.deltaTime;
                int timeRemaining = (int)Math.Truncate(_gameSettings.RoundTimeLimit - _elapsedTime);
                _timerChangeEvent.Invoke(GameEvent.GlobalChannel, timeRemaining);
                if (_elapsedTime >= _gameSettings.RoundTimeLimit)
                {
                    CurrentTrial.OnTimeUp();
                }
            }
            if (CurrentTrial.IsCompleted)
            {
                CurrentTrial.OnEndTrial();
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
        _elapsedTime = 0;
        _trialStartedEvent.Invoke(GameEvent.GlobalChannel, CurrentTrial);
    }
    
    private Trial GetRandomTrial()
    {
        return _gameSettings.AvailableTrials.Where(trial => trial != CurrentTrial).ToArray()[UnityEngine.Random.Range(0, _gameSettings.AvailableTrials.Length-1)];
        //return _gameSettings.AvailableTrials.Where(trial => trial != CurrentTrial).ToArray()[3];
    }
}
