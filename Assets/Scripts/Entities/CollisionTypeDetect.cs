using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CollisionTypeDetect : MonoBehaviour
    {
        // Touching detection

        public LayerMask groundLayer;

        public CollisionType ct;

        private Collider2D[] cols;

        public bool IsGrounded
        {
            get
            {
                return ct.isGrounded;
            }
        }

        public bool IsCeiled
        {
            get
            {
                return ct.isCeiled;
            }
        }

        public bool IsLefted
        {
            get
            {
                return ct.isLefted;
            }
        }

        public bool IsRighted
        {
            get
            {
                return ct.isRighted;
            }
        }

        public bool SlopeLeft
        {
            get
            {
                return ct.SlopeLeft;
            }
        }

        public bool SlopeRight
        {
            get
            {
                return ct.SlopeRight;
            }
        }

        public bool EdgeLeft
        {
            get
            {
                return ct.EdgeLeft;
            }
        }

        public bool EdgeRight
        {
            get
            {
                return ct.EdgeRight;
            }
        }

        void Start()
        {
            cols = GetComponents<Collider2D>();
            ct = new CollisionType(groundLayer);
        }

        public Vector2 SwapVelocitySlopeLeft()
        {
            if (SlopeLeft)
            {
                return new Vector2(0, 1);
            }
            else if (!IsLefted)
            {
                return new Vector2(-1, 0);
            }
            return new Vector2(0, 0);
        }

        public Vector2 SwapVelocitySlopeRight()
        {
            if (SlopeRight)
            {
                return new Vector2(0, 1);
            }
            else if (!IsRighted)
            {
                return new Vector2(1, 0);
            }
            return new Vector2(0, 0);
        }

        void FixedUpdate()
        {
            ct.NoCollider();
            foreach (Collider2D cd in cols)
            {
                CollisionType ct2 =  new CollisionType(groundLayer);
                ct2.SetBounds(cd.bounds);
                ct.Combine(ct2);
            }
        }
    }
}
