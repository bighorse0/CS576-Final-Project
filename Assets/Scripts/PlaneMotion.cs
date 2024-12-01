using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaneMotion : MonoBehaviour
{
    // Start is called before the first frame update
    public float g;
    public float lift_coefficient;
    public float mass;
    public Vector3 position_0;
    public Vector3 velocity_0;

    private Vector3 position;
    private Vector3 velocity;

    void Start()
    {
        g = 9.81f;
        lift_coefficient = 9.78f;
        position = position_0;
        velocity = velocity_0;
    }

    void UpdateState(float dt)
    {
        Vector3 lift = lift_coefficient * transform.up * ((1 + Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad)) /
                                                          (2 * Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad)));
        Vector3 gravity = mass * g * new Vector3(0, -1.0f, 0);
        Vector3 net_force = lift + gravity;

        position += velocity * dt;
        velocity += (net_force / mass) * dt;

        transform.position = position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateState(Time.deltaTime);
    }
}
