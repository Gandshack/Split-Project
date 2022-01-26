using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{

    /// <summary>
    /// A reference to self.
    /// </summary>
    public Enemy ThisEnemy;

    /// <summary>
    /// A reference to the player movement.
    /// </summary>
    public PlayerMovement Player;
    public float desiredDistance=4f;

    public bool isLookingLeft=false;
    public float lookingRange=5f;
    public Vector2 lookingDirection;
    public Transform Projectile;
    public float projForce = 500f;

    private ActionWithCooldown shoot;

    public Transform shootLocation;
    // Start is called before the first frame update
    void Start()
    {
        shoot = new ActionWithCooldown(0.0f, 2f, this.Shoot);
        ThisEnemy = GetComponent<Enemy>();
        Player=GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        lookingDirection = Vector2.right;
        if (isLookingLeft)
        {
            lookingDirection = Vector2.left;
        }
        shoot.Proceed(Time.deltaTime);
        HandlePlayerChecking();
    }
    
    void HandlePlayerChecking()
    {
        Rigidbody2D rbP = Player.gameObject.GetComponent<Rigidbody2D>();
        Rigidbody2D rbE = ThisEnemy.gameObject.GetComponent<Rigidbody2D>();
        Vector2 posP = rbP.position;
        Vector2 posE = shootLocation.position;
        if ((posP - posE).magnitude < lookingRange && (posP - posE).normalized.x * lookingDirection.x > 0)
        {
            shoot.Trigger();
            if(Vector2.Distance(transform.position, Player.transform.position)<desiredDistance){
                rbE.velocity=-lookingDirection;
            }
            if(Vector2.Distance(transform.position, Player.transform.position)>desiredDistance){
                rbE.velocity=lookingDirection;
            }
        }
        
    }
    
    bool Shoot()
    {
        Transform projectile=Instantiate(Projectile, shootLocation.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb)
        {
            Debug.Log("hi");
            rb.AddForce(lookingDirection * projForce);
            rb.AddForce(Vector2.up * projForce/4);

        }
        return true;
    }
}
