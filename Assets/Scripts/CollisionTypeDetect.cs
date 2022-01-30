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
        public bool isGrounded = false;
        public bool isLefted = false;
        public bool isRighted = false;

        public LayerMask groundLayer;

        public bool SlopeLeft = false;
        public bool SlopeRight = false;

        public bool EdgeLeft = false;
        public bool EdgeRight = false;

        private Collider2D col;

        void Start()
        {
            col = GetComponent<BoxCollider2D>();
        }

        bool OnGround()
        {
            Bounds bounds = col.bounds;
            Vector2 pos = bounds.center - bounds.extents;
            RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(0, -1), 0.05f, groundLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(pos + new Vector2(2 * bounds.extents.x, 0), new Vector2(0, -1), 0.05f, groundLayer);
            return hit || hit2;
        }

        bool TouchingInDir(Vector2 dir)
        {
            Bounds bounds = col.bounds;
            RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, dir, 0.05f, groundLayer);
            return hit;
        }

        bool IsSlopeLeft()
        {
            Bounds bounds = col.bounds;
            Vector2 pos = bounds.center - bounds.extents;
            RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(-1, 0), 0.05f, groundLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(pos + new Vector2(0, bounds.extents.y), new Vector2(-1, 0), 0.05f, groundLayer);
            return hit && !hit2;
        }
        bool IsSlopeRight()
        {
            Bounds bounds = col.bounds;
            Vector2 pos = bounds.center + bounds.extents;
            RaycastHit2D hit = Physics2D.Raycast(pos - new Vector2(0, 2 * bounds.extents.y), new Vector2(1, 0), 0.05f, groundLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(pos - new Vector2(0, bounds.extents.y), new Vector2(1, 0), 0.05f, groundLayer);
            return hit && !hit2;
        }

        bool IsEdgeLeft()
        {
            Bounds bounds = col.bounds;
            Vector2 pos = bounds.center - bounds.extents;
            RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(0, -1), 0.05f, groundLayer);
            return !hit;
        }

        bool IsEdgeRight()
        {
            Bounds bounds = col.bounds;
            Vector2 pos = bounds.center - bounds.extents;
            RaycastHit2D hit = Physics2D.Raycast(pos + new Vector2(2*bounds.extents.x, 0), new Vector2(0, -1), 0.05f, groundLayer);
            return !hit;
        }

        void FixedUpdate()
        {
            isGrounded = TouchingInDir(Vector2.down);
            isLefted = TouchingInDir(Vector2.left);
            isRighted = TouchingInDir(Vector2.right);
            SlopeLeft = IsSlopeLeft();
            SlopeRight = IsSlopeRight();
            EdgeLeft = IsEdgeLeft();
            EdgeRight = IsEdgeRight();
        }
    }
}
