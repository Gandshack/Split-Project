using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class FloatingEnemy : Enemy
    {
        public override void Move()
        {
            Rigidbody2D rbE = gameObject.GetComponent<Rigidbody2D>();

            // reverse at walls
            if (CTD.IsLefted && rbE.velocity.x <= 0 || CTD.IsRighted && rbE.velocity.x >= 0)
            {
                UpdateDirection(!isLookingLeft);
            }
            rbE.velocity = LookingDirection() * speed + new Vector2(0, rbE.velocity.y);
            if (!CTD.IsCeiled)
            {
                rbE.AddForce(new Vector2(0, 200 * Time.deltaTime * speed));
            }
        }
    }
}
