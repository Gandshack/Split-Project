/// <summary>
/// Handles the cool down of abilities and actions.
/// </summary>
public interface ICooldownComponent
{
    /// <summary>
    /// Whether or not the action can be done. 
    /// If IsOnCoolDown is true, then the action cannot be done.
    /// If IsOnCoolDOwn is false, then the action can be done.
    /// </summary>
    public bool IsOnCoolDown
    {
        get;
        set;
    }

    /// <summary>
    /// The amount of time until the action can be done again.
    /// </summary>
    public float CooldownTimeNeeded
    {
        get;
        set;
    }

    /// <summary>
    /// The last time the action was called.
    /// </summary>
    public float LastTimeActionUsed
    {
        get;
        set;
    }

    /// <summary>
    /// Handles the cool down and resets the action.
    /// This is called in Update.
    /// </summary>
    /// <param name="currentTime">The current time for the frame.</param>
    void HandleCooldown(float currentTime);
}
