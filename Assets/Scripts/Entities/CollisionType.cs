using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CollisionType
    {
        public bool isGrounded = false;
        public bool isLefted = false;
        public bool isRighted = false;
        public bool isCeiled = false;

        public bool SlopeLeft = false;
        public bool SlopeRight = false;

        public bool EdgeLeft = false;
        public bool EdgeRight = false;

        public LayerMask groundLayer;

        public CollisionType(LayerMask groundLayer)
        {
            isGrounded = false;
            isLefted = false;
            isRighted = false;
            isCeiled = false;

            SlopeLeft = false;
            SlopeRight = false;

            EdgeLeft = false;
            EdgeRight = false;
            this.groundLayer = groundLayer;
        }

        public RaycastHit2D TouchingInDir(Bounds bounds, Vector2 dir)
        {
            RaycastHit2D hit = Physics2D.BoxCast(bounds.center + new Vector3(0.05f * dir.x, 0.05f * dir.y, 0),
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

        public bool IsSlopeLeft(Bounds bounds)
        {
            Vector2 pos = bounds.center - bounds.extents;
            RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(-1, 0), 0.05f, groundLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(pos + new Vector2(0, 0.1f), new Vector2(-1, 0), 0.05f, groundLayer);
            //RaycastHit2D hit3 = Physics2D.Raycast(pos + new Vector2(0, 0.1f), new Vector2(-1, 0), 0.1f, groundLayer);
            return hit && !hit2;// && hit3;
        }
        public bool IsSlopeRight(Bounds bounds)
        {
            Vector2 pos = bounds.center + bounds.extents;
            RaycastHit2D hit = Physics2D.Raycast(pos - new Vector2(0, 2 * bounds.extents.y), new Vector2(1, 0), 0.05f, groundLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(pos - new Vector2(0, 0.1f), new Vector2(1, 0), 0.05f, groundLayer);
            //RaycastHit2D hit3 = Physics2D.Raycast(pos - new Vector2(0, 0.1f), new Vector2(1, 0), 0.1f, groundLayer);
            return hit && !hit2;// && hit3;
        }

        public bool IsEdgeLeft(Bounds bounds)
        {
            Vector2 pos = bounds.center - bounds.extents;
            RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(0, -1), 0.05f, groundLayer);
            return !hit;
        }

        public bool IsEdgeRight(Bounds bounds)
        {
            Vector2 pos = bounds.center - bounds.extents;
            RaycastHit2D hit = Physics2D.Raycast(pos + new Vector2(2 * bounds.extents.x, 0), new Vector2(0, -1), 0.05f, groundLayer);
            return !hit;
        }

        public void NoCollider()
        {
            isGrounded = false;
            isLefted = false;
            isRighted = false;
            isCeiled = false;

            SlopeLeft = false;
            SlopeRight = false;

            EdgeLeft = false;
            EdgeRight = false;
        }

        public void Combine(CollisionType ct)
        {
            isGrounded = isGrounded || ct.isGrounded;
            isLefted = isLefted || ct.isLefted;
            isRighted = isRighted || ct.isRighted;
            isCeiled = isCeiled || ct.isCeiled;

            SlopeLeft = SlopeLeft || ct.SlopeLeft;
            SlopeRight = SlopeRight || ct.SlopeRight;

            EdgeLeft = EdgeLeft || ct.EdgeLeft;
            EdgeRight = EdgeRight || ct.EdgeRight;
        }

        public void SetBounds(Bounds bounds)
        {
            isGrounded = TouchingInDir(bounds, Vector2.down);
            isLefted = TouchingInDir(bounds, Vector2.left);
            isRighted = TouchingInDir(bounds, Vector2.right);
            isCeiled = TouchingInDir(bounds, Vector2.up);
            if (!isGrounded && !isLefted)
            {
                RaycastHit2D r = TouchingInDir(bounds,  new Vector2(-1, -1));
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
                RaycastHit2D r = TouchingInDir(bounds, new Vector2(1, -1));
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
                RaycastHit2D r = TouchingInDir(bounds, new Vector2(-1, 1));
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
                RaycastHit2D r = TouchingInDir(bounds, new Vector2(1, 1));
                if (r)
                {
                    if (r.normal.x != 0)
                    {
                        isRighted = true;
                    }
                    if (r.normal.y != 0)
                    {
                        isCeiled = true;
                    }
                }
            }
            SlopeLeft = IsSlopeLeft(bounds);
            SlopeRight = IsSlopeRight(bounds);
            EdgeLeft = IsEdgeLeft(bounds);
            EdgeRight = IsEdgeRight(bounds);
        }
    }
}
