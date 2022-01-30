using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float desiredDistance=5f;
    public float speed = 3;

    public bool isLookingLeft=false;
    public float lookingRange=14f;
    public Transform ProjectileLeft;
    public Transform ProjectileRight;

    public float projForce = 500f;

    public Transform shootLocationLeft;
    public Transform shootLocationRight;

    public CollisionTypeDetect CTD;

    private Vector2 startPos;
    private bool startingLeft;
    private Vector2 lookingDirection;

    private ActionWithCooldown shoot;
    private Animator anim;

    /// <summary>
    /// A reference to the player movement.
    /// </summary>
    private PlayerMovement Player;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        shoot = new ActionWithCooldown(0.0f, 2f, this.Shoot);
        Player=GameObject.Find("Player").GetComponent<PlayerMovement>();
        CTD = GetComponent<CollisionTypeDetect>();
        startPos = transform.position;
        startingLeft = isLookingLeft;
        UpdateDirection(isLookingLeft);
    }

    // Update is called once per frame
    void Update()
    {
        shoot.Proceed(Time.deltaTime);
        HandlePlayerChecking();
    }

    void UpdateDirection(bool lookingLeft)
    {
        isLookingLeft = lookingLeft;
        lookingDirection = Vector2.right;
        if (isLookingLeft)
        {
            lookingDirection = Vector2.left;
        }
        anim.SetBool("isLookingLeft", isLookingLeft);
    }

    void HandlePlayerChecking()
    {
        Rigidbody2D rbP = Player.gameObject.GetComponent<Rigidbody2D>();
        Rigidbody2D rbE = gameObject.GetComponent<Rigidbody2D>();
        Vector2 posP = rbP.position;
        Vector2 posE = rbE.position;
        if (Vector2.Distance(posP, posE) < lookingRange)
        {
            if ((posP - posE).normalized.x * lookingDirection.x < 0)
            {
                UpdateDirection(!isLookingLeft);
            }
            shoot.Trigger();
            if (Vector2.Distance(posE, posP) < desiredDistance - 0.5)
            {
                rbE.velocity = -lookingDirection * speed;
            }
            else if (Vector2.Distance(posE, posP) > desiredDistance + 0.5)
            {
                rbE.velocity = lookingDirection * speed;
            }
        }
        else if ((posE - startPos).normalized.x < -0.5)
        {
            UpdateDirection(false);
            rbE.velocity = lookingDirection * speed;
        }
        else if ((posE - startPos).normalized.x > 0.5)
        {
            UpdateDirection(true);
            rbE.velocity = lookingDirection * speed;
        }
        else
        {
            UpdateDirection(startingLeft);
            rbE.velocity = new Vector2(0, rbE.velocity.y);
        }
        // If at an edge, don't move off it
        if (CTD.EdgeLeft && rbE.velocity.x < 0 || CTD.EdgeRight && rbE.velocity.x > 0)
        {
            rbE.velocity = new Vector2(0, rbE.velocity.y);
        }
    }

    bool Shoot()
    {
        Transform projectile;
        if (isLookingLeft)
        {
            projectile = Instantiate(ProjectileLeft, shootLocationLeft.position, Quaternion.identity);
        }
        else
        {
            projectile = Instantiate(ProjectileRight, shootLocationRight.position, Quaternion.identity);
        }
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.AddForce(lookingDirection * projForce);
            rb.AddForce(Vector2.up * projForce/4);

        }
        return true;
    }
}
