using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJumpComponent
{
    /// <summary>
    /// Whether the entity is touching the ground.
    /// </summary>
    public bool IsGrounded
    {
        get;
        set;
    }

    /// <summary>
    /// Whether or not the entity is jumping.
    /// </summary>
    public bool IsJumping
    {
        get;
        set;
    }

    /// <summary>
    /// The momentum of the enity's jumping.
    /// </summary>
    public float JumpMomentum
    {
        get;
        set;
    }

    /// <summary>
    /// Function that handles Jumping for the entity.
    /// </summary>
    void Jump();

}
