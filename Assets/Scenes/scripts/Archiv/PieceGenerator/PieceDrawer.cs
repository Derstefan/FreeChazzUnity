using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceDrawer
{
    private int width = 2048;
    private int height = 2048;

    private int numShapes = 5;
    private long seed = 0;
    private Texture2D texture;
    private System.Random random;

    public PieceDrawer(int width, int height)
    {
        this.width = width;
        this.height = height;
        random = new System.Random();
    }

    public Texture2D generate(long seed)
    {
        this.seed = seed;
        random = new System.Random((int)seed);
        texture = new Texture2D(width, height);
        GenerateRandomShapes();
        return texture;
    }

    private void GenerateRandomShapes()
    {
        for (int i = 0; i < numShapes; i++)
        {
            Color shapeColor = GenerateRandomColor(); // Random color for each shape (using Unity's Random)
            int xPos = random.Next(0, width);
            int yPos = random.Next(0, height);
            int size = random.Next(20, 500); // Random size for each shape

            int numSides = random.Next(3, 9); // Random number of sides for the polygon (3 to 8)
            DrawPolygon(numSides, shapeColor);
        }

        texture.Apply(); // Apply changes to the texture
    }


    private Color GenerateRandomColor()
    {
        int r = random.Next(256); // Random value for red (0-255)
        int g = random.Next(256); // Random value for green (0-255)
        int b = random.Next(256); // Random value for blue (0-255)
        float rNormalized = r / 255f; // Normalize red value to 0-1 range
        float gNormalized = g / 255f; // Normalize green value to 0-1 range
        float bNormalized = b / 255f; // Normalize blue value to 0-1 range
        return new Color(rNormalized, gNormalized, bNormalized);
    }


    private void DrawPolygon(int numSides, Color color)
    {
        List<Vector2> vertices = GenerateRandomVertices(numSides); // Generate random vertices
        List<Vector2> mirroredVertices = MirrorVertices(vertices); // Mirror the vertices horizontally

        DrawPolygon(vertices, color); // Draw the original polygon
        DrawPolygon(mirroredVertices, color); // Draw the mirrored polygon
    }

    private List<Vector2> GenerateRandomVertices(int numSides)
    {
        List<Vector2> vertices = new List<Vector2>();

        // Generate random points within the texture area
        for (int i = 0; i < numSides; i++)
        {
            float x = random.Next(0, width);
            float y = random.Next(0, height);
            Vector2 vertex = new Vector2(x, y);
            vertices.Add(vertex);
        }

        return vertices;
    }

    private List<Vector2> MirrorVertices(List<Vector2> vertices)
    {
        List<Vector2> mirroredVertices = new List<Vector2>();

        // Mirror the vertices horizontally at the middle of the area
        foreach (Vector2 vertex in vertices)
        {
            Vector2 mirroredVertex = new Vector2(width - vertex.x, vertex.y);
            mirroredVertices.Add(mirroredVertex);
        }

        return mirroredVertices;
    }


    private void DrawPolygon(List<Vector2> vertices, Color color)
    {
        // Find the minimum and maximum Y coordinates of the polygon
        int minY = (int)vertices.Min(v => v.y);
        int maxY = (int)vertices.Max(v => v.y);

        // Create an array to store the left and right edge X coordinates for each scanline
        int[] leftEdgeX = new int[maxY - minY + 1];
        int[] rightEdgeX = new int[maxY - minY + 1];

        // Initialize the edge arrays with invalid values
        for (int i = 0; i < leftEdgeX.Length; i++)
        {
            leftEdgeX[i] = int.MaxValue;
            rightEdgeX[i] = int.MinValue;
        }

        // Iterate through each edge of the polygon
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector2 currentVertex = vertices[i];
            Vector2 nextVertex = vertices[(i + 1) % vertices.Count];

            // Calculate the Y range covered by the current edge
            int startY = Mathf.CeilToInt(Mathf.Min(currentVertex.y, nextVertex.y)) - minY;
            int endY = Mathf.FloorToInt(Mathf.Max(currentVertex.y, nextVertex.y)) - minY;

            // Calculate the X coordinates of the edge for each scanline within its Y range
            for (int y = startY; y <= endY; y++)
            {
                float t = (y + minY - currentVertex.y) / (nextVertex.y - currentVertex.y);
                int x = Mathf.RoundToInt(Mathf.Lerp(currentVertex.x, nextVertex.x, t));

                // Update the left and right edge X coordinates
                leftEdgeX[y] = Mathf.Min(leftEdgeX[y], x);
                rightEdgeX[y] = Mathf.Max(rightEdgeX[y], x);
            }
        }

        // Fill the polygon using the edge arrays
        for (int y = 0; y < leftEdgeX.Length; y++)
        {
            int startX = leftEdgeX[y];
            int endX = rightEdgeX[y];

            for (int x = startX; x <= endX; x++)
            {
                texture.SetPixel(x, y + minY, color);
            }
        }
    }


}
