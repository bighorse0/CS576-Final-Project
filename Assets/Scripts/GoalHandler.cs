using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalHandler : MonoBehaviour
{
    public UnityEvent win;

    [SerializeField] private int level_number;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        win.AddListener(GameObject.FindGameObjectWithTag("level_controller").GetComponent<GameController>().Win);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("plane")) {
            win.Invoke();
            Time.timeScale = 0.0f;
        }
    }
}
