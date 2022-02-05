using Assets.Scripts;
using Assets.Scripts.Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// The health of the enemy.
    /// </summary>
    private HealthComponent hc;

    /// <summary>
    /// The player.
    /// </summary>
    protected GameObject Player;

    /// <summary>
    /// A reference to the player movement.
    /// </summary>
    /// 
    private PlayerMovement PlayerMovement;

    private FloatToIntBank sanityDamageBank = new FloatToIntBank(0.2f);

    public CollisionTypeDetect CTD;
    public bool isLookingLeft = false;

    protected Vector2 startPos;
    protected bool startingLeft;

    public float desiredDistance = 5f;
    public float desireRange = 0.5f;

    public float speed = 3;

    public float lookingRange = 14f;

    private Animator anim;

    private void Start()
    {
        Player = GameObject.Find("Player");
        hc = GetComponent<HealthComponent>();
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        CTD = GetComponent<CollisionTypeDetect>();
        anim = GetComponent<Animator>();
        startPos = transform.position;
        startingLeft = isLookingLeft;
        UpdateDirection(isLookingLeft);
    }

    public bool PlayerBelow()
    {
        Rigidbody2D rbP = Player.gameObject.GetComponent<Rigidbody2D>();
        Rigidbody2D rbE = gameObject.GetComponent<Rigidbody2D>();
        Vector2 posP = rbP.position;
        Vector2 posE = rbE.position;
        return Math.Abs(posP.x - posE.x) < 1 && posP.y < posE.y - 2;
    }

    public Animator Animator()
    {
        return anim;
    }

    public void FixedUpdate()
    {
        float playerDistance = (Player.transform.position - transform.position).magnitude;

        if (playerDistance < 12)
        {
            sanityDamageBank.Deposit((float)(Time.deltaTime / playerDistance));
            int cash = sanityDamageBank.CashOut();
            if (cash > 0)
            {
                PlayerMovement.DamageSanity(cash);
            }
        }

        Move();
    }

    protected void UpdateDirection(bool lookingLeft)
    {
        isLookingLeft = lookingLeft;
        anim.SetBool("isLookingLeft", isLookingLeft);
    }

    public Vector2 LookingDirection()
    {
        if (isLookingLeft)
        {
            return Vector2.left;
        }
        return Vector2.right;
    }

    public virtual void Move()
    {
        
    }

    public bool PlayerInRange()
    {
        Rigidbody2D rbP = Player.gameObject.GetComponent<Rigidbody2D>();
        Rigidbody2D rbE = gameObject.GetComponent<Rigidbody2D>();
        Vector2 posP = rbP.position;
        Vector2 posE = rbE.position;
        return Vector2.Distance(posP, posE) < lookingRange;
    }

    public void TakeDamage(float damage)
    {
        hc.OnDamage((int)damage);
        if(hc.IsDead()){
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
