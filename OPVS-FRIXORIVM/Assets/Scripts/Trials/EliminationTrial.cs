using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Trials/Elimination", fileName = "Elimination Trial")]
public class EliminationTrial : Trial
{
    private PlayerManager _playerManager;
    
    [SerializeField]
    private GameSettings _gameSettings;

    public override void OnStartTrial()
    {
        base.OnStartTrial();
        _playerManager = FindObjectOfType<PlayerManager>();

        _gameSettings.WakingUpEnabled = false;
    }

    public override void OnEndTrial()
    {
        Winners.AddRange(_playerManager.GetAllPlayerData().Where(player => !player.IsKnockedOut));
        _gameSettings.WakingUpEnabled = true;
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
    }

    public override void OnTimeUp()
    {
        Debug.Log("TODO: Sudden Death");
    }
}
