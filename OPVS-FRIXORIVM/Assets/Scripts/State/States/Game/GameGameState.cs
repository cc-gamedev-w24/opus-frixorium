using UnityEngine;
using UnityEngine.SceneManagement;
using static GameStateMachine.GameState;
public class GameGameState: IState<GameStateMachine.GameState>
{
    public GameStateMachine.GameState StateKey => Game;
    
    private AsyncOperation _pendingLoad;
    private const string ScenePath = "Samples/Game States/Scenes/GameScene";

    public void EnterState()
    {
        _pendingLoad = SceneManager.LoadSceneAsync(ScenePath, LoadSceneMode.Additive);
    }

    public void ExitState()
    {
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
