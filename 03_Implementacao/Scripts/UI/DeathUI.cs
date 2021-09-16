using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    public GameObject deathUI;
    public Health playerH;

    private bool active;
    private void Update()
    {
        if (playerH.isDead && !active)
        {
            active = true;
            if (!deathUI.activeSelf)
            {
                StartCoroutine(DelayedDeath());   
            }
        }
    }
    
    public void DeathScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        deathUI.SetActive(true);
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(2);
        DeathScreen();
    }
    
    
}
