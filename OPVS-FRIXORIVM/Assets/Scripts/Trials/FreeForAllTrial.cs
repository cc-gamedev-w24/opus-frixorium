using UnityEngine;

[CreateAssetMenu(menuName = "Trials/Free-for-All", fileName = "Free-for-All Trial")]
public class FreeForAllTrial : Trial
{
    private PlayerManager _playerManager;
    
    [SerializeField]
    private GameSettings _gameSettings;

    private int[] _scores;

    
    public override void OnStartTrial()
    {
        base.OnStartTrial();
        _playerManager = FindObjectOfType<PlayerManager>();
        _scores = new int[_playerManager.MaxPlayers]; // TODO: maybe move round score to game manager? idk
    }

    public override void OnEndTrial()
    {
        // TODO Add way to track round & game points
    }

    public override void OnUpdate()
    {
    }

    public override void OnTimeUp()
    {
        IsCompleted = true;
    }
}
