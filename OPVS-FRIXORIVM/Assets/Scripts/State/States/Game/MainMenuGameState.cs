using UnityEngine;
using UnityEngine.SceneManagement;
using static GameStateMachine.GameState;

public class MainMenuGameState: IState<GameStateMachine.GameState>
{
    private readonly GameEvent _triggerEvent;
    public GameStateMachine.GameState StateKey => MainMenu;

    private const string ScenePath = "Samples/Game States/Scenes/StartScene";
    private AsyncOperation _pendingLoad;

    private TriggerGameEventListener _triggerGameEventListener;
    
    public MainMenuGameState(GameEvent triggerEvent)
    {
        _triggerEvent = triggerEvent;
    }
    
    public void EnterState()
    {
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
        return _triggerGameEventListener.Triggered ? Lobby : StateKey;
    }
}
