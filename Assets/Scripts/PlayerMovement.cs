using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Lib;
using Assets.Scripts;

public class PlayerMovement : MonoBehaviour
{
    // Configurable values
    public float speed = 5f;
    public float health = 80;
    public float gravity = -9f;
    public float jumpSpeed = 300;

    public float maxSpeed = 5f;

    public Rigidbody2D playerBody;

    // Touching detection
    public bool isGrounded = false;
    public RectTransform isGroundedChecker;
    public bool isLefted = false;
    public RectTransform isLeftedChecker;
    public bool isRighted = false;
    public RectTransform isRightedChecker;
    public LayerMask groundLayer;

    public bool SlopeLeft = false;

    public Camera playerCamera;
    public LayerMask enemy;
    public Transform punchOrigin;

    public ActionWithCooldown jumpAction;
    public ActionWithCooldown punchAction;

    public PlayerMovement player;
    public bool WeaponOut = true;

    // Start is called before the first frame update
    void Start()
    {
        jumpAction = new ActionWithCooldown(0.1f, 0.05f, this.Jump);
        punchAction = new ActionWithCooldown(0.0f, 0.5f, this.Punch);

        playerBody = GetComponent<Rigidbody2D>();
        playerCamera = Camera.main;
    }

    bool Punch()
    {
        punchOrigin.LookAt(playerCamera.ScreenToWorldPoint(Input.mousePosition));
        Vector2 mousePos = Input.mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, punchOrigin.right, 1, enemy);
        if (hit)
        {
            Debug.Log(hit.transform.name);
            hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(20);
        }
        return true;
    }

    bool Jump()
    {
        isGrounded = OnGround();
        if (isGrounded)
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, 0);
            Vector2 vel2 = new Vector2(0, jumpSpeed);
            playerBody.AddForce(vel2);
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == 9)
        {
            Die();
        }
    }

    /*bool OnSlope()
     {
         Rect r = isGroundedChecker.rect;
         float y = isGroundedChecker.position.y;
         float x1 = isGroundedChecker.position.x - r.size.x/2;
         float x2 = isGroundedChecker.position.x + r.size.x / 2;
         RaycastHit2D hit = Physics2D.Raycast(new Vector2(x1, y), new Vector2(0, -1), 0.5f, groundLayer);
         RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(x1+0.01f, y), new Vector2(0, -1), 0.5f, groundLayer);
         RaycastHit2D hit3 = Physics2D.Raycast(new Vector2(x2-0.01f, y), new Vector2(0, -1), 0.5f, groundLayer);
         RaycastHit2D hit4 = Physics2D.Raycast(new Vector2(x2, y), new Vector2(0, -1), 0.5f, groundLayer);

         if (hit && hit2)
         {
             Debug.Log(hit.distance);
             Debug.Log(hit2.distance);
             return Math.Min(hit.distance, hit2.distance) < 0.2f && Math.Abs(hit.distance - hit2.distance) > 0.005f;
         }
         if (hit3 && hit4)
         {
             Debug.Log(hit3.distance);
             Debug.Log(hit4.distance);
             return Math.Min(hit3.distance, hit4.distance) < 0.2f && Math.Abs(hit3.distance - hit4.distance) > 0.005f;
         }
         return false;
     }*/

    bool OnGround()
    {
        Rect r = isGroundedChecker.rect;
        float y = isGroundedChecker.position.y;
        float x1 = isGroundedChecker.position.x - r.size.x / 2;
        float x2 = isGroundedChecker.position.x + r.size.x / 2;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x1, y), new Vector2(0, -1), 0.05f, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(x2, y), new Vector2(0, -1), 0.05f, groundLayer);
         return hit || hit2;
    }

    bool IsSlopeLeft()
    {
        Rect r = isGroundedChecker.rect;
        float y1 = isGroundedChecker.position.y;
        float y2 = isGroundedChecker.position.y + r.size.x / 2;
        float x = isGroundedChecker.position.x - r.size.x / 2;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, y1), new Vector2(-1, 0), 0.05f, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(x, y2), new Vector2(-1, 0), 0.05f, groundLayer);

        /*if (hit)
        {
            Debug.Log(hit.distance);
        }
        else
        {
            Debug.Log("No on 1");
        }
        if (hit2)
        {
            Debug.Log(hit2.distance);
        }
        else
        {
            Debug.Log("No on 2");
        }*/
        return hit && !hit2;
    }

    // Update is called once per frame
    void Update()
    {
        // Determine whether the player is touching something
        //onSlope = OnSlope();
        isGrounded = OnGround();
        isLefted = CheckIfSided(isLeftedChecker);
        isRighted = CheckIfSided(isRightedChecker);
        SlopeLeft = IsSlopeLeft();

        if ((Input.GetAxisRaw("Vertical") > 0) ^ Input.GetKey(KeyCode.Space))
        {
            jumpAction.Trigger();
        }
        if (Input.GetMouseButtonDown(0))
        {
            punchAction.Trigger();
        }

        punchAction.Proceed(Time.deltaTime);
        jumpAction.Proceed(Time.deltaTime);

        // Player controlled horizontal force
        if (Input.GetAxisRaw("Horizontal") < 0 &&(!isLefted || isGrounded))
        {
            Vector2 velocity = new Vector2(-speed * Time.deltaTime, 0);
            if (SlopeLeft && isGrounded)
            {
                velocity.y = -velocity.x;
                velocity.x = 0;
            }
            playerBody.AddForce(1000 * velocity);
        }
        if (Input.GetAxisRaw("Horizontal") > 0 &&(!isRighted || isGrounded))
        {
            Vector2 velocity = new Vector2(speed * Time.deltaTime, 0);
            playerBody.AddForce(1000 * velocity);
        }

        // Auto-jump up slopes

        // Max speed
        if (playerBody.velocity.x < -maxSpeed)
        {
            playerBody.velocity = new Vector2(-maxSpeed, playerBody.velocity.y);
        }
        else if (playerBody.velocity.x > maxSpeed)
        {
            playerBody.velocity = new Vector2(maxSpeed, playerBody.velocity.y);
        }
        if(Input.GetKeyDown(KeyCode.E)){
            WeaponOut=!WeaponOut;
        }
        // Crouching
        if (player._isSneaking() == true)
        {
            speed = 1f;
            maxSpeed = 2f;
        }
        else
        {
            speed = 3f;
            maxSpeed = 5f;
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
}
