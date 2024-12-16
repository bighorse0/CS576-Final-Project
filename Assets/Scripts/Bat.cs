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

    private PlaneController plane_controller;

    // Start is called before the first frame update
    void Start()
    {
        plane_controller = GameObject.FindGameObjectWithTag("plane").GetComponent<PlaneController>();
        bat_hit.AddListener(plane_controller.Punish);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - birth_time > 10.0f)
        {
            Destroy(transform.gameObject);
        }
        transform.position = transform.position + velocity * direction * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("plane")) {
            Debug.Log("BAT HIT PLANE");
            Destroy(gameObject);
            bat_hit.Invoke();
        }
        else if (other.gameObject.name.Contains("bat")) {

        }
        else {
            Destroy(gameObject);
        }
    }
}
