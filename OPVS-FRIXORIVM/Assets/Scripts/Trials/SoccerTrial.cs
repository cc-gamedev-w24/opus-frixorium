
using Events;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(menuName = "Trials/Soccer", fileName = "Soccer Trial")]
public class SoccerTrial : Trial
{
    private PlayerManager _playerManager;

    [SerializeField]
    private AudioClip _bgm;

    [SerializeField]
    private GameSettings _gameSettings;

    [SerializeField]
    private GameEvent _timerChangeEvent;

    [SerializeField]
    private GameEvent _soccerTrialAddScoreEvent;

    [SerializeField]
    private GameObject _netObject;

    [SerializeField]
    private GameObject _ballObject;

    private GameObject _netOne;
    private GameObject _netTwo;
    private GameObject _ball;

    private int[] _scores;

    public int[] Scores
    {
        get => _scores;
    }

    private float _elapsedTime;

    private DelegateGameEventListener _listener;

    private List<Player> _teamOne;
    private List<Player> _teamTwo;
    private int _teamOneScore;
    private int _teamTwoScore;

    [SerializeField]
    private GameObject _teamScorePanelObject;
    private GameObject _teamScorePanel;

    private bool _roundOver;

    private float _countdownUntilMusic;

    void Awake()
    {
        _listener = new DelegateGameEventListener(_soccerTrialAddScoreEvent, UpdateScoreOnEvent);
    }

    public override void OnUpdate()
    {
        if (_countdownUntilMusic > 0.0f)
        {
            _countdownUntilMusic -= Time.deltaTime;
        }
        else
        {
            PlayMusic();
        }
    }

    private void UpdateScoreOnEvent(object data)
    {
        if (data is not int teamNet)
        {
            Debug.LogError("Wrong data type passed to ui event handler!");
            return;
        }

        if (teamNet == 1)
        {
            _teamTwoScore += 10;
            _teamScorePanel.GetComponent<TeamScorePanelController>().SetTeamScore(teamNet, _teamTwoScore.ToString());
            foreach (Player player in _teamTwo)
            {
                if (player != null)
                {
                    player.PlayerData.PlayerScore += 10;
                }
            }
        }
        else if (teamNet == 2)
        {
            _teamOneScore += 10;
            _teamScorePanel.GetComponent<TeamScorePanelController>().SetTeamScore(teamNet, _teamOneScore.ToString());
            foreach (Player player in _teamOne)
            {
                if (player != null)
                {
                    player.PlayerData.PlayerScore += 10;
                }
            }
        }
    }

    public override void OnStartTrial()
    {
        base.OnStartTrial();
        _playerManager = FindObjectOfType<PlayerManager>();
        _scores = new int[_playerManager.MaxPlayers]; // TODO: maybe move round score to game manager? idk
        _elapsedTime = 0;
        _teamOne = new List<Player>();
        _teamTwo = new List<Player>();
        _teamOneScore = 0;
        _teamTwoScore = 0;
        _teamScorePanel = Instantiate(_teamScorePanelObject, GameObject.FindWithTag("GameUI").transform);
        _countdownUntilMusic = 1.8f;

        Player[] players = _playerManager.GetComponent<PlayerManager>().Players;

        int teamToAddTo = 1;

        foreach (Player player in players)
        {
            if (teamToAddTo == 1)
            {
                _teamOne.Add(player);
                teamToAddTo = 2;
            }
            else if (teamToAddTo == 2)
            {
                _teamTwo.Add(player);
                teamToAddTo = 1;
            }
        }

        
        _netOne = Instantiate(_netObject, GameObject.FindWithTag("Net One Transform").transform);
        _netTwo = Instantiate(_netObject, GameObject.FindWithTag("Net Two Transform").transform);
        _netOne.GetComponentInChildren<NetController>().TeamNumber = 1;
        _netTwo.GetComponentInChildren<NetController>().TeamNumber = 2;
        _netOne.GetComponentInChildren<NetController>().NetColor = new Color(1f, 0f, 0f, 0.5f);
        _netTwo.GetComponentInChildren<NetController>().NetColor = new Color(0f, 0f, 1f, 0.5f);
        _ball = Instantiate(_ballObject, GameObject.FindWithTag("Ball Transform").transform);
        _roundOver = false;
    }

    public override void OnEndTrial()
    {
        // TODO Add way to track round & game points
        Destroy(_netOne);
        Destroy(_netTwo);
        Destroy(_ball);
        Destroy(_teamScorePanel);
        _roundOver = true;
        GameObject.FindWithTag("Audio Manager").GetComponent<AudioManager>().StopBackgroundMusic();
    }

    public override void OnTimeUp()
    {
        Array.Fill(_scores, 0);
        IsCompleted = true;
    }

    public void PlayMusic()
    {
        if (!_roundOver)
        {
            if (!GameObject.FindWithTag("Audio Manager").GetComponent<AudioManager>().IsMusicPlagying())
            {
                GameObject.FindWithTag("Audio Manager").GetComponent<AudioManager>().PlayBackgroundMusic(_bgm);
            }
        }
    }
}
