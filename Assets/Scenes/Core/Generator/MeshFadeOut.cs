
using UnityEngine;


public class MeshFadeOut : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Material material;
    private float fadeDuration = 1f;
    private float fadeTimer = 0f;

    public void Initialize(Material meshMaterial, float duration)
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshMaterial;
        fadeDuration = duration;
    }

    private void Update()
    {
        fadeTimer += Time.deltaTime;

        if (fadeTimer < fadeDuration)
        {
            float normalizedTime = fadeTimer / fadeDuration;
            Color color = material.color;
            color.a = Mathf.Lerp(1f, 0f, normalizedTime);
            material.color = color;
        }
        else
        {
            // Destroy the game object after the fade-out effect
            Destroy(gameObject);
        }
    }
}
