using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D controller;
    public float speed = 3f;
    public float health = 80;
    public bool isGrounded = false;
    public Transform isGroundedChecker;
    public bool isLefted = false;
    public Transform isLeftedChecker;
    public bool isRighted = false;
    public Transform isRightedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public Camera playerCamera;
    public LayerMask enemy;
    public Transform punchOrigin;
    // Start is called before the first frame update
    void Start()
    {
        controller=GetComponent<Rigidbody2D>();
        playerCamera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckIfGrounded(isGroundedChecker);
        isLefted = CheckIfGrounded(isLeftedChecker);
        isRighted = CheckIfGrounded(isRightedChecker);
        Vector2 velocity = new Vector2(1,0);
        if(isGrounded && Input.GetAxisRaw("Vertical") > 0)
        {
            Vector2 vel2 = new Vector2(0, 1000*speed);
            controller.AddForce(vel2 * Time.deltaTime);
        }
        if (Input.GetAxisRaw("Horizontal") < 0 &&!isLefted)
        {
            controller.AddForce(-1000 * speed * velocity * Time.deltaTime);
        }
        if (Input.GetAxisRaw("Horizontal") > 0 &&!isRighted)
        {
            controller.AddForce(1000 * speed * velocity * Time.deltaTime);
        }
        if (controller.velocity.x < -5)
        {
            controller.velocity = new Vector2(-5, controller.velocity.y);
        }
        else if (controller.velocity.x > 5)
        {
            controller.velocity = new Vector2(5, controller.velocity.y);
        }
        punchOrigin.LookAt(playerCamera.ScreenToWorldPoint(Input.mousePosition));
        if(Input.GetMouseButtonDown(0)){
            punch();
        }
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
        Destroy(gameObject);
    }

    bool CheckIfGrounded(Transform isChecker)
    {
        Collider2D collider = Physics2D.OverlapCircle(isChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            return true;
        }
        else
        {
            return false;
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
