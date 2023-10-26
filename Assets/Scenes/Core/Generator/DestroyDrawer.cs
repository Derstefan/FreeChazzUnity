using System;
using UnityEngine;

public class DestroyDrawer : MonoBehaviour
{


    private static Shader DEFAULT_SHADER = Shader.Find("Sprites/Default");
    public static void startDestroyAnimation2(GameObject gameObject, float power, float fadeOut)
    {

        Sprite sprite = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;



        Vector2[] vertices2D = sprite.vertices[0..(sprite.vertices.Length > 100 ? 100 : sprite.vertices.Length)];
        ushort[] triangles = sprite.triangles[0..(sprite.vertices.Length > 99 ? 99 : sprite.vertices.Length)];

        Debug.Log("1vertices: " + vertices2D.Length);
        Debug.Log("1triangles: " + triangles.Length);
        Mesh mesh = new Mesh();
        mesh.SetVertices(Array.ConvertAll(vertices2D, i => (Vector3)i));
        mesh.SetUVs(0, sprite.uv);
        mesh.SetTriangles(Array.ConvertAll(triangles, i => (int)i), 0);



        mesh.colors = Array.ConvertAll(vertices2D, i => new Color(UnityEngine.Random.Range(0, 0.3f), UnityEngine.Random.Range(0, 0.3f), UnityEngine.Random.Range(0, 0.3f), 1f)); //


        GameObject meshObject = new GameObject("MeshPiece");
        meshObject.transform.parent = gameObject.transform;
        meshObject.transform.localPosition = Vector3.zero;
        meshObject.AddComponent<MeshRenderer>().material = RenderUtil.DEFAULT_MATERIAL;
        meshObject.AddComponent<MeshFilter>().mesh = mesh;


        SplitMesh(gameObject.transform, meshObject.transform, power, fadeOut);
        int sub = gameObject.transform.childCount;
        for (int i = 0; i < sub; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }

    }



    public static void startDestroyAnimation(GameObject gameObject, float power, float fadeOut, float size)
    {



        GameObject meshObject = new GameObject("MeshPiece");
        meshObject.transform.parent = gameObject.transform;
        meshObject.transform.localPosition = Vector3.zero;
        meshObject.AddComponent<MeshRenderer>().material = RenderUtil.DEFAULT_MATERIAL;
        meshObject.AddComponent<MeshFilter>().mesh = createRandomMesh(50, size);

        SplitMesh(gameObject.transform, meshObject.transform, power, fadeOut);
        int sub = gameObject.transform.childCount;
        for (int i = 0; i < sub; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }

    }



    public static void SplitMesh(Transform parent, Transform transform, float power, float fadeOut)
    {
        MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            //Debug.LogError("MeshFilter component not found!");
            return;
        }
        Mesh mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            //Debug.LogError("Mesh not found!");
            return;
        }

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Color[] colors = mesh.colors; // Get the original mesh colors

        Debug.Log("vertices: " + vertices.Length);
        Debug.Log("triangles: " + triangles.Length);
        Debug.Log("colors: " + colors.Length);

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
            newMeshRenderer.material = RenderUtil.DEFAULT_MATERIAL;

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
            meshFadeOut.Initialize(newMeshRenderer.material, fadeOut);
        }

        MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
            meshRenderer.enabled = false;

        MeshCollider meshCollider = transform.GetComponent<MeshCollider>();
        if (meshCollider != null)
            meshCollider.enabled = false;
    }


    private static Mesh createSquare(float size)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, -1f*size,0),
            new Vector3(1f*size, -1f*size,0),
            new Vector3(1f*size, 0,0),
        };
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
        mesh.colors = new Color[] { Color.red, Color.red, Color.red, Color.red };
        return mesh;
    }

    private static Mesh createRandomMesh(int n, float size)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[n];
        int[] triangles = new int[(n - 2) * 3]; // Triangulate as a fan

        for (int i = 0; i < n; i++)
        {
            float angle = (i / (float)n) * 2 * Mathf.PI;
            float randomX = UnityEngine.Random.Range(0f, size);
            float randomY = UnityEngine.Random.Range(0f, size);

            vertices[i] = new Vector3(randomX * Mathf.Cos(angle), randomY * Mathf.Sin(angle), 0);

            if (i >= 2)
            {
                int triIndex = (i - 2) * 3;
                triangles[triIndex] = 0;
                triangles[triIndex + 1] = i - 1;
                triangles[triIndex + 2] = i;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Calculate normals and colors (optional)
        Vector3[] normals = new Vector3[n];
        Color[] colors = new Color[n];
        for (int i = 0; i < n; i++)
        {
            normals[i] = Vector3.forward; // You can use other normals as needed.
            colors[i] = new Color(UnityEngine.Random.Range(0, 0.3f), UnityEngine.Random.Range(0, 0.3f), UnityEngine.Random.Range(0, 0.3f), 1f); // Random color for each vertex (optional).
        }
        mesh.normals = normals;
        mesh.colors = colors;

        return mesh;
    }


}
