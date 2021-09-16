using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    public GameObject loadingInterface;
    public Image loadingBar;
    public GameObject startMenu;

    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    
    public void Play()
    {
        startMenu.SetActive(false);
        Load();
    }

    private void Load()
    {
        loadingInterface.SetActive(true);
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Map"));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Level_1", LoadSceneMode.Additive));
        StartCoroutine(LoadingScreen());
    }

    IEnumerator LoadingScreen()
    {
        float progress = 0;
        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            while (!scenesToLoad[i].isDone)
            {
                progress += scenesToLoad[i].progress;
                loadingBar.fillAmount = progress / scenesToLoad.Count;
                yield return null;
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
