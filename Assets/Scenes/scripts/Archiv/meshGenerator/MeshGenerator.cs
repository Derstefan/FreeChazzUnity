using UnityEngine;

public class MeshGenerator
{
    private float width;
    private float height;

    public MeshGenerator(float width, float height)
    {
        this.width = width;
        this.height = height;
    }

    public Mesh GeneratePolygonMesh(int num)
    {
        Mesh mesh = new Mesh();

        int numVertices = num * 2; // Total number of vertices for both polygons
        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];
        int[] triangles = new int[num * 3 * 2]; // Total number of triangles for both polygons
        Color[] colors = new Color[numVertices]; // New array for vertex colors

        // Generate original polygon
        for (int i = 0; i < num; i++)
        {
            int randX = Random.Range(0, (int)width);
            int randY = Random.Range(0, (int)height);
            vertices[i] = new Vector3(randX, randY);
            uv[i] = new Vector2(randX, randY);

            // Assign triangle indices for the original polygon
            int triangleIndex = i * 3;
            triangles[triangleIndex] = i;
            triangles[triangleIndex + 1] = (i + 1) % num; // Wrap around to the first vertex
            triangles[triangleIndex + 2] = (i + 2) % num; // Wrap around to the second vertex

            colors[i] = Color.blue; // Assign blue color to each vertex
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

            colors[mirroredIndex] = Color.red; // Assign red color to each vertex
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.subMeshCount = 2; // Two submeshes: original and mirrored polygons
        mesh.SetTriangles(triangles, 0); // Assign triangles for the original polygon
        mesh.SetTriangles(triangles, 1); // Assign triangles for the mirrored polygon
        mesh.colors = colors; // Assign colors to the mesh vertices

        return mesh;
    }








}