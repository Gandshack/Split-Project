using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Configurable values
    public float speed = 5f;
    public float health = 80;
    public float gravity = -9f;
    public float jumpSpeed = 300;
    public float jumpLeeway = 0.1f;

    Rigidbody2D playerBody;

    // Touching detection
    [SerializeField] bool isGrounded = false;
    [SerializeField] RectTransform isGroundedChecker;
    [SerializeField] bool isLefted = false;
    [SerializeField] RectTransform isLeftedChecker;
    [SerializeField] bool isRighted = false;
    [SerializeField] RectTransform isRightedChecker;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] Camera playerCamera;
    [SerializeField] LayerMask enemy;
    [SerializeField] Transform punchOrigin;

    [SerializeField] float jumpDelay = 0;

    [SerializeField] bool isJumping = false;
    [SerializeField] float leewayLeft = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerCamera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Determine whether the player is touching something
        isGrounded = CheckIfSided(isGroundedChecker);
        isLefted = CheckIfSided(isLeftedChecker);
        isRighted = CheckIfSided(isRightedChecker);

        // Must wait before jumping again
        if (isJumping)
        {
            jumpDelay -= Time.deltaTime;
        }
        if (jumpDelay < 0)
        {
            isJumping = false;
        }

        // Can jump a little bit too early and still succeed
        if ((Input.GetAxisRaw("Vertical") > 0) ^ Input.GetKey(KeyCode.Space))
        {
            leewayLeft = jumpLeeway;
        }
        if (leewayLeft > 0)
        {
            leewayLeft -= Time.deltaTime;
        }

        if (!isJumping && isGrounded && leewayLeft > 0)
        {
            isJumping = true;
            jumpDelay = 0.05f;
            Vector2 vel2 = new Vector2(0, jumpSpeed);
            playerBody.AddForce(vel2);
        }

        // Player controlled horizontal force
        Vector2 velocity = new Vector2(speed * Time.deltaTime, 0);
        if (Input.GetAxisRaw("Horizontal") < 0 &&!isLefted)
        {
            playerBody.AddForce(-1000 * velocity);
        }
        if (Input.GetAxisRaw("Horizontal") > 0 &&!isRighted)
        {
            playerBody.AddForce(1000 * velocity);
        }

        // Air friction
        if (playerBody.velocity.x > 0)
        {
            playerBody.AddForce(-100 * velocity);
        }
        if (playerBody.velocity.x < 0)
        {
            playerBody.AddForce(100 * velocity);
        }
        // Max speed
        if (playerBody.velocity.x < -5)
        {
            playerBody.velocity = new Vector2(-5, playerBody.velocity.y);
        }
        else if (playerBody.velocity.x > 5)
        {
            playerBody.velocity = new Vector2(5, playerBody.velocity.y);
        }
        // Punching
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
