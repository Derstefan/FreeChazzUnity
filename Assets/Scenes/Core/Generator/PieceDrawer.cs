using System.Collections.Generic;
using UnityEngine;

public class PieceDrawer
{

    private static Material vertexColorMaterial = new Material(Shader.Find("Sprites/Default"));


    public static void render(Transform transform, Piece piece, int size)
    {
        GameObject pieceObject = generatePieceObject(piece.pieceId, Vector3.zero, piece.pieceTypeId.pieceTypeId, size);
        pieceObject.transform.parent = transform;
        pieceObject.transform.localPosition = new Vector3(+size / 2, -size / 2, -1);
    }

    public static GameObject generatePieceObject(string name, Vector3 vec, string seed, float size)
    {
        GameObject pieceObject = new GameObject(name);
        pieceObject.transform.localPosition = vec;

        int numPolygonsToAdd = 3; // Number of polygons to add
        for (int i = 0; i < numPolygonsToAdd; i++)
        {
            addPolygonToPieceObject(pieceObject, seed + i, i, size, size);
        }
        return pieceObject;
    }



    private static void addPolygonToPieceObject(GameObject pieceObject, string str, int z, float width, float height)
    {
        GameObject polygonObject = new GameObject("Polygon");
        polygonObject.transform.parent = pieceObject.transform;
        polygonObject.transform.localPosition = new Vector3(0, 0, -1 - 0.1f * z); // Set the position relative to the parent

        MeshFilter polygonMeshFilter = polygonObject.AddComponent<MeshFilter>();
        polygonMeshFilter.mesh = MeshGenerator.GeneratePolygonMesh(6, str, width, height);
        MeshRenderer polygonMeshRenderer = polygonObject.AddComponent<MeshRenderer>();
        polygonMeshRenderer.material = vertexColorMaterial;
    }


    public static GameObject generateTest(string name, Vector3 vec)
    {
        GameObject pieceObject = new GameObject(name);
        pieceObject.transform.localPosition = vec;
        MeshFilter polygonMeshFilter = pieceObject.AddComponent<MeshFilter>();
        MeshRenderer polygonMeshRenderer = pieceObject.AddComponent<MeshRenderer>();
        polygonMeshRenderer.material = vertexColorMaterial;




        List<Vector3> positions = new List<Vector3>
        {
            new Vector3(1.0f, 2.0f, 3.0f),
            new Vector3(4.0f, 5.0f, 6.0f),
            new Vector3(7.0f, 8.0f, 9.0f),
            new Vector3(10.0f, 11.0f, 12.0f),
            new Vector3(13.0f, 14.0f, 15.0f)
        };

        List<bool> roundedCorners = new List<bool>
        {
            true,
            false,
            true,
            false,
            true
        };
        polygonMeshFilter.mesh = MeshGenerator.GenerateRoundedMesh(positions, roundedCorners);

        return pieceObject;
    }
}
