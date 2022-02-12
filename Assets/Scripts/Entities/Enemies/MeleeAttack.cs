using Assets.Scripts.Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    /// <summary>
    /// A reference to the player movement.
    /// </summary>
    private PlayerMovement Player;

    private Enemy ThisEnemy;

    public Countdown hitCooldown = new Countdown(1.0f);

    /// <summary>
    /// The damage to the player on hit.
    /// </summary>
    public float Damage = 10f;


    // Start is called before the first frame update
    void Start()
    {
        Player=GameObject.Find("Player").GetComponent<PlayerMovement>();
        ThisEnemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hitCooldown.Proceed(Time.deltaTime);
        if((Vector2.Distance(transform.position, Player.transform.position)<5f))
        {
            //var step= ThisEnemy.speed * Time.deltaTime;
            //transform.position=Vector2.MoveTowards(transform.position,new Vector2(Player.transform.position.x, transform.position.y),step);
            if(!hitCooldown.IsRunning())
            {
                Hit();
                hitCooldown.Start();
            }
        }
        if(Mathf.Abs(transform.position.x- Player.transform.position.x)<2f&& Player.transform.position.y-transform.position.y<4f)
        {
            ThisEnemy.Animator().SetBool("PlayerHere", true);
            Player.Pull();
            ThisEnemy.desiredDistance = 3;
        }
        else
        {
            ThisEnemy.Animator().SetBool("PlayerHere", false);
            ThisEnemy.desiredDistance = 1;
        }
    }

    void Hit()
    {
        if(Vector2.Distance(transform.position, Player.transform.position)<2f){
            Player.TakeDamage(Damage);
        }
    }
}
