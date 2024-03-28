using System.Linq;
using System;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Trials/Free-for-All", fileName = "Free-for-All Trial")]
public class FreeForAllTrial : Trial
{
    private PlayerManager _playerManager;

    [SerializeField]
    private AudioClip _bgm;

    [SerializeField]
    private GameSettings _gameSettings;

    private bool _roundOver;

    private int[] _scores;

    private float _countdownUntilMusic;


    public override void OnStartTrial()
    {
        base.OnStartTrial();
        _playerManager = FindObjectOfType<PlayerManager>();
        _scores = new int[_playerManager.MaxPlayers]; // TODO: maybe move round score to game manager? idk
        _roundOver = false;
        _countdownUntilMusic = 1.8f;
    }

    public override void OnEndTrial()
    {
        // TODO Add way to track round & game points
        _roundOver = true;
        GameObject.FindWithTag("Audio Manager").GetComponent<AudioManager>().StopBackgroundMusic();
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

    public override void OnTimeUp()
    {
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
