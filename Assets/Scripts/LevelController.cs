using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] internal Vector3 vel;

    [SerializeField] private int level;

    internal bool reached_end;

    // Start is called before the first frame update
    void Start()
    {
        reached_end = false;
    }

    void Update() {

    }

    public void Win() {
        reached_end = true;
        // save level as completed
    }



}
