using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health=100f;
    public GameObject player;
    public PlayerMovement playerMovement;
    float playerDistance;

    void Update()
    {
        playerDistance = Mathf.Sqrt(Mathf.Pow((player.transform.position.x - transform.position.x),2) + Mathf.Pow((player.transform.position.y - transform.position.y),2));

        if ((playerMovement.WeaponOut == true) && playerDistance < 10f)
        {
            TakeDamage(100/playerDistance * Time.deltaTime);
        }
    }
    public void TakeDamage(float damage)
    {
        health-=damage;
        if(health<=0){
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
