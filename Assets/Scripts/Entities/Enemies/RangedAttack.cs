using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{

    public Transform ProjectileLeft;
    public Transform ProjectileRight;

    public float projForce = 500f;

    public Transform shootLocationLeft;
    public Transform shootLocationRight;

    private ActionWithCooldown shoot;

    private Enemy ThisEnemy;

    // Start is called before the first frame update
    void Start()
    {
        shoot = new ActionWithCooldown(0.0f, 2f, this.Shoot);
        ThisEnemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        shoot.Proceed(Time.deltaTime);
        if (ThisEnemy.PlayerInRange())
        {
            shoot.Trigger();
        }
    }

    bool Shoot()
    {
        Transform projectile;
        if (ThisEnemy.isLookingLeft)
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
            rb.AddForce(ThisEnemy.LookingDirection() * projForce);
            rb.AddForce(Vector2.up * projForce/4);

        }
        return true;
    }
}
