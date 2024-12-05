using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class PlaneMotion : MonoBehaviour
{
    public float g;
    public Vector3 r_0;
    public Vector3 v_0;
    public Vector3 boost_amt;
    public Vector3 punish_amt;

    private float lift_coefficient;
    private float tilt_inflation_factor;
    private float  AOA_inflation_factor;
    private Rigidbody rigid_body;

    [SerializeField] private GameObject velocity_text;

    void Start()
    {
        lift_coefficient = 9.8f;
        tilt_inflation_factor = 0.2f;
         AOA_inflation_factor = 0.2f;
        transform.position = r_0;
        rigid_body = GetComponent<Rigidbody>();
        rigid_body.velocity = v_0;
        Physics.gravity = rigid_body.mass * new Vector3(0.0f, -g, 0.0f);
    }

    void FixedUpdate()
    {
        UpdateMotion();
        velocity_text.GetComponent<Text>().text = v_0.z.ToString() + "mph";
    }

    private void UpdateMotion()
    {
        Vector3 base_lift = lift_coefficient * transform.up;
        float tilt = transform.eulerAngles.z * Mathf.Deg2Rad;
        float lift_factor = ((1 - tilt_inflation_factor) * Mathf.Cos(tilt) + tilt_inflation_factor) / Mathf.Cos(tilt);
        Vector3 lift = lift_factor * base_lift;
        rigid_body.AddForce(lift);
    }

    public void Boost() {
        v_0 += boost_amt;
        lift_coefficient += 0.2f;
        rigid_body.velocity += boost_amt;
    }

    public void Punish() {
        v_0 -= punish_amt;
        rigid_body.velocity -= boost_amt;
    }
}