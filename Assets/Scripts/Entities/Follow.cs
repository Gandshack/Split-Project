using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    /// <summary>
    /// The Rigidbody2D to follow
    /// </summary>
    public Rigidbody2D BodyToFollow;

    /// <summary>
    /// The Rigidbody2D of this entity.
    /// </summary>
    public Rigidbody2D ThisBody;

    /// <summary>
    /// The speed at which the entity follows.
    /// </summary>
    public float Speed = 1.0f;

    void Start()
    {
        ThisBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 dist = BodyToFollow.position - ThisBody.position;
        // This entity will reach the other one in 1 / Speed seconds,
        // regardless of initial distance.
        ThisBody.velocity = Speed * dist;
    }

    void TravelInstantly()
    {
        ThisBody.position = BodyToFollow.position;
    }
}
