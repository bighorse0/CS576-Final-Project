using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundHandler : MonoBehaviour
{
    public UnityEvent crash;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        crash.AddListener(GameObject.FindGameObjectWithTag("level_controller").GetComponent<GameController>().Fail);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("plane")) {
            crash.Invoke();
            Time.timeScale = 0.0f;
        }
        else if (other.gameObject.name.Contains("snowball")) {
            Destroy(other.transform.gameObject);
        }
    }
}
