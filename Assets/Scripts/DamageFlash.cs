using Assets.Scripts.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class DamageFlash : MonoBehaviour
    {

        private SpriteRenderer rend;
        private PeriodicEvent per;

        void Start()
        {
            rend = GetComponent<SpriteRenderer>();
            per = new PeriodicEvent(0.1f, 0.4f, 0.5f);
        }

        void Update()
        {
            if (per.Tick(Time.deltaTime))
            {
                rend.color = Color.red;
            }
            else
            {
                rend.color = Color.white;
            }
        }

        public void Do()
        {
            per.Start();
        }
    }
}
