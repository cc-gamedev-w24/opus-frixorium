using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static GameStateMachine.GameState;
public class LobbyGameState: IState<GameStateMachine.GameState> {
    private readonly PlayerInputManager _playerManager;

    public GameStateMachine.GameState StateKey => Lobby;
    
    private AsyncOperation _pendingLoad;
    private const string ScenePath = "Samples/Game States/Scenes/LobbyScene";

    public LobbyGameState(PlayerInputManager playerManager)
    {
        _playerManager = playerManager;
    }

    public void EnterState()
    {
        Debug.Log("Entered lobby state");
        _playerManager.enabled = true;
        SceneManager.LoadScene(ScenePath, LoadSceneMode.Additive);
    }

    public void ExitState()
    {
        Debug.Log("Exited lobby state");
        if (_pendingLoad.isDone)
        {
            SceneManager.UnloadSceneAsync(ScenePath);
        }
        else
        { 
            _pendingLoad.completed += _ => SceneManager.UnloadSceneAsync(ScenePath);
        }
    }

    public void UpdateState()
    {
    }

    public GameStateMachine.GameState GetNextState()
    {
        return StateKey;
    }
}
