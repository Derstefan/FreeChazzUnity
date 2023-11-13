using UnityEngine;

public class GroundCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has a MeshRenderer component
        MeshRenderer meshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            // Disable the MeshRenderer to make the collided object disappear
            meshRenderer.enabled = false;
        }
    }
}
