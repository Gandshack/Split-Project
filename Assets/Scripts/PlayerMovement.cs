using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    public float speed = 3f;
    public Camera playerCamera;
    public LayerMask enemy;
    public Transform punchOrigin;
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
        punchOrigin.LookAt(playerCamera.ScreenToWorldPoint(Input.mousePosition));
        if(Input.GetMouseButtonDown(0)){
            punch();
        }
    }
    void punch(){
        Vector2 mousePos=Input.mousePosition;
        RaycastHit2D hit=Physics2D.Raycast(transform.position, punchOrigin.right, 1, enemy);
        if(hit){
            Debug.Log(hit.transform.name);
            hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(20);
        }
    }
}
