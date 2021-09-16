using UnityEngine;

public class Location : MonoBehaviour
{
    [SerializeField] private Transform[] locationSpots;

    public Transform[] GetLocationSpots()
    {
        return locationSpots;
    }
}