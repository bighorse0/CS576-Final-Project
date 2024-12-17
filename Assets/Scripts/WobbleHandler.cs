using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleHandler : MonoBehaviour
{
    public float wobble_rate;
    public float wobble_factor;
    private float radius;
    private float width;
    private float time_ellapsed;

    private void Start()
    {
        radius = transform.localScale.x;
        width = transform.localScale.z;
        time_ellapsed = 0.0f;
    }

    void Update()
    {
        Wobble();
        
    }

    private void Wobble()
    {
        transform.localScale = new Vector3(radius + wobble_factor * Mathf.Sin(time_ellapsed * wobble_rate),
                                           radius - wobble_factor * Mathf.Sin(time_ellapsed * wobble_rate), width);
        time_ellapsed += Time.deltaTime;
    }
}
