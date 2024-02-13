using UnityEngine;

/// <summary>
///     Player controller while in lobby
/// </summary>
public class LobbyPlayer : MonoBehaviour
{
    private PlayerData _playerData;

    private void Awake()
    {
        _playerData = GetComponentInParent<Player>().PlayerData;
    }

    private void OnReady()
    {
        _playerData.IsReady = true;
    }

    private void OnUnready()
    {
        _playerData.IsReady = false;
    }

    private void OnDropOut()
    {
        if (!_playerData.IsReady)
            Destroy(transform.parent.gameObject);
    }
}
