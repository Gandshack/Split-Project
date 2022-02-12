using Assets.Scripts.Saving;
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
            menu = transform.Find("Menu");
            paused = false;
            menu.gameObject.SetActive(false);
        }

        public void Resume()
        {
            Time.timeScale = 1;
            paused = false;
            menu.gameObject.SetActive(false);
        }

        public void Pause()
        {
            Time.timeScale = 0;
            paused = true;
            menu.gameObject.SetActive(true);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)||Input.GetKeyDown(KeyCode.Escape))
            {
                if (paused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
            else if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                DataService.Instance.Save();
            }
            else if (Input.GetKeyDown(KeyCode.Quote))
            {
                DataService.Instance.Load();
            }
            else if (Input.GetKeyDown(KeyCode.Minus))
            {
                DataService.Instance.ClearSave();
            }
        }

    }
}
