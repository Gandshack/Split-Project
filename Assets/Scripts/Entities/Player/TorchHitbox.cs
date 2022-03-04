using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities.Player
{
    class TorchHitbox : MonoBehaviour
    {
        public bool onLeft;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 3)
            {
                Enemy e = collider.gameObject.GetComponentInParent<Enemy>();
                if (e == null)
                {
                    return;
                }
                e.TakeDamage(20);
                Rigidbody2D EnemyBody = collider.gameObject.GetComponent<Rigidbody2D>();
                if (onLeft)
                {
                    EnemyBody.AddForce(new Vector2(-1500, 0));
                }
                else
                {
                    EnemyBody.AddForce(new Vector2(1500, 0));
                }
            }
        }
    }
}
