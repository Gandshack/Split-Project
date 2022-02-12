using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class GroundedEnemy : Enemy
    {
        public override void Move()
        {
            Rigidbody2D rbP = Player.gameObject.GetComponent<Rigidbody2D>();
            Rigidbody2D rbE = gameObject.GetComponent<Rigidbody2D>();
            Vector2 posP = rbP.position;
            Vector2 posE = rbE.position;
            // stop at walls
            if (CTD.EdgeLeft && rbE.velocity.x < 0 || CTD.EdgeRight && rbE.velocity.x > 0)
            {
                rbE.velocity = new Vector2(0, rbE.velocity.y);
            }
            // if player in range
            if (Vector2.Distance(posP, posE) < lookingRange)
            {
                if ((posP - posE).normalized.x * LookingDirection().x < 0)
                {
                    UpdateDirection(!isLookingLeft);
                }
                if (Vector2.Distance(posE, posP) < desiredDistance - desireRange)
                {
                    rbE.velocity = -LookingDirection() * speed + new Vector2(0, rbE.velocity.y);
                }
                else if (Vector2.Distance(posE, posP) > desiredDistance + desireRange)
                {
                    rbE.velocity = LookingDirection() * speed + new Vector2(0, rbE.velocity.y);
                }
            }
            // go back to start location
            else if ((posE - startPos).normalized.x < -0.5)
            {
                UpdateDirection(false);
                rbE.velocity = LookingDirection() * speed + new Vector2(0, rbE.velocity.y);
            }
            else if ((posE - startPos).normalized.x > 0.5)
            {
                UpdateDirection(true);
                rbE.velocity = LookingDirection() * speed + new Vector2(0, rbE.velocity.y);
            }
            else
            {
                UpdateDirection(startingLeft);
                rbE.velocity = new Vector2(0, rbE.velocity.y);
            }
            // If at an edge, don't move off it
            if (CTD.EdgeLeft && rbE.velocity.x < 0 || CTD.EdgeRight && rbE.velocity.x > 0)
            {
                rbE.velocity = new Vector2(0, rbE.velocity.y);
            }
        }
    }
}
