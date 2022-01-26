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
    private GameObject Player;

    /// <summary>
    /// A reference to the player movement.
    /// </summary>
    /// 
    private PlayerMovement PlayerMovement;

    private Countdown sanityCountdown = new Countdown(0.2f);

    private void Start()
    {
        Player = GameObject.Find("Player");
        hc = GetComponent<HealthComponent>();
        PlayerMovement = Player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        float playerDistance = (Player.transform.position - transform.position).magnitude;

        if (playerDistance < 10)
        {
            sanityCountdown.Proceed((float)(Time.deltaTime / Math.Max(playerDistance, 0.5)));

            if (!sanityCountdown.IsRunning())
            {
                PlayerMovement.DamageSanity(1);
                sanityCountdown.Start();
            }
        }
        if ((PlayerMovement.WeaponOut == true) && playerDistance < 5f)
        {
            TakeDamage(100/playerDistance * Time.deltaTime);
        }
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
