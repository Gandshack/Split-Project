using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    /// <summary>
    /// A reference to self.
    /// </summary>
    public Enemy ThisEnemy;

    /// <summary>
    /// A reference to the player movement.
    /// </summary>
    public PlayerMovement Player;

    /// <summary>
    /// The speed at which the enemy moves.
    /// </summary>
    public float Speed=1f;

    /// <summary>
    /// How long it takes for another hit.
    /// </summary>
    public float CoolDown = 1.0f;

    /// <summary>
    /// How long until the next hit.
    /// </summary>
    public float CoolDownLeft=0f;

    /// <summary>
    /// The damage to the player on hit.
    /// </summary>
    public float Damage = 10f;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        ThisEnemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        Player=GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CoolDownLeft -= Time.deltaTime;
        if((Vector2.Distance(transform.position, Player.transform.position)<5f)&&!Player._isSneaking())
        {
            var step= Speed * Time.deltaTime;
            transform.position=Vector2.MoveTowards(transform.position,new Vector2(Player.transform.position.x, transform.position.y),step);
            if(CoolDownLeft <= 0f)
            {
                Hit();
                CoolDownLeft = CoolDown;
            }
        }
        if(Mathf.Abs(transform.position.x- Player.transform.position.x)<1f&& Player.transform.position.y-transform.position.y<2f)
        {
            anim.SetFloat("Speed", 1);
            Player.Pull();
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }

    void Hit()
    {
        if(Vector2.Distance(transform.position, Player.transform.position)<2f){
            Player.TakeDamage(Damage);
        }
    }
}
