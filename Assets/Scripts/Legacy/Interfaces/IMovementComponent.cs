/// <summary>
/// Interface for handling entity movement.
/// </summary>
public interface IMovementComponent
{
    /// <summary>
    /// The movement speed of the entity's movement.
    /// </summary>
    public float MovementSpeed
    {
        get;
        set;
    }

    /// <summary>
    /// The maximum velocity the entity can move.
    /// </summary>
    public float MaxVelocity
    {
        get;
        set;
    }

    /// <summary>
    /// Whether the entity is blocked going left.
    /// </summary>
    public bool IsBlockedLeft
    {
        get;
        set;
    }

    /// <summary>
    /// Whether the entity is blocked from going right.
    /// </summary>
    public bool IsBlockedRight
    {
        get;
        set;
    }

    /// <summary>
    /// Function that handles moving left for the entity.
    /// </summary>
    void MoveLeft();

    /// <summary>
    /// Function that handles moving right for the entity.
    /// </summary>
    void MoveRight();

    /// <summary>
    /// Function that checks if the entity is moving at their max velocity.
    /// </summary>
    /// <returns>Whether or not the entity is moving at their max velocity.</returns>
    bool IsMovingMaxVelocity();

}
