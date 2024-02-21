using UnityEngine;
using static GameStateMachine.GameState;
public class GameGameState: IState<GameStateMachine.GameState>
{

    public GameStateMachine.GameState StateKey => Game;

    public void EnterState()
    {
        Debug.Log("Entered game state");
    }

    public void ExitState()
    {
        Debug.Log("Exited game state");
    }

    public void UpdateState()
    {
    }

    public GameStateMachine.GameState GetNextState()
    {
        return StateKey;
    }
}
