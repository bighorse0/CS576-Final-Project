using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BubbleHandler : MonoBehaviour
{
    private int bubble_type;
    [SerializeField] private TextMeshPro bubble_text;

    public void SetType(int thing) {
        bubble_type = thing;
    }

    public void SetText(string text) {
        bubble_text.text = text;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("plane")) {
            if (bubble_type == 1) {
                Debug.Log("CORRECT");
            }
            else if (bubble_type == 2) {
                Debug.Log("INCORRECT");
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