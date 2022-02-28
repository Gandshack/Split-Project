using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    class CheckpointActivator : MonoBehaviour
    {
        public Checkpoint cp;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerMovement>())
            {
                cp.Activate();
            }
        }
    }
}
