using Assets.Scripts.Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBullet : MonoBehaviour
{
    private Countdown lifetime;

    // Start is called before the first frame update
    void Start()
    {
        lifetime = new Countdown(0.5f);
        lifetime.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Rigidbody2D>().AddForce(Vector2.left * Time.deltaTime);
        lifetime.Proceed(Time.deltaTime);
        Debug.Log("RUNNING!");
        if (!lifetime.IsRunning())
        {
            Debug.Log("DONE!");
            Destroy(this.gameObject);
        }
    }
}
