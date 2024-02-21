using System;

/// <summary>
///     Interface for state behaviors
/// </summary>
/// <typeparam name="T">Enum representing available states</typeparam>
public interface IState<out T> where T: Enum
{
    /// <summary>
    ///     Which state does this represent
    /// </summary>
    T StateKey { get; }
    
    /// <summary>
    ///     Called when entering state
    /// </summary>
    void EnterState();
    
    /// <summary>
    ///     Called when exiting state
    /// </summary>
    void ExitState();
    
    /// <summary>
    ///     Called in update loop each frame that the state is active
    /// </summary>
    void UpdateState();
    
    /// <summary>
    ///     Logic for determining whether to remain in current state or transition to another
    /// </summary>
    /// <returns>StateKey if no transition is necessary, key of next state otherwise.</returns>
    T GetNextState();
}
