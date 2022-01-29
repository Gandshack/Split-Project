using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    /// <summary>
    /// A reference to the player movement.
    /// </summary>
    private PlayerMovement Player;
    public float desiredDistance=5f;
    public float speed = 3;

    public bool isLookingLeft=false;
    public float lookingRange=12f;
    public Vector2 lookingDirection;
    public Transform ProjectileLeft;
    public Transform ProjectileRight;

    public float projForce = 500f;

    private ActionWithCooldown shoot;
    private Animator anim;

    public Transform shootLocationLeft;
    public Transform shootLocationRight;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        shoot = new ActionWithCooldown(0.0f, 2f, this.Shoot);
        Player=GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDirection();
        shoot.Proceed(Time.deltaTime);
        HandlePlayerChecking();
    }

    void UpdateDirection()
    {
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
        Debug.Log(Vector2.Distance(posP, posE));
        Debug.Log(lookingRange);
        if (Vector2.Distance(posP, posE) < lookingRange)
        {
            if ((posP - posE).normalized.x * lookingDirection.x < 0)
            {
                isLookingLeft = !isLookingLeft;
                UpdateDirection();
            }
            shoot.Trigger();
            if (Vector2.Distance(posE, posP) < desiredDistance - 0.5)
            {
                rbE.velocity = -lookingDirection * speed;
            }
            if (Vector2.Distance(posE, posP) > desiredDistance + 0.5)
            {
                rbE.velocity = lookingDirection * speed;
            }
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
