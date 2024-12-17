using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class TruckKunHandler : MonoBehaviour
{
    public bool triggered;
    public float velocity;
    private float distance_traveled;

    void Start()
    {
        triggered = false;
        distance_traveled = 0.0f;
    }

    void FixedUpdate()
    {
        if (triggered) drive();
        
    }

    private void drive()
    {
        if (distance_traveled > 500.0f) return;
        transform.position += new Vector3(Time.deltaTime * velocity, 0.0f, 0.0f);
        distance_traveled += Time.deltaTime * velocity;
    }
}
