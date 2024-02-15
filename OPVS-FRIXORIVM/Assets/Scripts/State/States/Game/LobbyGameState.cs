using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static GameStateMachine.GameState;
public class LobbyGameState: IState<GameStateMachine.GameState> {
    private readonly GameEvent _triggerEvent;
    private readonly PlayerInputManager _playerManager;
    

    public GameStateMachine.GameState StateKey => Lobby;
    
    private AsyncOperation _pendingLoad;
    private const string ScenePath = "Samples/Game States/Scenes/LobbyScene";
    
    private TriggerGameEventListener _triggerGameEventListener;

    public LobbyGameState(GameEvent triggerEvent, PlayerInputManager playerManager)
    {
        _triggerEvent = triggerEvent;
        _playerManager = playerManager;
    }

    public void EnterState()
    {
        _playerManager.enabled = true;
        _triggerGameEventListener = new TriggerGameEventListener(_triggerEvent);
        _pendingLoad = SceneManager.LoadSceneAsync(ScenePath, LoadSceneMode.Additive);
    }

    public void ExitState()
    {
        if (_pendingLoad.isDone)
        {
            HandleExit();
        }
        else
        { 
            _pendingLoad.completed += _ => HandleExit();
        }
    }

    private void HandleExit()
    {
        _triggerGameEventListener.Dispose();
        SceneManager.UnloadSceneAsync(ScenePath);
    }

    public void UpdateState()
    {
    }

    public GameStateMachine.GameState GetNextState()
    {
        return _triggerGameEventListener.Triggered ? Game : StateKey;
    }
}
