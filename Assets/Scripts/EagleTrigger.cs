using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleTrigger : MonoBehaviour
{

    [SerializeField] GameObject eagle;
    [SerializeField] int number_of_attacks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("plane")) {
            Vector3 eagle_spawn_pos = other.gameObject.transform.position;
            eagle_spawn_pos.x = 0;
            eagle_spawn_pos.z -= 10;
            eagle_spawn_pos.y += 10;
            GameObject enemy_eagle = Instantiate(eagle, eagle_spawn_pos, Quaternion.Euler(-28f, 180f, 0f));
            enemy_eagle.GetComponent<EagleController>().max_attacks = number_of_attacks;

        }
    }
}
