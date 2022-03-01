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

        public void Start()
        {
            activated = false;
            transform.Find("ActivatedGlow").gameObject.SetActive(false);
        }

        public void Activate()
        {
            activated = true;
            transform.Find("ActivatedGlow").gameObject.SetActive(true);
            DataService.Instance.SaveAtGroundPos(basePos.position);
        }

        public string GetUniqueName()
        {
            return basePos.position.ToString("F");
        }
    }
}
