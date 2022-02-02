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
            Rigidbody2D rbP = Player.gameObject.GetComponent<Rigidbody2D>();
            Rigidbody2D rbE = gameObject.GetComponent<Rigidbody2D>();
            Vector2 posP = rbP.position;
            Vector2 posE = rbE.position;
            // reverse at walls
            if (CTD.isLefted && rbE.velocity.x <= 0 || CTD.isRighted && rbE.velocity.x >= 0)
            {
                UpdateDirection(!isLookingLeft);
            }
            rbE.velocity = LookingDirection() * speed + new Vector2(0, rbE.velocity.y);
            if (!CTD.isCeiled)
            {
                rbE.AddForce(new Vector2(0, 200 * Time.deltaTime * speed));
            }
            /*if (Vector2.Distance(posP, posE) < lookingRange)
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
            if ((posE - startPos).normalized.x < -0.5)
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
            }*/
        }
    }
}
