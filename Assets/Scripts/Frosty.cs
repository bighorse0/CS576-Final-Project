using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frosty : MonoBehaviour
{
    public Vector3 corrected_bias;  // controls where the snowball spawns intially with regard to Frosty
    public Vector3 addedSnowBallSize;  // controls how large the snowball is
    public float shooting_delay;  // delay between snowballs thrown
    public float projectile_velocity;  // velocity of snowballs
    public int max_iterations;  // maximum iterations Frosty iterates to find optimum throwing location (decrease for increased performance)
    public bool checkRaycast;  // whether Frosty checks for path to target before throwing snowball 
    public GameObject projectile_template;  // projectile

    // private GameObject projectile_template;
    private Vector3 direction_from_frosty_to_plane;
    private Vector3 shooting_direction;
    private Vector3 projectile_starting_pos;
    private bool plane_is_accessible;


    // Start is called before the first frame update
    void Start()
    {
        // projectile_template = (GameObject)Resources.Load("Level Prefabs/Bubble Prefabs/snowball", typeof(GameObject));  // projectile prefab
        direction_from_frosty_to_plane = new Vector3(0.0f, 0.0f, 0.0f);
        projectile_starting_pos = new Vector3(0.0f, 0.0f, 0.0f);
        plane_is_accessible = false;
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject plane = GameObject.Find("plane 1");
        Vector3 plane_centroid = plane.GetComponent<Collider>().bounds.center;
        Vector3 frosty_centroid = GetComponent<Collider>().bounds.center;
        direction_from_frosty_to_plane = plane_centroid - frosty_centroid;
        direction_from_frosty_to_plane.Normalize();
        // Debug.Log("direction_from_frosty_to_plane.eulerAngles: " + direction_from_frosty_to_plane.ToString());
        // Debug.Log("plane_centroid, frosty_centroid: " + plane_centroid.ToString() + ", " + frosty_centroid.ToString());
        // Debug.Log("plane.GetComponent<PlaneControllerSnow>().RB.velocity: " + plane.GetComponent<PlaneControllerSnow>().RB.velocity);

        RaycastHit hit;
        if (Physics.Raycast(frosty_centroid, direction_from_frosty_to_plane, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (!checkRaycast || hit.collider.gameObject == plane || hit.collider.gameObject.name.Contains("Trigger"))
            {
                float delta_position = float.PositiveInfinity;
                Vector3 future_target_position = plane_centroid;
                int i = 0;
                while (delta_position > 0.001 && i < max_iterations){
                    float distance = (future_target_position - frosty_centroid).magnitude;
                    //Debug.Log("distance: " + distance.ToString());
                    float look_ahead_time = distance / projectile_velocity;
                    //Debug.Log("look_ahead_time: " + look_ahead_time.ToString());
                    Vector3 last_future_target_position = future_target_position;
                    //Debug.Log("last_future_target_position: " + last_future_target_position.ToString());
                    future_target_position = plane_centroid + (look_ahead_time * plane.GetComponent<PlaneControllerSnow>().RB.velocity);// * (1.0f + 0.5f * look_ahead_time); // * plane.GetComponent<PlaneControllerSnow>().RB.velocity;
                    //Debug.Log("future_target_position" + future_target_position.ToString());
                    delta_position = (future_target_position - last_future_target_position).magnitude;
                    i++;
                }
                // Debug.Log("delta_position: " + delta_position.ToString());
                shooting_direction = (future_target_position - frosty_centroid);
                 //Debug.Log("shooting_direction: " + shooting_direction.ToString());
                shooting_direction.Normalize();
                // Debug.Log("shooting_direction.Normalize(): " + shooting_direction.ToString());

                float angle_to_rotate_frosty = Mathf.Rad2Deg * Mathf.Atan2(shooting_direction.x, shooting_direction.z);
                // Debug.Log("angle_to_rotate_frosty: " + angle_to_rotate_frosty.ToString());
                transform.eulerAngles = new Vector3(0.0f, angle_to_rotate_frosty, 0.0f);
                // Debug.Log("transform.eulerAngles: " + transform.eulerAngles.ToString());
                Vector3 current_frosty_direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y), 1.1f, Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y));
                projectile_starting_pos = transform.position + 1.1f * current_frosty_direction + corrected_bias;  // estimated position of the frosty
                plane_is_accessible = true;
            }
            else
                plane_is_accessible = false;       
        }
    }

    private IEnumerator Spawn()
    {
        while (true)
        {            
            if (plane_is_accessible)
            {
                GameObject new_object = Instantiate(projectile_template, projectile_starting_pos, Quaternion.identity);
                new_object.GetComponent<Snowball>().velocity = projectile_velocity;
                new_object.GetComponent<Snowball>().birth_time = Time.time;
                new_object.GetComponent<Snowball>().frosty = transform.gameObject;
                new_object.GetComponent<Snowball>().direction = shooting_direction;
                new_object.GetComponent<Snowball>().addedSnowballSize = addedSnowBallSize;
            }
            yield return new WaitForSeconds(shooting_delay); // next shot will be shot after this delay
        }
    }
}
