using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DoorLevelSwitch : MonoBehaviour
{
    public Transform teleport;
    public GameObject player;
    
    public GameObject loadingInterface;
    public Image loadingBar;
    public GameObject activeUI;
    public GameObject winUI;
    public Minimap minimap;

    public String sceneToUnload;
    
    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    private bool loading;

    public GameObject interactUI;
    public bool winDoor;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!interactUI.activeSelf)
            {
                interactUI.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E) && !loading)
            {
                if (winDoor)
                {
                    loading = true;
                    minimap.resetAfterLoad();
                    winUI.SetActive(true);
                    activeUI.SetActive(false);
                }
                else
                {
                    loading = true;
                    minimap.resetAfterLoad();
                    loadingInterface.SetActive(true);
                    activeUI.SetActive(false);
                    Load();   
                }
            }   
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (interactUI.activeSelf)
        {
            interactUI.SetActive(false);
        }
    }

    private void Load()
    {
        scenesToLoad.Add(SceneManager.UnloadSceneAsync(sceneToUnload));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive));
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

        if (scenesToLoad[1].isDone)
        {
            player.transform.position = teleport.transform.position;
        
            loadingInterface.SetActive(false);
            activeUI.SetActive(true);   
            
            scenesToLoad = new List<AsyncOperation>();
            
            loading = false;
        }
    }
    
}
