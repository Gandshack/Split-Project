using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public bool isLookingLeft=false;
    public float lookingRange=5f;
    public Vector2 lookingDirection;
    public Transform Projectile;
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
        HandlePlayerChecking();
    }
    
    void HandlePlayerChecking()
    {
        RaycastHit2D hit=Physics2D.Raycast(transform.position,lookingDirection, lookingRange);
        if(hit.transform.gameObject.GetComponent<PlayerMovement>()!=null){
            HandleShooting();
        }
    }
    
    void HandleShooting()
    {
        Transform projectile=Instantiate(Projectile, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().AddForce(lookingDirection*5f);
    }
}
