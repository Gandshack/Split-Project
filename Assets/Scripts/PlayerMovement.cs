using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D controller;
    public float speed = 5f;
    public float health = 80;

    public bool isGrounded = false;
    public RectTransform isGroundedChecker;
    public bool isJumping = false;

    public bool isLefted = false;
    public RectTransform isLeftedChecker;
    public bool isRighted = false;
    public RectTransform isRightedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public Camera playerCamera;
    public LayerMask enemy;
    public Transform punchOrigin;
    public float gravity=-9f;

    public float jumpDelay = 0;
    // Start is called before the first frame update
    void Start()
    {
        controller=GetComponent<Rigidbody2D>();
        playerCamera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckIfSided(isGroundedChecker);
        isLefted = CheckIfSided(isLeftedChecker);
        isRighted = CheckIfSided(isRightedChecker);
        Vector2 velocity = new Vector2(1,0);
        if (isJumping)
        {
            jumpDelay -= Time.deltaTime;
        }
        if (jumpDelay < 0)
        {
            isJumping = false;
        }
        if (!isJumping && isGrounded && Input.GetAxisRaw("Vertical") > 0)
        {
            isJumping = true;
            jumpDelay = 0.1f;
            Vector2 vel2 = new Vector2(0, 100*speed);
            controller.AddForce(vel2);
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

    bool CheckIfSided(RectTransform isChecker)
    {
        Rect r = isChecker.rect;
        //Debug.DrawRay(isChecker.position, r.size, Color.black);
        Collider2D collider = Physics2D.OverlapBox(isChecker.position, r.size, 0, groundLayer);
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
