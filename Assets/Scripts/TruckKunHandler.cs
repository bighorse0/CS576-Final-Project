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
    public AudioClip clip;

    private AudioSource source;
    private float distance_traveled;
    private bool audio_played;

    void Start()
    {
        triggered = false;
        source = GetComponent<AudioSource>();
        distance_traveled = 0.0f;
        audio_played = false;
    }

    void FixedUpdate()
    {
        if (triggered)
        {
            
            drive();
            if (!audio_played && clip != null)
            {
                source.PlayOneShot(clip);
                audio_played = true;
            }
        }
    }

    private void drive()
    {
        if (distance_traveled > 500.0f) return;
        transform.position += new Vector3(Time.deltaTime * velocity, 0.0f, 0.0f);
        distance_traveled += Time.deltaTime * velocity;
    }
}
