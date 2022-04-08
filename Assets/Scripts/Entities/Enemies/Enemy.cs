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

    private DamageFlash flashComp;

    private Animator anim;

    private AudioSource[] allMyAudioSources;

    private void Awake()
    {
        startPos = transform.position;
        startingLeft = isLookingLeft;
    }

    protected virtual void Start()
    {
        Player = GameObject.Find("Player");
        allMyAudioSources = GetComponents<AudioSource>();
        hc = GetComponent<HealthComponent>();
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        CTD = GetComponent<CollisionTypeDetect>();
        anim = GetComponent<Animator>();
        flashComp = GetComponent<DamageFlash>();
        UpdateDirection(isLookingLeft);
    }

    public void PlayAudio(int index)
    {
        allMyAudioSources[index].Play();
    }

    public void ReturnToStart()
    {
        transform.position = startPos;
    }

    public void FullHeal()
    {
        hc.MaxHeal();
    }

    public bool PlayerBelow()
    {
        Rigidbody2D rbP = Player.GetComponent<Rigidbody2D>();
        Rigidbody2D rbE = gameObject.GetComponent<Rigidbody2D>();
        Vector2 posP = rbP.position;
        Vector2 posE = rbE.position;
        RaycastHit2D rc = Physics2D.Raycast(posE, Vector2.down, 30f, 1 << 10);
        RaycastHit2D rcg = Physics2D.Raycast(posE, Vector2.down, 30f, CTD.groundLayer);
        return rc && (!rcg || (rc.distance < rcg.distance));
    }
    public int Dir(Vector2 pos, float leeway)
    {
        Rigidbody2D rbE = gameObject.GetComponent<Rigidbody2D>();
        if (rbE.position.x < pos.x - leeway)
        {
            return -1;
        }
        else if (rbE.position.x > pos.x + leeway)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public int PlayerDir(float leeway)
    {
        Rigidbody2D rbP = Player.GetComponent<Rigidbody2D>();
        return Dir(rbP.position, leeway);
    }
    public int StartingDir(float leeway)
    {
        return Dir(startPos, leeway);
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
        PlayAudio(0);
        flashComp.Do();
        hc.OnDamage((int)damage);
        if(hc.IsDead()){
            Die();
        }
    }
    public void Die()
    {
        Debug.Log("enemy died");
        Destroy(gameObject);
    }

    public string GetUniqueName()
    {
        return startPos.ToString("F");
    }
}
