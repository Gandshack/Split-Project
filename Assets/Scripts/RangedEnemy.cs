using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public bool isLookingLeft=false;
    public float lookingRange=5f;
    public Vector2 lookingDirection;
    public Transform Projectile;

    public Transform shootLocation;
    // Start is called before the first frame update
    void Start()
    {
        lookingDirection=Vector2.right;
        if(isLookingLeft){
            lookingDirection=Vector2.left;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lookingDirection = Vector2.right;
        if (isLookingLeft)
        {
            lookingDirection = Vector2.left;
        }
        HandlePlayerChecking();
    }
    
    void HandlePlayerChecking()
    {
        RaycastHit2D hit=Physics2D.Raycast(transform.position,lookingDirection, lookingRange, 1);
        if (hit && hit.transform.gameObject.GetComponent<PlayerMovement>()!=null){
            HandleShooting();
        }
    }
    
    void HandleShooting()
    {
        Transform projectile=Instantiate(Projectile, shootLocation.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb)
        {
            Debug.Log(rb.position);
            rb.AddForce(lookingDirection * 10f);
        }
    }
}
