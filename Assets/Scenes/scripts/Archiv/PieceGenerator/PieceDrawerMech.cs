using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceDrawerMech
{
    private Transform parent;
    private int numShapes = 5;
    private long seed = 0;

    public PieceDrawerMech(Transform parent)
    {
        this.parent = parent;
    }

    public void GenerateRandomPieces(long seed)
    {
        this.seed = seed;
        Random.InitState((int)seed);

        for (int i = 0; i < numShapes; i++)
        {
            Color shapeColor = GenerateRandomColor();
            int numSides = Random.Range(3, 9);
            GeneratePolygon(numSides, shapeColor);
        }
    }

    private Color GenerateRandomColor()
    {
        return Random.ColorHSV();
    }

    private void GeneratePolygon(int numSides, Color color)
    {
        GameObject polygonGO = new GameObject("Polygon");
        polygonGO.transform.parent = parent;

        MeshFilter meshFilter = polygonGO.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = polygonGO.AddComponent<MeshRenderer>();

        List<Vector3> vertices = GenerateRandomVertices(numSides);
        List<int> triangles = GenerateTriangles(numSides);
        List<Color> colors = GenerateColors(numSides, color);

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetColors(colors);
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshRenderer.material = new Material(Shader.Find("Standard"));
    }

    private List<Vector3> GenerateRandomVertices(int numSides)
    {
        List<Vector3> vertices = new List<Vector3>();

        float dwidth = parent.lossyScale.x;

        for (int i = 0; i < numSides; i++)
        {
            float angle = 2 * Mathf.PI * i / numSides;
            float x = Mathf.Cos(angle) * dwidth / 2f;
            float y = Mathf.Sin(angle) * dwidth / 2f;
            Vector3 vertex = new Vector3(x, y, 0f);
            vertices.Add(vertex);
        }

        return vertices;
    }

    private List<int> GenerateTriangles(int numSides)
    {
        List<int> triangles = new List<int>();

        for (int i = 0; i < numSides - 2; i++)
        {
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
        }

        return triangles;
    }

    private List<Color> GenerateColors(int numSides, Color color)
    {
        List<Color> colors = new List<Color>();

        for (int i = 0; i < numSides; i++)
        {
            colors.Add(color);
        }

        return colors;
    }
}


