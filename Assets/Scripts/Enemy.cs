using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// The health of the enemy.
    /// </summary>
    public float Health=100f;

    /// <summary>
    /// The player.
    /// </summary>
    public GameObject Player;

    /// <summary>
    /// A reference to the player movement.
    /// </summary>
    /// 
    public PlayerMovement PlayerMovement;

    void Update()
    {
        float playerDistance = Mathf.Sqrt(Mathf.Pow((Player.transform.position.x - transform.position.x),2) + Mathf.Pow((Player.transform.position.y - transform.position.y),2));

        if ((PlayerMovement.WeaponOut == true) && playerDistance < 5f)
        {
            TakeDamage(100/playerDistance * Time.deltaTime);
        }
    }
    public void TakeDamage(float damage)
    {
        Health -= damage;
        if(Health <= 0){
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
