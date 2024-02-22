using UnityEngine;
using UnityEngine.SceneManagement;
using static GameStateMachine.GameState;

public class MainMenuGameState: IState<GameStateMachine.GameState>
{
    public GameStateMachine.GameState StateKey => MainMenu;

    private const string ScenePath = "Samples/Game States/Scenes/StartScene";
    private AsyncOperation _pendingLoad;
    
    public void EnterState()
    {
        Debug.Log("Entered Main Menu Game State");
        _pendingLoad = SceneManager.LoadSceneAsync(ScenePath, LoadSceneMode.Additive);
    }

    public void ExitState()
    {
        Debug.Log("Exited Main Menu Game State");
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
        return Lobby;
    }
}
