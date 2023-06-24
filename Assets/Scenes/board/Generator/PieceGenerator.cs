using UnityEngine;

public class PieceGenerator : MonoBehaviour
{
    private MeshGenerator meshGenerator;
    public float triangleWidth = 100f;
    public float triangleHeight = 100f;

    private void Start()
    {
        meshGenerator = new MeshGenerator(triangleWidth, triangleHeight);
        Mesh triangleMesh = meshGenerator.GeneratePolygonMesh(10);
        GetComponent<MeshFilter>().mesh = triangleMesh;
    }
}
