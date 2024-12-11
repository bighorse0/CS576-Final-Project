using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEditor.Rendering;

public class PlaneController : MonoBehaviour
{
    public float horizontal_rotation_sens;
    public float vertical_rotation_sens;
    public float gravitational_acc;
    public float boost_amt;
    public float punish_amt;
    public Vector3 base_velocity;

    private float horizontal_input;
    private float vertical_input;
    private float bound_angle_z; 
    private float bound_angle_x;
    private float roll_anglur_velocity;
    private Rigidbody RB;
    [SerializeField] private GameObject velocity_text;

    void Start()
    {
        horizontal_input = 0.0f;
        vertical_input   = 0.0f;

        bound_angle_z = 30.0f;
        bound_angle_x = 30.0f;

        roll_anglur_velocity = 0.0f;

        RB = GetComponent<Rigidbody>();
        RB.velocity = base_velocity;
        Physics.gravity = new Vector3(0.0f, -gravitational_acc, 0.0f);
    }

    void Update()
    {
        horizontal_input = Input.GetAxisRaw("Horizontal");
          vertical_input = Input.GetAxisRaw("Vertical");

        FixedCameraAngle();
    }

    void FixedUpdate()
    {
        velocity_text.GetComponent<Text>().text = RB.velocity.magnitude.ToString() + "mph";
        
        float v = RB.velocity.magnitude;
        float V = 0.5f * v * v;
        float U = gravitational_acc * transform.position.y;
        Debug.Log("Mechanical Energy: " + V + U);

        UpdateRoll();
        ApplyForce();
        AlignNoseWithVelocity();
    }

    private void UpdateRoll()
    {
        Vector3 angles = transform.eulerAngles;
        angles.z -= 1.25f * horizontal_input;

        if (horizontal_input == 0 && angles.z > 0)
        {
            float targetAngle = angles.z > 180.0f ? 360.0f : 0.0f;
            angles.z = Mathf.SmoothDampAngle(angles.z, targetAngle, ref roll_anglur_velocity, 0.3f);
        }

        if (bound_angle_z < angles.z && angles.z < 180.0f)
        {
            angles.z = bound_angle_z;
        }
        if (180.0f <= angles.z && angles.z < 360.0f - bound_angle_z)
        {
            angles.z = 360.0f - bound_angle_z;
        }

        transform.eulerAngles = angles;
    }

    private void FixedCameraAngle()
    {
        Vector3 angles = Camera.main.transform.eulerAngles;
        angles.z = 0.0f;
        Camera.main.transform.eulerAngles = angles;
    }
    private void ApplyForce()
    {

        Vector3 horizontal_force = horizontal_input * horizontal_rotation_sens * transform.right;
        Vector3 lift = vertical_rotation_sens * transform.up;
        Vector3 drag = -1.25f * Mathf.Abs(Mathf.Sin(transform.eulerAngles.x)) * transform.forward;

        float lift_factor  = vertical_input;
        float norm_angle_x = transform.eulerAngles.x - (transform.eulerAngles.x > 180.0f ? 360.0f : 0.0f);

        if (bound_angle_x <= norm_angle_x)
        {
            lift_factor = Mathf.Max(vertical_input, 0.0f);
        }
        else if (norm_angle_x <= -bound_angle_x)
        {
            lift_factor = Mathf.Min(vertical_input, 0.0f);
        }

        lift *= lift_factor;

        Debug.Log(transform.eulerAngles.x);

        Vector3 net_force = horizontal_force + lift + drag;

        RB.AddForce(net_force);
    }

    private void AlignNoseWithVelocity()
    {
        if (RB.velocity.magnitude > 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(RB.velocity.normalized, transform.up);
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
        }
    }

    public void Boost()
    {
        Vector3 boost_velocity = boost_amt * RB.velocity.normalized;
        RB.velocity += boost_velocity;
    }

    public void Punish()
    {
        Vector3 punish_velocity = Mathf.Min(punish_amt, RB.velocity.magnitude) * RB.velocity.normalized;
        RB.velocity -= punish_velocity;
    }
}
