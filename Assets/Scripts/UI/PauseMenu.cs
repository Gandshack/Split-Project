using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class PauseMenu : MonoBehaviour
    {
        private bool paused;
        private Transform menu;

        void Start()
        {
            paused = false;
            menu = transform.Find("Menu");
        }

        public void Resume()
        {
            Time.timeScale = 1;
            menu.gameObject.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (paused)
                {
                    Time.timeScale = 1;
                    paused = false;
                    menu.gameObject.SetActive(false);
                }
                else
                {
                    Time.timeScale = 0;
                    paused = true;
                    menu.gameObject.SetActive(true);
                }
            }
        }
    }
}
