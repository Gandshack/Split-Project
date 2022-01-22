
/// <summary>
/// Handles the sneaking behavior for entities.
/// </summary>
public interface ISneakingComponent
{
    /// <summary>
    /// Whether the entity is sneaking or not.
    /// </summary>
    public bool IsSneaking
    {
        get;
        set;
    }

    /// <summary>
    /// Movement speed when entity is sneaking.
    /// </summary>
    public float SneakMovementSpeed
    {
        get;
        set;
    }

    /// <summary>
    /// The maximum velocity an entity can move when sneaking.
    /// </summary>
    public float MaximumSneakingVelocity
    {
        get;
        set;
    }

    /// <summary>
    /// Function that handles sneaking left for the entity.
    /// </summary>
    void SneakLeft();

    /// <summary>
    /// Function that handles sneaking right for the entity.
    /// </summary>
    void SneakRight();

    /// <summary>
    /// Function that checks if the entity is sneaking at their max velocity.
    /// </summary>
    /// <returns>Whether or not the entity is sneaking at their max velocity.</returns>
    bool IsSneakingMaxVelocity();

}
