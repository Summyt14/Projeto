using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.gameObject.GetComponent<Health>();
            DeathUI deathUI = other.gameObject.GetComponent<DeathUI>();
            health.Hit(health.maxHealth);
            deathUI.DeathScreen();
        }
    }
}
