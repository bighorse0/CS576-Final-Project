using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f; // Time before the projectile destroys itself
    private WizardManager wizardManager;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy the projectile after its lifetime expires

        // Find the WizardManager in the scene (if needed)
        wizardManager = FindObjectOfType<WizardManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("plane 1")) // Check if it hits the airplane
        {
            if (wizardManager != null)
            {
                wizardManager.crash.Invoke(); // Invoke the crash event
            }

            Destroy(gameObject); // Destroy the projectile
        }
        else
        {
            Destroy(gameObject); // Destroy the projectile on other collisions
        }
    }
}
