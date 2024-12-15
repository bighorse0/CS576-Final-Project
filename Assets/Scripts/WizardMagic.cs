using System.Collections;
using UnityEngine;

public class PredictiveNPCShooter : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile to fire
    public Transform firePoint;          // The point where the projectile is fired from
    public Transform player;             // Reference to the player's Transform
    public float projectileSpeed = 10f;  // Speed of the projectile
    public float fireRate = 1f;          // Time between shots
    public float detectionRadius = 20f;  // Radius within which the NPC will shoot

    private float nextFireTime = 0f;     // Time until the next shot
    private Vector3 shootingDirection;   // Direction to fire the projectile
    private Vector3 projectileStartingPos;
    private bool playerIsAccessible;

    void Update()
    {
        // Check if the player is within the detection radius
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            PredictPlayerPosition();
            if (playerIsAccessible && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void PredictPlayerPosition()
    {
        // Step 1: Raycast to check accessibility to player
        Vector3 npcPosition = transform.position;
        Vector3 playerPosition = player.position;
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
        Vector3 directionToPlayer = playerPosition - npcPosition;

        RaycastHit hit;
        playerIsAccessible = false;

        if (Physics.Raycast(npcPosition, directionToPlayer.normalized, out hit, detectionRadius))
        {
            if (hit.collider.gameObject == player.gameObject)
            {
                // Step 2: Predict the player's future position
                float epsilon = 0.1f;
                float deltaPos = float.MaxValue;
                Vector3 predictedPosition = playerPosition;
                Vector3 lastPredictedPosition;

                float lookAheadTime = directionToPlayer.magnitude / projectileSpeed;

                while (deltaPos > epsilon)
                {
                    lastPredictedPosition = predictedPosition;

                    // Calculate new lookahead time and position
                    lookAheadTime = (predictedPosition - npcPosition).magnitude / projectileSpeed;
                    predictedPosition = playerPosition + playerVelocity * lookAheadTime;

                    deltaPos = (predictedPosition - lastPredictedPosition).magnitude;
                }

                // Step 3: Set shooting direction and starting position
                shootingDirection = (predictedPosition - firePoint.position).normalized;
                projectileStartingPos = firePoint.position;
                playerIsAccessible = true;

                // Optional debug visualization
                Debug.DrawLine(firePoint.position, predictedPosition, Color.green, 0.5f);
            }
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab is not assigned!");
            return;
        }

        // Instantiate projectile
        GameObject projectile = Instantiate(projectilePrefab, projectileStartingPos, Quaternion.identity);

        // Check if the projectile has a Rigidbody, if not add one manually
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        if (projectileRigidbody != null)
        {
            // If the Rigidbody is kinematic, use manual movement
            if (projectileRigidbody.isKinematic)
            {
                PredictiveProjectile movementScript = projectile.AddComponent<PredictiveProjectile>();
                movementScript.Initialize(shootingDirection, projectileSpeed);
            }
            else
            {
                // Normal behavior for non-kinematic Rigidbody
                projectileRigidbody.velocity = shootingDirection * projectileSpeed;
                Debug.Log("Projectile Fired! Velocity: " + projectileRigidbody.velocity);
            }
        }
        else
        {
            Debug.LogError("Projectile Prefab does not have a Rigidbody component!");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection radius for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }




    public class PredictiveProjectile : MonoBehaviour
    {
    private Vector3 direction;
    private float speed;

    // Initialize with the direction and speed values
    public void Initialize(Vector3 shootingDirection, float projectileSpeed)
    {
        direction = shootingDirection;
        speed = projectileSpeed;
    }

    // Move the projectile manually (since Rigidbody is Kinematic)
    void Update()
    {
        // Update position by moving it along the predicted direction
        transform.position += direction * speed * Time.deltaTime;
    }
}

}
