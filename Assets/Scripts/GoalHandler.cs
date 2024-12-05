using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalHandler : MonoBehaviour
{
    public UnityEvent win;

    // Start is called before the first frame update
    void Start()
    {
        win.AddListener(GameObject.FindGameObjectWithTag("level_controller").GetComponent<UIController>().WinMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("plane")) {
            win.Invoke();
            Debug.Log("WIN");
        }
    }
}
