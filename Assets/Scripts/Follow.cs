using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Rigidbody2D objectToFollow;
    public Rigidbody2D cameraBody;

    public float speed = 1.0f;

    void Start()
    {
        cameraBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 dist = objectToFollow.position - cameraBody.position;
        cameraBody.velocity = 10 * speed * dist;
    }
}
