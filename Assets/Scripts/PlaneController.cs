using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float horizontal_rotation_sens;
    public float vertical_rotation_sens;

    private float bound_angle_z;
    private float bound_angle_x;
    private float camera_height;
    private float camera_offset;

    void Start()
    {
        bound_angle_z = 60.0f;
        bound_angle_x = 45.0f;

        camera_height = 0.5f;
        camera_offset = 2.5f;
    }

    void Update()
    {
        HandleKeysEvents();
        UpdateCameraPosition();
    }

    private void HandleKeysEvents()
    {
        float new_angle_z = transform.eulerAngles.z - horizontal_rotation_sens * Input.GetAxis("Horizontal");
        float new_angle_x = transform.eulerAngles.x -   vertical_rotation_sens * Input.GetAxis("Vertical");

        if (bound_angle_z < new_angle_z && new_angle_z < 180.0f)
        {
            new_angle_z = bound_angle_z;
        }
        else if (180.0f <= new_angle_z && new_angle_z < 360.0f - bound_angle_z)
        {
            new_angle_z = 360.0f - bound_angle_z;
        }


        if (bound_angle_x < new_angle_x && new_angle_x < 180.0f)
        {
            new_angle_x = bound_angle_x;
        }
        else if (180.0f <= new_angle_x && new_angle_x < 360.0f - bound_angle_x)
        {
            new_angle_x = 360.0f - bound_angle_x;
        }

        transform.eulerAngles = new Vector3(new_angle_x, transform.eulerAngles.y, new_angle_z);
    }

    private void UpdateCameraPosition()
    {
        Vector3 COM = transform.GetComponent<Rigidbody>().centerOfMass;
        Camera.main.transform.position = transform.position + new Vector3(COM.x, COM.y + camera_height, COM.z - camera_offset);
    }
}
