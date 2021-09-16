using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CameraZone : MonoBehaviour
{
    [SerializeField] private GameObject virtualCamera;

    private void Start()
    {
        virtualCamera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            virtualCamera.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            virtualCamera.SetActive(false);
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}
