using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    public float speed = 3f;
    public Camera playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        controller=GetComponent<CharacterController>();
        playerCamera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity=new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        controller.Move(velocity*Time.deltaTime*speed);
    }
}
