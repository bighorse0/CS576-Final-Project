using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("plane")) boss.GetComponent<BossHandler>().triggered = true;
    }
}
