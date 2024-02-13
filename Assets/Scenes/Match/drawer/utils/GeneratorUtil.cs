using System.Security.Cryptography;
using System.Text;
using UnityEngine;






public class GeneratorUtil
{
    public static Vector2 GenerateRandomPoint(float x0, float y0, float x1, float y1)
    {
        float randomX = UnityEngine.Random.Range(x0, x1);
        float randomY = UnityEngine.Random.Range(y0, y1);

        return new Vector2(randomX, randomY);
    }

    public static Vector2[] GenerateRandomPoints(int numPoints, float x0, float y0, float x1, float y1)
    {
        Vector2[] points = new Vector2[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            points[i] = GenerateRandomPoint(x0, y0, x1, y1);
        }

        return points;
    }

    public static Vector2[] MirrorPoints(Vector2[] points, float size)
    {
        Vector2[] mirroredPoints = new Vector2[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            mirroredPoints[i] = new Vector2(size - points[i].x, points[i].y);
        }

        return mirroredPoints;
    }

    public static Vector2[] GenerateRandomPointsInsideShape(Vector2[] shapeVertices, int numberOfPoints)
    {
        Vector2[] randomPoints = new Vector2[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            // Generate a random point inside the shape using barycentric coordinates
            float r1 = Random.Range(0f, 1f);
            float r2 = Random.Range(0f, 1f);

            Vector2 randomPoint = GetRandomPointInShape(shapeVertices, r1, r2);
            randomPoints[i] = randomPoint;
        }

        return randomPoints;
    }

    public static Vector2 GetRandomPointInShape(Vector2[] shapeVertices, float r1, float r2)
    {
        // Assume the shape is convex for simplicity
        int triangleIndex = Random.Range(0, shapeVertices.Length - 2);
        Vector2 v0 = shapeVertices[0];
        Vector2 v1 = shapeVertices[triangleIndex + 1];
        Vector2 v2 = shapeVertices[triangleIndex + 2];

        // Barycentric coordinates
        float a = 1 - Mathf.Sqrt(r1);
        float b = r1 * (1 - r2);
        float c = r2 * r1;

        // Calculate the random point inside the triangle
        Vector2 randomPoint = a * v0 + b * v1 + c * v2;

        return randomPoint;
    }

    //--------------------- Helper functions ---------------------

    // Function to convert a string seed to an integer seed
    public static int StringToSeed(string seedString)
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
