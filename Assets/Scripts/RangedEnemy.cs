using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
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
        shoot = new ActionWithCooldown(0.0f, 1f, this.Shoot);
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
        RaycastHit2D hit=Physics2D.Raycast(transform.position,lookingDirection, lookingRange, 1);
        if (hit && hit.transform.gameObject.GetComponent<PlayerMovement>()!=null){
            shoot.Trigger();
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
        }
        return true;
    }
}
