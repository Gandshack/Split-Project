using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class BalloonEnemy : FloatingEnemy
    {
        public bool diving = false;
        public bool rising = false;

        public override void Move()
        {
            if (PlayerBelow() && PlayerInRange())
            {
                Dive();
            }
            base.Move();
            if (diving && !rising)
            {
                Rigidbody2D r = gameObject.GetComponent<Rigidbody2D>();
                r.AddForce(Vector2.down * 1000f*Time.deltaTime);
            }
            else if (rising && !diving)
            {
                Rigidbody2D r = gameObject.GetComponent<Rigidbody2D>();
                r.AddForce(Vector2.up * 1000f*Time.deltaTime);
            }
            if (CTD.IsGrounded)
            {
                diving = false;
                rising = true;
            }
            if (CTD.IsCeiled)
            {
                rising = false;
            }
        }

        private void Dive()
        {
            diving = true;
        }
    }
}
