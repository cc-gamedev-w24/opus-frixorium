using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Abstract state machine
/// </summary>
/// <typeparam name="T">Enum type representing available states</typeparam>
public abstract class StateMachine<T>: MonoBehaviour where T: Enum
{
    /// <summary>
    ///     Dictionary of states mapped to their behaviors
    /// </summary>
    protected Dictionary<T, IState<T>> States;
    
    /// <summary>
    ///     Current state behavior
    /// </summary>
    protected IState<T> CurrentState;

    protected virtual void Start()
    {
        CurrentState.EnterState();
    }

    private void Update()
    {
        // Check state to see if transition is necessary
        var nextStateKey = CurrentState.GetNextState();
        if (nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else
        {
            TransitionState(nextStateKey);
        }
    }

    /// <summary>
    ///     Exit current state and enter next
    /// </summary>
    /// <param name="nextStateKey">Next state to enter</param>
    public virtual void TransitionState(T nextStateKey)
    {
        CurrentState.ExitState();
        CurrentState = States[nextStateKey];
        CurrentState.EnterState();
    }
}

