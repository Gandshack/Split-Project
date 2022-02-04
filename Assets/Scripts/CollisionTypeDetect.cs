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
        public bool isCeiled = false;

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

        bool BelowCeiling()
        {
            Bounds bounds = col.bounds;
            Vector2 pos = bounds.center - bounds.extents + new Vector3(0, 2 * bounds.extents.y, 0);
            RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(0, 1), 0.05f, groundLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(pos + new Vector2(2 * bounds.extents.x, 0), new Vector2(0, 1), 0.05f, groundLayer);
            return hit || hit2;
        }

        RaycastHit2D TouchingInDir(Vector2 dir)
        {
            Bounds bounds = col.bounds;
            RaycastHit2D hit = Physics2D.BoxCast(bounds.center + new Vector3(0.05f*dir.x, 0.05f*dir.y, 0),
                                                 bounds.size - new Vector3(0.1f, 0.1f, 0), 0, dir, 0.05f, groundLayer);
            return hit;
        }

        /*private void setCollisions(Collision2D collision, bool val)
        {
            if (groundLayer == (groundLayer | (1 << collision.gameObject.layer)))
            {
                Debug.Log(gameObject.name);
                Debug.Log(collision.gameObject.name);
                Debug.Log(val);
                List<ContactPoint2D> L = new List<ContactPoint2D>();
                collision.GetContacts(L);
                foreach (ContactPoint2D p in L)
                {
                    Debug.Log("collision!!!!!!!!!!");
                    float c = Vector2.Dot(p.normal.normalized, Vector2.right);
                    if (c > 0.5)
                    {
                        isLefted = val;
                    }
                    c = Vector2.Dot(p.normal.normalized, Vector2.left);
                    if (c > 0.5)
                    {
                        isRighted = val;
                    }
                    c = Vector2.Dot(p.normal.normalized, Vector2.up);
                    if (c > 0.5)
                    {
                        isGrounded = val;
                    }
                    c = Vector2.Dot(p.normal.normalized, Vector2.down);
                    if (c > 0.5)
                    {
                        isCeiled = val;
                    }
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            setCollisions(collision, true);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            setCollisions(collision, false);
        }*/

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
            RaycastHit2D hit = Physics2D.Raycast(pos + new Vector2(2 * bounds.extents.x, 0), new Vector2(0, -1), 0.05f, groundLayer);
            return !hit;
        }

        void FixedUpdate()
        {
            isGrounded = TouchingInDir(Vector2.down);
            isLefted = TouchingInDir(Vector2.left);
            isRighted = TouchingInDir(Vector2.right);
            isCeiled = TouchingInDir(Vector2.up);
            if (!isGrounded && !isLefted)
            {
                RaycastHit2D r = TouchingInDir(new Vector2(-1, -1));
                if (r)
                {
                    if (r.normal.x != 0)
                    {
                        isLefted = true;
                    }
                    if (r.normal.y != 0)
                    {
                        isGrounded = true;
                    }
                }
            }
            if (!isGrounded && !isRighted)
            {
                RaycastHit2D r = TouchingInDir(new Vector2(1, -1));
                if (r)
                {
                    if (r.normal.x != 0)
                    {
                        isRighted = true;
                    }
                    if (r.normal.y != 0)
                    {
                        isGrounded = true;
                    }
                }
            }
            if (!isCeiled && !isLefted)
            {
                RaycastHit2D r = TouchingInDir(new Vector2(-1, 1));
                if (r)
                {
                    if (r.normal.x != 0)
                    {
                        isLefted = true;
                    }
                    if (r.normal.y != 0)
                    {
                        isCeiled = true;
                    }
                }
            }
            if (!isCeiled && !isRighted)
            {
                RaycastHit2D r = TouchingInDir(new Vector2(1, 1));
                if (r)
                {
                    if (r.normal.x != 0)
                    {
                        isRighted = true;
                    }
                    if (r.normal.y !=0)
                    {
                        isCeiled = true;
                    }
                }
            }
            SlopeLeft = IsSlopeLeft();
            SlopeRight = IsSlopeRight();
            EdgeLeft = IsEdgeLeft();
            EdgeRight = IsEdgeRight();
        }
    }
}
