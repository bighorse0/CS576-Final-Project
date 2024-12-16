using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Bat : MonoBehaviour
{
    public Vector3 direction;
    public float velocity;
    public float birth_time;
    public UnityEvent bat_hit;

    public AudioClip bat_fly;
    public AudioClip bat_hit_sound;

    private AudioSource source;

    private PlaneController plane_controller;

    // Start is called before the first frame update
    void Start()
    {
        plane_controller = GameObject.FindGameObjectWithTag("plane").GetComponent<PlaneController>();
        bat_hit.AddListener(plane_controller.Punish);
        source = GetComponent<AudioSource>();
        StartCoroutine(Wings());
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - birth_time > 5.0f)
        {
            Destroy(transform.gameObject);
        }
        transform.position = transform.position + velocity * direction * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("plane")) {
            Debug.Log("BAT HIT PLANE");
            source.PlayOneShot(bat_hit_sound);
            bat_hit.Invoke();
        }
        else if (other.gameObject.name.Contains("bat")) {

        }
        else {
            Destroy(gameObject);
        }
    }

    IEnumerator Wings() {
        float clip_len = bat_fly.length;
        while (true) {
            source.PlayOneShot(bat_fly);
            yield return new WaitForSeconds(clip_len);
        }
    }
}
