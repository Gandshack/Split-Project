using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    Enemy enemy;
    public PlayerMovement player;
    public float speed=3f;
    // Start is called before the first frame update
    void Start()
    {
        enemy=GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if((Vector2.Distance(transform.position,player.transform.position)<5f)&&!player._isSneaking()){
            var step=speed*Time.deltaTime;
            transform.position=Vector2.MoveTowards(transform.position,new Vector2(player.transform.position.x, transform.position.y),step);
            hit();
        }
        
    }

    void hit()
    {
        if(Vector2.Distance(transform.position,player.transform.position)<2f){
            player.TakeDamage(10);
        }
    }
    IEnumerator burst()
    {
        speed*=10f;
        yield return new WaitForSeconds(0.3f);
        speed/=10f;
    }
}
