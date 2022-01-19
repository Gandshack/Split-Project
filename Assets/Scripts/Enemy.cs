using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health=100f;
    public Transform dir;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(player.position.x-transform.position.x)<3.5f){
            dir.LookAt(player);
        }
        if(Mathf.Abs(player.position.x-transform.position.x)<1f){
            hit();
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
    public void hit()
    {
        RaycastHit2D _hit=Physics2D.Raycast(transform.position,dir.forward,1);
        if(_hit){
            player.GetComponent<PlayerMovement>().TakeDamage(10);
        }
        Sleep(0.5f);
    }
    IEnumerator Sleep(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
