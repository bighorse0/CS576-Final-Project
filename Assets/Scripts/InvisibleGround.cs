using UnityEngine;

public class DisableRenderer : MonoBehaviour
{
    void Start()
    {
        // Disable the Renderer component on this GameObject
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
        else
        {
            Debug.LogWarning("No Renderer component found on " + gameObject.name);
        }
    }
}
