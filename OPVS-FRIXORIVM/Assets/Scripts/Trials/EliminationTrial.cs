using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Trials/Elimination", fileName = "Elimination Trial")]
public class EliminationTrial : Trial
{
    private PlayerManager _playerManager;

    [SerializeField]
    private AudioClip _bgm;

    [SerializeField]
    private GameSettings _gameSettings;

    private bool _roundOver;

    private float _countdownUntilMusic;

    public override void OnStartTrial()
    {
        base.OnStartTrial();
        _playerManager = FindObjectOfType<PlayerManager>();
        _gameSettings.WakingUpEnabled = false;
        _roundOver = false;
        _countdownUntilMusic = 1.8f;
    }

    public override void OnEndTrial()
    {
        Winners.AddRange(_playerManager.GetAllPlayerData().Where(player => !player.IsKnockedOut));
        _gameSettings.WakingUpEnabled = true;
        _roundOver = true;
        GameObject.FindWithTag("Audio Manager").GetComponent<AudioManager>().StopBackgroundMusic();
    }

    public override void OnUpdate()
    {
        var awakePlayers = 0;
        foreach (var playerData in _playerManager.GetAllPlayerData())
        {
            if (playerData.IsKnockedOut)
                continue;
            awakePlayers++;
            if (awakePlayers > 1)
            {
                return;
            }
        }
        IsCompleted = true;

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
        Debug.Log("TODO: Sudden Death");
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
