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
    public bool recently_correct;
    public bool recently_incorrect;
    public Vector3 base_velocity;
    public Vector3 movement_direction;

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
    private bool in_animation;
    public Rigidbody RB;
    private GameObject velocity_text;
    private GameObject stalling_text;
    //[SerializeField] private GameObject plane_body;

    void Start()
    {
        horizontal_input = 0.0f;
        vertical_input   = 0.0f;

        bound_angle_z = 30.0f;
        bound_angle_x = 30.0f;
        roll_angular_velocity = 0.0f;

        base_fov = 60.0f;
        max_fov = 100f;
        min_fov = 30.0f;
        current_fov_rate = 0.0f;

        drag_factor = 0.625f;

        stall_threshold = 2.0f;
        recovery_thresold = 5.0f;
        currently_stalling = false;
        in_animation = false;

        recently_correct = false;
        recently_incorrect = false;

        RB = GetComponent<Rigidbody>();
        RB.velocity = base_velocity;
        Physics.gravity = new Vector3(0.0f, -gravitational_acc, 0.0f);

        velocity_text = GameObject.FindGameObjectWithTag("velocity_text");
        stalling_text = GameObject.FindGameObjectWithTag("stalling_text");

        stalling_text.GetComponent<Text>().text = "";
    }

    void Update()
    {
        horizontal_input = Input.GetAxisRaw("Horizontal");
          vertical_input = Input.GetAxisRaw("Vertical");

        UpdateCamera();

        // for use in bat npc
        float xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        float ydirection = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.x);
        float zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        movement_direction = new Vector3(xdirection, ydirection, zdirection);

        // Disabled animations because they're too buggy
        //if (recently_correct && !in_animation) StartCoroutine(AileronRoll());
        //if (recently_incorrect && !in_animation) StartCoroutine(Turbulence());
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
    // Functions for Update
    private void UpdateCamera()
    {
        // Fix the roll (angle about the z_axis) of the camera
        Vector3 angles   = Camera.main.transform.eulerAngles;
        Vector3 position = Camera.main.transform.position;
        angles.z = 0.0f;
        Camera.main.transform.eulerAngles = angles;

        // Dynamic FOV based on velocity
        float target_fov = (base_fov - min_fov) * (RB.velocity.magnitude / base_velocity.magnitude) + min_fov;
        target_fov = Mathf.Clamp(target_fov, min_fov, max_fov);
        Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, target_fov, ref current_fov_rate, 0.5f);
    }

    //private IEnumerator AileronRoll()
    //{
    //    recently_correct = false;
    //    in_animation = true;
    //    float angle_rotated = 0.0f;
    //    float roll_direction = (float)(Random.Range(0, 2) * 2 - 1);
    //    while (angle_rotated < 360.0f)
    //    {
    //        float delta = 180.0f * Time.deltaTime;
    //        plane_body.transform.Rotate(0.0f, 0.0f, delta);
    //        angle_rotated += delta;
    //        yield return null;
    //    }

    //    plane_body.transform.eulerAngles = Vector3.zero;
    //    in_animation = false;
    //}
    //private IEnumerator Turbulence()
    //{
    //    recently_incorrect = false;
    //    in_animation= true;
    //    float time_elapsed = 0.0f;
    //    while (time_elapsed < 1.5f)
    //    {
    //        float delta = 200.0f * Time.deltaTime;
    //        plane_body.transform.Rotate(0.0f, 0.0f, (float)(Random.Range(0, 2) * 2 - 1) * delta);
    //        time_elapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //    plane_body.transform.eulerAngles = Vector3.zero;
    //    in_animation = false;
    //}


    // Functions for FixedUpdate
    private void UpdateStallingStatus()
    {
        currently_stalling = currently_stalling ? RB.velocity.magnitude < recovery_thresold
                                                : RB.velocity.magnitude < stall_threshold;
    }

    private void UpdateVelocityUI()
    {
        velocity_text.GetComponent<Text>().text = currently_stalling ? ""
                                                : RB.velocity.magnitude.ToString() + "mph";
        stalling_text.GetComponent<Text>().text = currently_stalling ? "Stalling! Trying to pick up speed: " + RB.velocity.magnitude.ToString() + "mph" : "";
    }

    private void UpdateRoll()
    {
        if (in_animation) return;

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
        if (in_animation) return;

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
            Quaternion target_rotation = Quaternion.LookRotation(RB.velocity.normalized, transform.up);
            target_rotation = Quaternion.Euler(target_rotation.eulerAngles.x, target_rotation.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, target_rotation, Time.deltaTime * 2.0f);
        }
    }

    private void PrintMechanicalEnergy()
    {
        float V = 0.5f * Mathf.Pow(RB.velocity.magnitude, 2.0f);
        float U = gravitational_acc * transform.position.y;
        //Debug.Log("Mechanical Energy: " + V + U);
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
