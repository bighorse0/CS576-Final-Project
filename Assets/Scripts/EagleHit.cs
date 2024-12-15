using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleHit : MonoBehaviour
{
    public AudioClip eagle_hit;

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("plane")) {
            Debug.Log("HIT");
            source.PlayOneShot(eagle_hit);

            // TODO: HANDLE PLANE VELOCITY DECREASE ON HIT
        }
    }
}
