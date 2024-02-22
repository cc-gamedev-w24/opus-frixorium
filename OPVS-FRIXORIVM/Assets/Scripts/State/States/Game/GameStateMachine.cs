using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameStateMachine.GameState;

/// <summary>
///     State machine implementation for game states
/// </summary>
public sealed class GameStateMachine: StateMachine<GameStateMachine.GameState>
{
    public enum GameState
    {
        MainMenu,
        Lobby,
        Game
    }
    
    /// <summary>
    ///     The state to enter at start
    /// </summary>
    [SerializeField]
    private GameState _startingState;

    /// <summary>
    ///     Event to invoke on state change
    /// </summary>
    [SerializeField]
    private GameEvent _stateChangedEvent;

    [Header("State Data")]
    [SerializeField]
    private PlayerInputManager _playerInputManager;
    
    protected override void Start()
    {
        States = new Dictionary<GameState, IState<GameState>>
        {
            [MainMenu] = new MainMenuGameState(),
            [Lobby] = new LobbyGameState(_playerInputManager),
            [Game] = new GameGameState()
        };
        
        CurrentState = States[_startingState];
        base.Start();
    }

    protected override void TransitionState(GameState nextStateKey)
    {
        base.TransitionState(nextStateKey);
        _stateChangedEvent.Invoke(GameEvent.GlobalChannel, nextStateKey);
    }
}
