using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    class Checkpoint : MonoBehaviour
    {
        public bool activated;
        public Transform basePos;

        public void Activate()
        {
            activated = true;
            DataService.Instance.SaveAtGroundPos(basePos.position);
        }
    }
}
