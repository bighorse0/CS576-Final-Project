using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MinionProjectile : MonoBehaviour
{
    public Vector3 direction;
    public float velocity;
    public float birth_time;

    public UnityEvent projectile_hit;

    public AudioClip projectile_travel;
    public AudioClip projectile_hit_sound;

    private AudioSource source;

    private PlaneController plane_controller;
    

    void Start()
    {
        plane_controller = GameObject.FindGameObjectWithTag("plane").GetComponent<PlaneController>();
        projectile_hit.AddListener(plane_controller.Punish);
        source = GetComponent<AudioSource>();
        StartCoroutine(Wobble());

    }

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
            source.PlayOneShot(projectile_hit_sound);
            projectile_hit.Invoke();
        }
        else if (other.gameObject.name.Contains("projectile")) {

        }
        else {
            Destroy(gameObject);
        }
    }

    IEnumerator Wobble() {
        float clip_len = projectile_travel.length;
        while (true) {
            source.PlayOneShot(projectile_travel);
            yield return new WaitForSeconds(clip_len);
        }
    }
}

