using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class BubbleHandler : MonoBehaviour
{
    public UnityEvent answeredCorrect;
    public UnityEvent answeredIncorrect;

    private float wobble_speed;
    private float wobble_factor;
    private float radius;
    private float time_ellapsed;

    private int bubble_type;
    [SerializeField] private TextMeshPro bubble_text;

    private void Start() {

        PlaneController plane = GameObject.FindGameObjectWithTag("plane").GetComponent<PlaneController>();

        answeredCorrect.AddListener(delegate{ plane.Boost(); plane.recently_correct = true; });
        answeredIncorrect.AddListener(delegate { plane.Punish(); plane.recently_incorrect = true; });

        wobble_speed = 2.1f;
        wobble_factor = 0.38f;

        radius = transform.localScale.x;

        time_ellapsed = 0.0f;
    }

    private void Update()
    {
        Wobble();    
    }

    public void SetType(int thing) {
        bubble_type = thing;
    }

    public void SetText(string text) {
        bubble_text.text = text;
    }

    private void Wobble()
    {
        time_ellapsed += Time.deltaTime;
        float extend_amount = Mathf.Sin(wobble_speed * time_ellapsed) * wobble_factor; 
        transform.localScale = new Vector3(radius + extend_amount, radius - extend_amount, radius);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("plane")) {
            if (bubble_type == 1) {
                Debug.Log("CORRECT");
                answeredCorrect.Invoke();
            }
            else if (bubble_type == 2) {
                Debug.Log("INCORRECT");
                answeredIncorrect.Invoke();
            }
            else if (bubble_type == 0) {
                Debug.Log("PROBLEM");
            }

            /*GameObject[] currently_loaded_bubbles = GameObject.FindGameObjectsWithTag("bubble");
            foreach (GameObject loaded_bubble in currently_loaded_bubbles) {
                UnityEngine.Object.Destroy(loaded_bubble);
            }*/
        }
    }
}
 