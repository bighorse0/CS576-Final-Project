using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossHandler : MonoBehaviour
{
    public float velocity;
    public float spawn_distance;
    public GameObject plane;
    public bool triggered;
    public AudioClip clip;

    private AudioSource source;
    private bool driving;

    void Start()
    {
        triggered = false;
        source = GetComponent<AudioSource>();  
    }

    void Update()
    {
        if (triggered)
        {
            triggered = false;
            source.PlayOneShot(clip);
            Vector3 position = plane.transform.position;
            position.x = (float)Random.Range(0, 2) * 15.0f - 7.5f;
            position.y = 0.5f;
            position.z += spawn_distance;
            transform.position = position;
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.z > plane.transform.position.z - 20.0f)
        {
            transform.position -= new Vector3(0.0f, 0.0f, Time.deltaTime * velocity);
        }
    }
}
