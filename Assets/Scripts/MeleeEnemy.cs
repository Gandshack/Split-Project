using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    Enemy enemy;
    public PlayerMovement player;
    public float speed=1f;
    public float coolDown=0f;
    // Start is called before the first frame update
    void Start()
    {
        enemy=GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        coolDown-=Time.deltaTime;
        if((Vector2.Distance(transform.position,player.transform.position)<5f)&&!player._isSneaking()){
            var step=speed*Time.deltaTime;
            transform.position=Vector2.MoveTowards(transform.position,new Vector2(player.transform.position.x, transform.position.y),step);
            if(coolDown<=0f){
            hit();
            coolDown=1f;
            }
        }
        if(Mathf.Abs(transform.position.x-player.transform.position.x)<1f&&player.transform.position.y-transform.position.y<2f){
            player.playerBody.AddForce(Vector2.left*10f);
        }
        
    }

    void hit()
    {
        if(Vector2.Distance(transform.position,player.transform.position)<2f){
            player.TakeDamage(10);
        }
    }
}
