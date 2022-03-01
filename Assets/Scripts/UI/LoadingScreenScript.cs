using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneWithLoadingScreen());
    }

    public IEnumerator LoadSceneWithLoadingScreen()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        yield return new WaitForSeconds(0.25f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(DataService.Instance.SceneToLoad);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeQuit()
    {
        Application.Quit();
    }

    public void ClearSave()
    {
        DataService.Instance.ClearSave();
    }
}
