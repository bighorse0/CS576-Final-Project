using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BatSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bat_prefab;

    private GameObject plane;
    private float spawning_delay; 
    private Vector3 direction_from_spawner_to_plane;
    private Vector3 flying_direction;
    private Vector3 bat_starting_pos;
    private float bat_velocity;
    private bool plane_is_in_range;

    // Start is called before the first frame update
    void Start()
    {
        plane = GameObject.FindGameObjectWithTag("plane");
        spawning_delay = 1f;  
        direction_from_spawner_to_plane = new Vector3(0.0f, 0.0f, 0.0f);
        bat_starting_pos = new Vector3(0.0f, 0.0f, 0.0f);
        bat_velocity = 25.0f;
        plane_is_in_range = false;
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 plane_centroid = plane.GetComponent<BoxCollider>().bounds.center;
        Vector3 spawner_centroid = GetComponent<Collider>().bounds.center;
        direction_from_spawner_to_plane = plane_centroid - spawner_centroid;
        direction_from_spawner_to_plane.Normalize();

        RaycastHit hit;

        if (Physics.Raycast(spawner_centroid, direction_from_spawner_to_plane, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == plane)
            {
                // deflection shooting
                Vector3 target_pos = plane.transform.position;
                Vector3 target_velocity = plane.GetComponent<PlaneController>().movement_direction * plane.GetComponent<PlaneController>().RB.velocity.magnitude * 1.2f;
                float look_ahead_time = 0.0f;
                int max_iterations = 1000;

                for (int iteration = 0; iteration < max_iterations; ++iteration) {
                    float old_look_ahead_time = look_ahead_time;
                    look_ahead_time = Vector3.Distance(transform.position, (target_pos + look_ahead_time * target_velocity)) / bat_velocity;
                    if (look_ahead_time - old_look_ahead_time < Mathf.Epsilon) {
                        //Debug.Log("break");
                        break;
                    }
                }

                Vector3 future_target_pos = target_pos + look_ahead_time * target_velocity;

                // convert future_target_pos to angle
                flying_direction = future_target_pos - spawner_centroid;
                flying_direction.Normalize();
                
                bat_starting_pos = transform.position;
                bat_starting_pos.y -= 0.5f;

                plane_is_in_range = true;
            }
            else
                plane_is_in_range = false;            
        }
    }

    private IEnumerator Spawn()
    {
        while (true)
        {            
            if (plane_is_in_range) {
                if ((plane.gameObject.transform.position.z < transform.position.z)) {
                    //Debug.Log("Spawn BAT");
                    GameObject new_bat = Instantiate(bat_prefab, bat_starting_pos, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    new_bat.GetComponent<Bat>().direction = flying_direction;
                    new_bat.GetComponent<Bat>().velocity = bat_velocity;
                    new_bat.GetComponent<Bat>().birth_time = Time.time;
                }
            }
            yield return new WaitForSeconds(spawning_delay);
        }
    }

}
