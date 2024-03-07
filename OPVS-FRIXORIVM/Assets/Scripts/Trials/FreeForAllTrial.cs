using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Trials/Free-for-All", fileName = "Free-for-All Trial")]
public class FreeForAllTrial : Trial
{
    private PlayerManager _playerManager;
    
    [SerializeField]
    private GameSettings _gameSettings;

    protected override IPredicate _winCondition => new FuncPredicate(EndAfterTimeUp);

    private int[] _scores;

    private float _elapsedTime;
    
    public override void OnStartTrial()
    {
        base.OnStartTrial();
        _playerManager = FindObjectOfType<PlayerManager>();
        _scores = new int[_playerManager.MaxPlayers]; // TODO: maybe move round score to game manager? idk
        _elapsedTime = 0;
    }

    public override void OnEndTrial()
    {
        // TODO Add way to track round & game points
    }

    private bool EndAfterTimeUp()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime < _gameSettings.RoundTimeLimit) return false;
        return true;
    }
}
