using UnityEngine;

public class Testing : MonoBehaviour
{
    private MeshGenerator meshGenerator;
    public float triangleWidth = 100f;
    public float triangleHeight = 100f;

    private void Update()
    {
        Debug.Log("Testing");

        meshGenerator = new MeshGenerator(triangleWidth, triangleHeight);
        Mesh triangleMesh = meshGenerator.GeneratePolygonMesh(10);

        GetComponent<MeshFilter>().mesh = triangleMesh;
    }
}
