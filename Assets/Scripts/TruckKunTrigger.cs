using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckKunTrigger : MonoBehaviour
{
    public GameObject cars;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("plane")) cars.GetComponent<TruckKunHandler>().triggered = true;
    }
}
