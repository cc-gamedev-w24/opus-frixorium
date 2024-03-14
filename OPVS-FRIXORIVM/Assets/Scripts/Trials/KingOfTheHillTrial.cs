
using Events;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Trials/King-Of-The-Hill", fileName = "King-Of-The-Hill Trial")]
public class KingOfTheHillTrial : Trial
{
    private PlayerManager _playerManager;

    [SerializeField]
    private GameSettings _gameSettings;

    [SerializeField]
    private GameEvent _timerChangeEvent;

    [SerializeField]
    private GameEvent _trialScoreChangeEvent;

    [SerializeField]
    private GameEvent _trialAddScoreEvent;

    [SerializeField]
    private GameObject _hillObject;

    private GameObject _hill;


    protected override IPredicate _winCondition => new FuncPredicate(EndAfterTimeUp);

    private int[] _scores;

    public int[] Scores
    {
        get => _scores;
    }

    private float _elapsedTime;

    private DelegateGameEventListener _listener;

    void Awake()
    {
        _listener = new DelegateGameEventListener(_trialAddScoreEvent, UpdateScoreOnEvent);
    }

    private void UpdateScoreOnEvent(object data)
    {
        if (data is not int player)
        {
            Debug.LogError("Wrong data type passed to ui event handler!");
            return;
        }

        _scores[player] += 5;
    }

    public override void OnStartTrial()
    {
        base.OnStartTrial();
        _playerManager = FindObjectOfType<PlayerManager>();
        _scores = new int[_playerManager.MaxPlayers]; // TODO: maybe move round score to game manager? idk
        _elapsedTime = 0;
        _hill = Instantiate<GameObject>(_hillObject);
        _hill.transform.position = new Vector3(7.63f, -3.82f, 0.94f);
    }

    public override void OnEndTrial()
    {
        // TODO Add way to track round & game points
    }

    private bool EndAfterTimeUp()
    {
        _elapsedTime += Time.deltaTime;
        int timeRemaining = (int)Math.Truncate(_gameSettings.RoundTimeLimit - _elapsedTime);
        _timerChangeEvent.Invoke(GameEvent.GlobalChannel, timeRemaining);
        _trialScoreChangeEvent.Invoke(GameEvent.GlobalChannel, _scores);
        if (_elapsedTime < _gameSettings.RoundTimeLimit) return false;
        Array.Fill(_scores, 0);
        Destroy(_hill);
        return true;
    }
}
