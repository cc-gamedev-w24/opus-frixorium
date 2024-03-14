
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

    public override void OnUpdate()
    {
        _trialScoreChangeEvent.Invoke(GameEvent.GlobalChannel, _scores);
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

    public override void OnTimeUp()
    {
        Array.Fill(_scores, 0);
        Destroy(_hill);
        IsCompleted = true;
    }
}
