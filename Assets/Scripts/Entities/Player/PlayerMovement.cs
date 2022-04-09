using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Lib;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Configurable values
    public float speed = 5f;
    public float gravity = -9f;
    public float jumpSpeed = 300;

    public HealthBar healthBar;
    public HealthBar sanityBar;
    public HealthComponent hc;
    public HealthComponent sc;

    public Countdown sanityHeal;

    public float maxSpeed = 5f;

    public CollisionTypeDetect CTD;

    private Camera playerCamera;

    public LayerMask enemy;
    public Transform punchOrigin;

    public ActionWithCooldown jumpAction;
    public ActionWithCooldown punchAction;

    public Countdown invulTime;

    public bool WeaponOut = true;

    private Rigidbody2D playerBody;

    private DamageFlash flashComp;

    public AudioSource punchSound;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        jumpAction = new ActionWithCooldown(0.1f, 0.05f, this.Jump);
        punchAction = new ActionWithCooldown(0.0f, 1.0f, this.Punch);

        playerBody = GetComponent<Rigidbody2D>();
        CTD = GetComponent<CollisionTypeDetect>();
        playerCamera = Camera.main;
        punchSound = GetComponent<AudioSource>();
        flashComp = GetComponent<DamageFlash>();

        sanityHeal = new Countdown(0.1f);
        invulTime = new Countdown(0.5f);

        anim = GetComponent<Animator>();
    }

    bool Punch()
    {
        Debug.Log("III\n");
        punchSound.Play();
        return true;
    }

    bool Jump()
    {
        if (CTD.IsGrounded)
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, 0);
            Vector2 vel2 = new Vector2(0, jumpSpeed);
            playerBody.AddForce(vel2);
            return true;
        }
        return false;
    }

    public void Pull()
    {
        playerBody.AddForce(Vector2.left * 50f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            Die();
        }
        if (collision.gameObject.layer == 9)
        {
            Die();
        }
    }

    public void AttackEndsEvent()
    {
        anim.SetBool("isAttacking", false);
    }

    void Update()
    {
        if ((Input.GetAxisRaw("Vertical") > 0) ^ Input.GetKey(KeyCode.Space))
        {
            jumpAction.Trigger();
        }

        if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.X))
        {
            anim.SetBool("isAttacking", true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        jumpAction.Proceed(Time.deltaTime);
        //punchAction.Proceed(Time.deltaTime);

        // Player controlled horizontal force
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            Vector2 velocity = CTD.SwapVelocitySlopeLeft();
            playerBody.AddForce(1000 * speed * Time.deltaTime * velocity);
            anim.SetBool("facingLeft", true);
            anim.SetBool("isMoving", true);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            Vector2 velocity = CTD.SwapVelocitySlopeRight();
            playerBody.AddForce(1000 * speed * Time.deltaTime * velocity);
            anim.SetBool("facingLeft", false);
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        // Auto-jump up slopes

        // Max speed

        if (Input.GetKeyDown(KeyCode.E)){
            WeaponOut=!WeaponOut;
        }
        // Crouching
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            speed = 1f;
            maxSpeed = 2f;
        }
        else
        {
            speed = 3f;
            maxSpeed = 5f;
        }

        var newXVel = Math.Max(-maxSpeed, playerBody.velocity.x);
        newXVel = Math.Min(maxSpeed, newXVel);
        playerBody.velocity = new Vector2(newXVel, playerBody.velocity.y);

        sanityHeal.Proceed(Time.deltaTime);
        if (!sanityHeal.IsRunning())
        {
            sc.Heal(1);
            sanityHeal = new Countdown(0.1f);
            sanityHeal.Start();
            sanityBar.UpdateHealth(sc.GetHealthFraction());
        }

        invulTime.Proceed(Time.deltaTime);
    }

    public void SetHealth(int health)
    {
        hc.SetHealth(health);
        healthBar.UpdateHealth(hc.GetHealthFraction());
    }

    public void TakeDamage(float damage)
    {
        if (!invulTime.IsRunning())
        {
            flashComp.Do();
            hc.OnDamage((int)damage);
            healthBar.UpdateHealth(hc.GetHealthFraction());
            if (hc.IsDead())
            {
                Die();
            }
            invulTime.Start();
        }
    }

    public void DamageSanity(float damage)
    {
        /*sc.OnDamage((int)damage);
        sanityHeal = new Countdown(3f);
        sanityHeal.Start();
        sanityBar.UpdateHealth(sc.GetHealthFraction());
        if (sc.IsDead())
        {
            Die();
        }*/
    }

    public void Die()
    {
        DataService.Instance.LoadSceneWithLoadingScreen("Test level");
    }
}
