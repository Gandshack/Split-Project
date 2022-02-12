using Assets.Scripts.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    class PauseMenu : MonoBehaviour
    {
        private bool paused;
        private Transform menu;

        void Start()
        {
            menu = transform.Find("Menu");
            Time.timeScale = 1;
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

        public void Load()
        {
            SceneManager.LoadScene("Test level");
        }

        public void Save()
        {
            DataService.Instance.Save();
        }
        public void ClearSave()
        {
            DataService.Instance.ClearSave();
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
