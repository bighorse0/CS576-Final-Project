using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MinionAttack: MonoBehaviour
{
    [SerializeField] private GameObject projectile_prefab;

    private GameObject plane;
    private float shooting_delay;
    private Vector3 direction_from_minion_to_plane;
    private Vector3 shooting_direction;
    private Vector3 projectile_starting_pos;
    private float projectile_velocity;
    private bool plane_is_accessible;

    void Start()
    {
        plane = GameObject.FindGameObjectWithTag("plane");
        shooting_delay = 0.5f;  
        direction_from_minion_to_plane = new Vector3(0.0f, 0.0f, 0.0f);
        projectile_starting_pos = new Vector3(0.0f, 0.0f, 0.0f);
        projectile_velocity = 25.0f;
        plane_is_accessible = false;
        StartCoroutine("Spawn");
    }

        void Update()
    {
        Vector3 plane_centroid = plane.GetComponent<BoxCollider>().bounds.center;
        Vector3 spawner_centroid = GetComponent<BoxCollider>().bounds.center;
        direction_from_minion_to_plane = plane_centroid - spawner_centroid;
        direction_from_minion_to_plane.Normalize();

        RaycastHit hit;

        if (Physics.Raycast(spawner_centroid, direction_from_minion_to_plane, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == plane)
            {
                // deflection shooting
                Vector3 target_pos = plane.transform.position;
                Vector3 target_velocity = plane.GetComponent<PlaneController>().movement_direction * plane.GetComponent<PlaneController>().RB.velocity.magnitude * 1.3f;
                float look_ahead_time = 0.0f;
                int max_iterations = 100000;

                for (int iteration = 0; iteration < max_iterations; ++iteration) {
                    float old_look_ahead_time = look_ahead_time;
                    look_ahead_time = Vector3.Distance(transform.position, (target_pos + look_ahead_time * target_velocity)) / projectile_velocity;
                    if (look_ahead_time - old_look_ahead_time < Mathf.Epsilon) {
                        //Debug.Log("break");
                        break;
                    }
                }

                Vector3 future_target_pos = target_pos + look_ahead_time * target_velocity;

                // convert future_target_pos to angle
                shooting_direction = future_target_pos - spawner_centroid;
                shooting_direction.Normalize();
                
                projectile_starting_pos = transform.position;
                projectile_starting_pos.y -= 0.5f;

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
            if (plane_is_accessible) {
                if ((plane.gameObject.transform.position.z < transform.position.z)) {
                    //Debug.Log("Spawn BAT");
                    GameObject bullet = Instantiate(projectile_prefab, projectile_starting_pos, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    bullet.GetComponent<MinionProjectile>().direction = shooting_direction;
                    bullet.GetComponent<MinionProjectile>().velocity = projectile_velocity;
                    bullet.GetComponent<MinionProjectile>().birth_time = Time.time;
                }
            }
            yield return new WaitForSeconds(shooting_delay);
        }
    }

}
