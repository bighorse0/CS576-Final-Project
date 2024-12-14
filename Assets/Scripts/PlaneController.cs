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
    private float roll_angular_velocity;
    private float base_fov;
    private float max_fov;
    private float min_fov; 
    private float current_fov_rate;
    private float drag_factor;
    private float stall_threshold;
    private float recovery_thresold;
    private bool currently_stalling;
    private Rigidbody RB;
    private GameObject velocity_text;
    private GameObject stalling_text;

    void Start()
    {
        horizontal_input = 0.0f;
        vertical_input   = 0.0f;

        bound_angle_z = 30.0f;
        bound_angle_x = 30.0f;
        roll_angular_velocity = 0.0f;

        base_fov = 60.0f;
        max_fov = 120.0f;
        min_fov = 30.0f;
        current_fov_rate = 0.0f;

        drag_factor = 0.625f;

        currently_stalling = false;
        stall_threshold = 2.0f;
        recovery_thresold = 5.0f;

        RB = GetComponent<Rigidbody>();
        RB.velocity = base_velocity;
        Physics.gravity = new Vector3(0.0f, -gravitational_acc, 0.0f);

        velocity_text = GameObject.FindGameObjectWithTag("velocity_text");
        stalling_text = GameObject.FindGameObjectWithTag("stalling_text");
    }

    void Update()
    {
        horizontal_input = Input.GetAxisRaw("Horizontal");
          vertical_input = Input.GetAxisRaw("Vertical");

        UpdateCamera();
    }

    void FixedUpdate()
    {
        UpdateStallingStatus();
        UpdateVelocityUI();
        UpdateRoll();
        ApplyForce();
        AlignNoseWithVelocity();
        PrintMechanicalEnergy();
    }

    private void UpdateCamera()
    {
        Vector3 angles   = Camera.main.transform.eulerAngles;
        Vector3 position = Camera.main.transform.position;
        angles.z = 0.0f;
        Camera.main.transform.eulerAngles = angles;

        float target_fov = (base_fov - min_fov) * (RB.velocity.magnitude / 10.0f) + min_fov;
        target_fov = Mathf.Clamp(target_fov, min_fov, max_fov);
        Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, target_fov, ref current_fov_rate, 0.5f);
    }

    private void UpdateStallingStatus()
    {
        currently_stalling = currently_stalling ? RB.velocity.magnitude < recovery_thresold
                                                : RB.velocity.magnitude < stall_threshold;
    }

    private void UpdateVelocityUI()
    {
        velocity_text.GetComponent<Text>().text = currently_stalling ? "" : RB.velocity.magnitude.ToString() + "mph";
        stalling_text.GetComponent<Text>().text = currently_stalling ? "Stalling! Trying to pick up speed: " + RB.velocity.magnitude.ToString() + "mph" : "";
    }

    private void UpdateRoll()
    {
        if (currently_stalling) return;

        Vector3 angles = transform.eulerAngles;
        angles.z -= 1.25f * horizontal_input;

        if (horizontal_input == 0 && angles.z > 0)
        {
            float targetAngle = angles.z > 180.0f ? 360.0f : 0.0f;
            angles.z = Mathf.SmoothDampAngle(angles.z, targetAngle, ref roll_angular_velocity, 0.3f);
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

    private void ApplyForce()
    {
        float horizontal_factor = horizontal_input;
        float lift_factor  = vertical_input;
        float pitch_angle = (transform.eulerAngles.x > 180.0f ? 360.0f : 0.0f) - transform.eulerAngles.x;

        // If pitch is too low, prevent the player from pitching the plane downward
        if (pitch_angle <= -bound_angle_x)
        {
            lift_factor = Mathf.Max(vertical_input, 0.0f);
        }

        // Similarly for high pitch
        else if (bound_angle_x <= pitch_angle)
        {
            lift_factor = Mathf.Min(vertical_input, 0.0f);
        }

        if (currently_stalling)
        {
            horizontal_factor = 0.0f;
            lift_factor = 0.0f;
        }

        Vector3 horizontal_force = horizontal_input * horizontal_rotation_sens * transform.right;
        Vector3 lift = lift_factor * vertical_rotation_sens * transform.up;
        Vector3 drag = -drag_factor * gravitational_acc * Mathf.Abs(Mathf.Sin(transform.eulerAngles.x)) * transform.forward;
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

    private void PrintMechanicalEnergy()
    {
        float V = 0.5f * Mathf.Pow(RB.velocity.magnitude, 2.0f);
        float U = gravitational_acc * transform.position.y;
        Debug.Log("Mechanical Energy: " + V + U);
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
