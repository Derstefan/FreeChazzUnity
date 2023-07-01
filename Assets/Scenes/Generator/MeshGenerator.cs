using UnityEngine;
using System.Security.Cryptography;
using System.Text;

public class MeshGenerator
{
    private float width;
    private float height;

    public MeshGenerator(float width, float height)
    {
        this.width = width*0.8f;
        this.height = height*0.9f;
    }

public Mesh GeneratePolygonMesh(int num)
{
    Mesh mesh = new Mesh();

    Color randomColor = new Color(Random.value * 0.3f, Random.value * 0.3f, Random.value * 0.3f);

    int numVertices = num * 2; // Total number of vertices for both polygons
    Vector3[] vertices = new Vector3[numVertices];
    Vector2[] uv = new Vector2[numVertices];
    int[] triangles = new int[num * 3 * 2]; // Total number of triangles for both polygons
    Color[] colors = new Color[numVertices]; // New array for vertex colors

    // Generate original polygon
    for (int i = 0; i < num; i++)
    {
        float randX = Random.Range(0f, width / 2);
        float randY = Random.Range(height / 2, -height / 2);
        vertices[i] = new Vector3(randX, randY);
        uv[i] = new Vector2(randX, randY);

        // Assign triangle indices for the original polygon
        int triangleIndex = i * 3;
        triangles[triangleIndex] = i;
        triangles[triangleIndex + 1] = (i + 1) % num; // Wrap around to the first vertex
        triangles[triangleIndex + 2] = (i + 2) % num; // Wrap around to the second vertex
    }

    // Generate mirrored polygon
    for (int i = 0; i < num; i++)
    {
        int mirroredIndex = i + num; // Index offset for the mirrored polygon
        vertices[mirroredIndex] = new Vector3(-vertices[i].x, vertices[i].y);
        uv[mirroredIndex] = new Vector2(-uv[i].x, uv[i].y);

        // Assign triangle indices for the mirrored polygon
        int triangleIndex = (i + num) * 3;
        triangles[triangleIndex] = mirroredIndex;
        triangles[triangleIndex + 1] = (mirroredIndex + 1) % numVertices; // Wrap around to the first vertex
        triangles[triangleIndex + 2] = (mirroredIndex + 2) % numVertices; // Wrap around to the second vertex
    }

    // Generate a random color for the entire mesh
    

    for (int i = 0; i < numVertices; i++)
    {
        colors[i] = randomColor;
    }

    mesh.vertices = vertices;
    mesh.uv = uv;
    mesh.subMeshCount = 2; // Two submeshes: original and mirrored polygons
    mesh.SetTriangles(triangles, 0); // Assign triangles for the original polygon
    mesh.SetTriangles(triangles, 1); // Assign triangles for the mirrored polygon
    mesh.colors = colors; // Assign colors to the mesh vertices

    return mesh;
}


public Mesh GeneratePolygonMesh(int num, string seed)
{
    Mesh mesh = new Mesh();

    UnityEngine.Random.InitState(StringToSeed(seed));
    Color randomColor = new Color(UnityEngine.Random.value * 0.3f, UnityEngine.Random.value * 0.3f, UnityEngine.Random.value * 0.2f);

    int numVertices = num * 2; // Total number of vertices for both polygons
    Vector3[] vertices = new Vector3[numVertices];
    Vector2[] uv = new Vector2[numVertices];
    int[] triangles = new int[num * 3 * 2]; // Total number of triangles for both polygons
    Color[] colors = new Color[numVertices]; // New array for vertex colors

    // Generate original polygon
    for (int i = 0; i < num; i++)
    {
        float randX = UnityEngine.Random.Range(0f, width / 2f);
        float randY = UnityEngine.Random.Range(height / 2f, -height / 2f);
        vertices[i] = new Vector3(randX, randY);
        uv[i] = new Vector2(randX, randY);

        // Assign triangle indices for the original polygon
        int triangleIndex = i * 3;
        triangles[triangleIndex] = i;
        triangles[triangleIndex + 1] = (i + 1) % num; // Wrap around to the first vertex
        triangles[triangleIndex + 2] = (i + 2) % num; // Wrap around to the second vertex
    }

    // Generate mirrored polygon
    for (int i = 0; i < num; i++)
    {
        int mirroredIndex = i + num; // Index offset for the mirrored polygon
        vertices[mirroredIndex] = new Vector3(-vertices[i].x, vertices[i].y);
        uv[mirroredIndex] = new Vector2(-uv[i].x, uv[i].y);

        // Assign triangle indices for the mirrored polygon
        int triangleIndex = (i + num) * 3;
        triangles[triangleIndex] = mirroredIndex;
        triangles[triangleIndex + 1] = (mirroredIndex + 1) % numVertices; // Wrap around to the first vertex
        triangles[triangleIndex + 2] = (mirroredIndex + 2) % numVertices; // Wrap around to the second vertex
    }

    // Generate a random color for the entire mesh
    for (int i = 0; i < numVertices; i++)
    {
        colors[i] = randomColor;
    }

    mesh.vertices = vertices;
    mesh.uv = uv;
    mesh.subMeshCount = 2; // Two submeshes: original and mirrored polygons
    mesh.SetTriangles(triangles, 0); // Assign triangles for the original polygon
    mesh.SetTriangles(triangles, 1); // Assign triangles for the mirrored polygon
    mesh.colors = colors; // Assign colors to the mesh vertices

    return mesh;
}



// Function to convert a string seed to an integer seed
int StringToSeed(string seedString)
{
    using (MD5 md5 = MD5.Create())
    {
        byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(seedString));
        int seed = 0;
        for (int i = 0; i < 4; i++)
        {
            seed ^= ((int)hashBytes[i]) << (i * 8);
        }
        return seed;
    }
}


}