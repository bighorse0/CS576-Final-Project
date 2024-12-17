using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RoundaboutHandler : MonoBehaviour
{
    public GameObject center_pole;
    private Vector3 center;
    private Rigidbody RB;
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        RB.mass = 1.0f;
        RB.velocity = new Vector3(0.0f, 0.0f, 35.0f);
        center = center_pole.transform.position;
        center.y = 0.0f;
    }

    void FixedUpdate()
    {
        Vector3 position = transform.position;
        position.y = 0.0f;
        transform.position = position;
        Vector3 radius_vector = center - position;
        RB.AddForce(1225.0f / radius_vector.magnitude * radius_vector.normalized);
        RB.AddForce(new Vector3(0.0f, 1.7f, 0.0f));
        AlignNoseWithVelocity();
    }

    private void AlignNoseWithVelocity()
    {
        Quaternion targetRotation = Quaternion.LookRotation(RB.velocity.normalized, transform.up);
        targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
    }
}
