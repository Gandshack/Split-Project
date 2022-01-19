using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D controller;
    public float speed = 3f;
    public float health = 80;
    public Camera playerCamera;
    public LayerMask enemy;
    public Transform punchOrigin;
    // Start is called before the first frame update
    void Start()
    {
        controller=GetComponent<Rigidbody2D>();
        playerCamera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity=new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        controller.AddForce(1000*velocity * Time.deltaTime);
        punchOrigin.LookAt(playerCamera.ScreenToWorldPoint(Input.mousePosition));
        if(Input.GetMouseButtonDown(0)){
            punch();
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }

    void punch(){
        Vector2 mousePos=Input.mousePosition;
        RaycastHit2D hit=Physics2D.Raycast(transform.position, punchOrigin.right, 1, enemy);
        if(hit){
            Debug.Log(hit.transform.name);
            hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(20);
        }
    }
}
