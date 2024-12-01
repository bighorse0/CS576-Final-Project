using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float horizontal_rotation_sens;
    public float vertical_rotation_sens;

    private float horizontal_rotation_bound;
    private float vertical_rotation_bound;

    // Start is called before the first frame update
    void Start()
    {
        horizontal_rotation_bound = 60;
          vertical_rotation_bound = 60;
    }

    // Update is called once per frame
    void Update()
    {
        float new_angle_z = transform.eulerAngles.z - horizontal_rotation_sens * Input.GetAxis("Horizontal");
        float new_angle_x = transform.eulerAngles.x -   vertical_rotation_sens * Input.GetAxis("Vertical");

        if (60.0f < new_angle_z && new_angle_z < 180.0f )
        {
            new_angle_z = 60.0f;
        } else if (180.0f <= new_angle_z && new_angle_z < 300.0f)
        {
            new_angle_z = 300.0f;
        }


        if (60.0f < new_angle_x && new_angle_x < 180.0f)
        {
            new_angle_x = 60.0f;
        }
        else if (180.0f <= new_angle_x && new_angle_x < 300.0f)
        {
            new_angle_x = 300.0f;
        }

        transform.eulerAngles = new Vector3(new_angle_x, 0.0f, new_angle_z);
    }
}
