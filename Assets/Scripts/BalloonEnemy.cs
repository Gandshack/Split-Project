using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class BalloonEnemy : MonoBehaviour
    {

        Enemy ThisEnemy;

        public bool diving = false;

        private void Start()
        {
            ThisEnemy = GetComponent<Enemy>();
        }

        private void Update()
        {
            if (ThisEnemy.PlayerInRange() && ThisEnemy.PlayerBelow())
            {
                Dive();
            }
        }

        private void Dive()
        {
            diving = true;
        }
    }
}
