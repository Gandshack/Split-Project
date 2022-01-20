using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D controller;
    public float speed = 5f;
    public float health = 80;
    public bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public Camera playerCamera;
    public LayerMask enemy;
    public Transform punchOrigin;
    public float gravity=-9f;
    // Start is called before the first frame update
    void Start()
    {
        controller=GetComponent<Rigidbody2D>();
        playerCamera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfGrounded();
        Vector2 velocity=new Vector2(Input.GetAxisRaw("Horizontal"),0);
        if(isGrounded && Input.GetAxisRaw("Vertical") > 0)
        {
            Vector2 vel2 = new Vector2(0, 10000*speed);
            controller.AddForce(vel2 * Time.deltaTime);
        }
        velocity.y = controller.velocity.y;
        controller.velocity = velocity;
        
        punchOrigin.LookAt(playerCamera.ScreenToWorldPoint(Input.mousePosition));
        if(Input.GetMouseButtonDown(0)){
            punch();
        }
    }
    public bool _isSneaking()
    {
        return Input.GetAxisRaw("Vertical")<0;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log("player ded");
    }

    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
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
