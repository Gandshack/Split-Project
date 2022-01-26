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

    public bool WeaponOut = true;

    private Rigidbody2D playerBody;

    // Start is called before the first frame update
    void Start()
    {
        jumpAction = new ActionWithCooldown(0.1f, 0.05f, this.Jump);
        punchAction = new ActionWithCooldown(0.0f, 0.5f, this.Punch);

        playerBody = GetComponent<Rigidbody2D>();
        CTD = GetComponent<CollisionTypeDetect>();
        playerCamera = Camera.main;

        sanityHeal = new Countdown(0.1f);
    }

    bool Punch()
    {
        punchOrigin.LookAt(playerCamera.ScreenToWorldPoint(Input.mousePosition));
        Vector2 mousePos = Input.mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, punchOrigin.forward, 1, enemy);
        if (hit)
        {
            Debug.Log(hit.transform.name);
            hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(20);
        }
        return true;
    }

    bool Jump()
    {
        if (CTD.isGrounded)
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
        playerBody.AddForce(Vector2.left * 10f);
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

    // Update is called once per frame
    void FixedUpdate()
    {
        punchOrigin.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        // Determine whether the player is touching something

        if ((Input.GetAxisRaw("Vertical") > 0) ^ Input.GetKey(KeyCode.Space))
        {
            jumpAction.Trigger();
        }
        if (Input.GetMouseButtonDown(0))
        {
            punchAction.Trigger();
        }

        punchAction.Proceed(Time.deltaTime);
        jumpAction.Proceed(Time.deltaTime);

        // Player controlled horizontal force
        if (Input.GetAxisRaw("Horizontal") < 0 &&(!CTD.isLefted || CTD.isGrounded))
        {
            Vector2 velocity = new Vector2(-speed * Time.deltaTime, 0);
            if (CTD.SlopeLeft && CTD.isGrounded)
            {
                velocity.y = -velocity.x;
                velocity.x = 0;
            }
            playerBody.AddForce(1000 * velocity);
        }
        if (Input.GetAxisRaw("Horizontal") > 0 &&(!CTD.isRighted || CTD.isGrounded))
        {
            Vector2 velocity = new Vector2(speed * Time.deltaTime, 0);
            if (CTD.SlopeRight && CTD.isGrounded)
            {
                velocity.y = velocity.x;
                velocity.x = 0;
            }
            playerBody.AddForce(1000 * velocity);
        }

        // Auto-jump up slopes

        // Max speed
        if (playerBody.velocity.x < -maxSpeed)
        {
            playerBody.velocity = new Vector2(-maxSpeed, playerBody.velocity.y);
        }
        else if (playerBody.velocity.x > maxSpeed)
        {
            playerBody.velocity = new Vector2(maxSpeed, playerBody.velocity.y);
        }
        if(Input.GetKeyDown(KeyCode.E)){
            WeaponOut=!WeaponOut;
        }
        // Crouching
        if (_isSneaking() == true)
        {
            speed = 1f;
            maxSpeed = 2f;
        }
        else
        {
            speed = 3f;
            maxSpeed = 5f;
        }
        sanityHeal.Proceed(Time.deltaTime);
        if (!sanityHeal.IsRunning())
        {
            sc.Heal(1);
            sanityHeal = new Countdown(0.1f);
            sanityHeal.Start();
            sanityBar.UpdateHealth(sc.GetHealthFraction());
        }
    }
    public bool _isSneaking()
    {
        return Input.GetAxisRaw("Vertical")<0;
    }


    public void TakeDamage(float damage)
    {
        hc.OnDamage((int)damage);
        healthBar.UpdateHealth(hc.GetHealthFraction());
        if (hc.IsDead())
        {
            Die();
        }
    }

    public void DamageSanity(float damage)
    {
        Debug.Log("Sanity!");
        sc.OnDamage((int)damage);
        sanityHeal = new Countdown(3f);
        sanityHeal.Start();
        sanityBar.UpdateHealth(sc.GetHealthFraction());
        if (sc.IsDead())
        {
            Die();
        }
    }

    public void Die()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("player ded");
    }
}
