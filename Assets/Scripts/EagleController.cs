using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EagleController : MonoBehaviour
{
    public AudioClip eagle_flap;
    public AudioClip eagle_screech;
    public AudioClip eagle_woosh;

    public UnityEvent eagle_hit;
    public UnityEvent eagle_miss;

    public int num_attacks;

    private Animator animator;
    private AudioSource source;
    private GameObject plane;
    private PlaneController plane_controller;

    private bool isFlyingIn = true;
    private bool isCaughtUp = false;
    private bool isMoving = false;
    private bool isAttacking = false;
    private bool isFlyingOut = false;
    private bool isScreeching = false;
    private bool gotHit = false;

    private float frozen_y;

    [SerializeField] private int max_attaacks;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        plane = GameObject.FindGameObjectWithTag("plane");
        plane_controller = plane.GetComponent<PlaneController>();
        eagle_hit.AddListener(plane_controller.Punish);
        eagle_miss.AddListener(plane_controller.Boost);
        StartCoroutine(FlyIn());
    }

    // Update is called once per frame
    void Update()
    {
        if (num_attacks >= max_attaacks && !isFlyingOut) {
            StartCoroutine(FlyOut());
        }

        Vector3 plane_pos = plane.gameObject.transform.position;
        Vector3 eagle_pos = transform.position;

        if (isCaughtUp) {
            eagle_pos.z = plane_pos.z += 15f; 
        }
            
        if (isAttacking && !isFlyingIn && !isFlyingOut) {
            eagle_pos.y = frozen_y;
        }
        else if (!isFlyingIn && !isFlyingOut) {
            eagle_pos.y = plane_pos.y -= 10f;
        }

        transform.position = eagle_pos;

        if (!isMoving && !isFlyingIn && !isFlyingOut) {
            StartCoroutine(GoToAttackPoint(plane_pos.x));
        }
    }

    IEnumerator FlyIn() {
        Debug.Log("EAGLE Catching Up");

        animator.SetFloat("Speed", 5);
        StartCoroutine(Screech());

        while (transform.position.z < (plane.gameObject.transform.position.z + 15f)) {
            Vector3 eagle_pos = transform.position;
            eagle_pos.z += 20 * Time.deltaTime;
            transform.position = eagle_pos;
            yield return null;
        }

        animator.SetFloat("Speed", 0); 

        Debug.Log("EAGLE Caught Up");
        isCaughtUp = true;
        Debug.Log("EAGLE Flying In");

        while (transform.position.y > (plane.gameObject.transform.position.y - 10f)) {
            Vector3 eagle_pos = transform.position;
            eagle_pos.y -= 5 * Time.deltaTime;
            transform.position = eagle_pos;
            yield return null;
        }

        Debug.Log("EAGLE DONE Flying In");
        isFlyingIn = false;
        yield return new WaitForSeconds(1f);
    }

    IEnumerator GoToAttackPoint(float attack_x) {
        Debug.Log("EAGLE GOING TO ATTACK POINT");
        isMoving = true;
        yield return new WaitForSeconds(2f);

        animator.SetFloat("Speed", 5);

        while ( !((attack_x - 0.1f) < transform.position.x) || !(transform.position.x < (attack_x + 0.1f)) ) {
            Vector3 eagle_pos = transform.position;
            if (eagle_pos.x < attack_x) {
                eagle_pos.x += 3 * Time.deltaTime;
            }
            if (eagle_pos.x > attack_x) {
                eagle_pos.x -= 3 * Time.deltaTime;
            }
            transform.position = eagle_pos;

            yield return null;
        }

        animator.SetFloat("Speed", 0);

        StartCoroutine(Screech());

        Debug.Log("EAGLE Attack Position Reached");
        yield return new WaitForSeconds(2f);
        Attack();
    }

    private void Attack() {
        gotHit = false;
        Debug.Log("EAGLE Attacking");
        frozen_y = transform.position.y;
        isAttacking = true;
	    animator.SetTrigger("Attack");
        source.PlayOneShot(eagle_woosh);
		StartCoroutine(stopAttack(1));
    }

    public IEnumerator stopAttack(float length) {
		yield return new WaitForSeconds(length); 
		isAttacking = false;
        isMoving = false;
        num_attacks++;
        if (gotHit) {
            Debug.Log("HIT");
            eagle_hit.Invoke();
        }
        else {
            Debug.Log("MISS");
            eagle_miss.Invoke();
        }
	}

    IEnumerator FlyOut() {
        Debug.Log("EAGLE Flying Away");
        isFlyingOut = true;
        yield return new WaitForSeconds(2f);
        isCaughtUp = false;

        animator.SetTrigger("Leave");
        
        while ( transform.position.y < 70 ) {
            Vector3 eagle_pos = transform.position;
            eagle_pos.z -= 20 * Time.deltaTime;
            eagle_pos.y += 10 * Time.deltaTime;
            transform.position = eagle_pos;
            yield return null;
        }
       
        
        Debug.Log("EAGLE Flew Away");
        Destroy(gameObject);
    }

    IEnumerator Screech() {
        float screech_len = eagle_screech.length;
        isScreeching = true;
        source.PlayOneShot(eagle_screech, 0.4f);
        yield return new WaitForSeconds(screech_len);
        isScreeching = false;
    }

    public void Flap() {
        if (!isScreeching) {
            source.pitch = Random.Range(0.8f, 1.2f);
        }
        source.PlayOneShot(eagle_flap);
    }

    public void DealDamage(DealDamageComponent comp) {
    }

    public void Hit() {
        gotHit = true;
    }
}
