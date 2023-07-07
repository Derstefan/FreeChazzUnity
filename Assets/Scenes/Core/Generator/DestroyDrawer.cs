using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDrawer
{
  public static void startDestroyAnimation(GameObject gameObject, float power,float fadeOut){
        int sub = gameObject.transform.childCount;
        for (int i = 0; i < sub; i++)
        {
            SplitMesh(gameObject.transform, gameObject.transform.GetChild(i), power,fadeOut);
        }
    }

public static void SplitMesh(Transform parent, Transform transform, float power, float fadeOut)
{
    MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
    if (meshFilter == null)
    {
        Debug.LogError("MeshFilter component not found!");
        return;
    }

    Mesh mesh = meshFilter.sharedMesh;
    if (mesh == null)
    {
        Debug.LogError("Mesh not found!");
        return;
    }

    Vector3[] vertices = mesh.vertices;
    int[] triangles = mesh.triangles;
    Color[] colors = mesh.colors; // Get the original mesh colors

    int partCount = Mathf.Min(1032, Mathf.CeilToInt(triangles.Length / 3f));
    int trianglesPerPart = Mathf.CeilToInt(triangles.Length / (float)partCount);

    for (int i = 0; i < partCount; i++)
    {
        int startIndex = i * trianglesPerPart * 3;
        int endIndex = Mathf.Min(startIndex + trianglesPerPart * 3, triangles.Length);

        // Create a new smaller mesh
        GameObject newMeshObject = new GameObject("SubMesh");
        newMeshObject.transform.position = transform.position;
        newMeshObject.transform.rotation = transform.rotation;
        newMeshObject.transform.localScale = transform.localScale;

        MeshFilter newMeshFilter = newMeshObject.AddComponent<MeshFilter>();
        MeshRenderer newMeshRenderer = newMeshObject.AddComponent<MeshRenderer>();
        newMeshRenderer.material = new Material(Shader.Find("Sprites/Default"));

        Mesh newMesh = new Mesh();

        // Copy vertices, triangles, and colors for the current part
        int numVertices = Mathf.Abs(endIndex - startIndex);
        Vector3[] newVertices = new Vector3[numVertices];
        int[] newTriangles = new int[numVertices];
        Color[] newColors = new Color[numVertices];

        for (int j = startIndex, k = 0; j < endIndex; j++, k++)
        {
            int vertexIndex = triangles[j];
            newVertices[k] = vertices[vertexIndex];
            newTriangles[k] = k;
            newColors[k] = colors[vertexIndex]; // Assign the color from the original mesh
        }

        newMesh.vertices = newVertices;
        newMesh.triangles = newTriangles;
        newMesh.colors = newColors; // Assign the colors to the new mesh

        newMeshFilter.sharedMesh = newMesh;

        // Add 2D physics effect
        Rigidbody2D newRigidbody2D = newMeshObject.AddComponent<Rigidbody2D>();
        newRigidbody2D.gravityScale = 1.0f;
        newRigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        BoxCollider2D newCollider2D = newMeshObject.AddComponent<BoxCollider2D>();
        newCollider2D.size = newMeshFilter.sharedMesh.bounds.size;

        // Apply force to separate the smaller meshes
        Vector3 center = newMeshFilter.sharedMesh.bounds.center;
        newRigidbody2D.AddForceAtPosition(center.normalized * power, center, ForceMode2D.Impulse);

        // Set the z-position to zero to make the shapes fall on the ground
        //newMeshObject.transform.position = new Vector3(newMeshObject.transform.position.x, newMeshObject.transform.position.y, 0f);

         MeshFadeOut meshFadeOut = newMeshObject.AddComponent<MeshFadeOut>();
        meshFadeOut.Initialize(newMeshRenderer.material,fadeOut);
    }

    MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();
    if (meshRenderer != null)
        meshRenderer.enabled = false;

    MeshCollider meshCollider = transform.GetComponent<MeshCollider>();
    if (meshCollider != null)
        meshCollider.enabled = false;
}



}
