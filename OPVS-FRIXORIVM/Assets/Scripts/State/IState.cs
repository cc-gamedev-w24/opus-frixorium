/// <summary>
///     Interface for state behaviors
/// </summary>
public interface IState
{
    
    /// <summary>
    ///     Called when entering state
    /// </summary>
    void OnEnter();
    
    /// <summary>
    ///     Called when exiting state
    /// </summary>
    void OnExit();
    
    /// <summary>
    ///     Called in update loop each frame that the state is active
    /// </summary>
    void Update();

    /// <summary>
    ///     Called in fixed update each frame that state is active
    /// </summary>
    void FixedUpdate();
}
