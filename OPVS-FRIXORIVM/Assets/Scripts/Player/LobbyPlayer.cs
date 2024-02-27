/// <summary>
///     Player controller while in lobby
/// </summary>
public class LobbyPlayer : PlayerController
{
    private void OnReady()
    {
        PlayerData.IsReady = true;
    }

    private void OnUnready()
    {
        PlayerData.IsReady = false;
    }

    private void OnDropOut()
    {
        if (!PlayerData.IsReady)
            Destroy(transform.parent.gameObject);
    }
    protected override string ActionMap => "Lobby";
}
