using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Snowball : MonoBehaviour
{
    public Vector3 direction;
    public float velocity;
    public float birth_time;
    public Vector3 addedSnowballSize; 
    public GameObject frosty;
    public UnityEvent crash;

    // Start is called before the first frame update
    void Start()
    {   
        // Add capability to detect and invoke crash
        crash.AddListener(GameObject.FindGameObjectWithTag("level_controller").GetComponent<GameController>().Fail);
        transform.localScale += addedSnowballSize;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Time.time - birth_time > 10.0f)  // snowballs live for 10 sec
        {
            Destroy(transform.gameObject);
        }*/
        transform.position = transform.position + velocity * direction * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        ////////////////////////////////////////////////
        // (a) if the object collides with plane, invoke crash
        // (b) if the object collides with another snowball, or (frosty), don't do anything
        // (c) if the object collides with anything else (e.g., terrain), destroy the snowball
        ////////////////////////////////////////////////
        // Debug.Log("Snowball OnTriggerEnter: " + other.name);
        // (a)
        if(other.name == "plane 1") {
            crash.Invoke();
        }

        // (b)
        else if(string.Equals("snowball(Clone)", other.name) || string.Equals("present(Clone)", other.name) || string.Equals(other.name, frosty.name)) {
            // Nothing
        }

        // (c)
        else {
            // Debug.Log("Contact!");
            // Destroy(transform.gameObject);
        }

        //Debug.Log(transform.gameObject.name);
    }
}
