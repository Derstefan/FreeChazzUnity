using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDrawer : MonoBehaviour 
{

    public Mesh mesh;

    public Transform transform { get; set; }
    public float size { get; set; }
    public int width { get; set; }
    public int height { get; set; }

    private List<GameObject> possibleSquares = new List<GameObject>();
    private GameObject selectedPiece;

    private MeshGenerator meshGenerator;

    private Material vertexColorMaterial = new Material(Shader.Find("Sprites/Default"));


    public BoardDrawer(Transform transform, int width, int height,float size)
    {
        this.transform = transform;
        this.size = size;
        this.width = width;
        this.height = height;
        this.mesh = createSquare();
        this.meshGenerator = meshGenerator = new MeshGenerator(size, size);
    }


    public void RenderChessPieces(List<Piece> pieceList)
    {
        foreach (Piece piece in pieceList)
        {
            CreatePieceObject(piece);
        }
    }


     public void CreatePieceObject(Piece piece)
    {

        piece.gameObject =  generatePieceObject(transform.GetChild(1).transform,piece);
    }

    public GameObject generatePieceObject(Transform transform, Piece piece)
    {
        GameObject pieceObject = new GameObject(piece.pieceId);
        pieceObject.transform.parent = transform;
        pieceObject.transform.localPosition = new Vector3(piece.pos.x * size + size/2f, -piece.pos.y * size - size/2f, -1);
       
        int numPolygonsToAdd = 3; // Number of polygons to add
        for (int i = 0; i < numPolygonsToAdd; i++)
        {
            addPolygonToPieceObject(pieceObject,piece.pieceTypeId.pieceTypeId+i,i);
        }
        return pieceObject;
    }

    private void addPolygonToPieceObject(GameObject pieceObject,string str, int z)
    {
        GameObject polygonObject = new GameObject("Polygon");
        polygonObject.transform.parent = pieceObject.transform;
        polygonObject.transform.localPosition = new Vector3(0,0,-1-0.1f*z); // Set the position relative to the parent

        MeshFilter polygonMeshFilter = polygonObject.AddComponent<MeshFilter>();
        polygonMeshFilter.mesh = meshGenerator.GeneratePolygonMesh(6, str);
        MeshRenderer polygonMeshRenderer = polygonObject.AddComponent<MeshRenderer>();
        polygonMeshRenderer.material = vertexColorMaterial;
    }
    
    



    
    public void CreateChessboard(Material lightSquareMaterial, Material darkSquareMaterial)
    {

        Mesh mesh = createSquare();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject square = new GameObject("Square ("+i+","+j+")");
                square.transform.parent = transform.GetChild(0).transform;
                square.transform.localPosition = new Vector3(i * size, -j * size,0);

                MeshFilter meshFilter = square.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = square.AddComponent<MeshRenderer>();


                meshFilter.mesh = mesh;
                meshRenderer.material = (i + j) % 2 == 0 ? lightSquareMaterial : darkSquareMaterial;
            }
        }
    }


    

    public void drawPossibleMoves(Pos[] possibleMoves,Material color){

        
        foreach (Pos pos in possibleMoves)
        {
                possibleSquares.Add(createSquareObject("PossibleMove ("+pos.x+","+pos.y+")",transform.GetChild(0).transform, new Vector3(pos.x * size, -pos.y * size,-10.1f),color));
        }


    }

    public void drawSelected(Pos piecePos, Material color){

        selectedPiece = createSquareObject("PiecePos ("+piecePos.x+","+piecePos.y+")",transform.GetChild(0).transform, new Vector3(piecePos.x * size, -piecePos.y * size,-10.1f),color);
    }



    public void removePossibleMoves(){
        foreach (GameObject square in possibleSquares)
        {
            Destroy(square);
        }
        possibleSquares.Clear();
    }

    public void removeSelected(){
        Destroy(selectedPiece);
    }



    private GameObject createSquareObject(string name,Transform parent,Vector3 position, Material color){
                GameObject square = new GameObject(name);
                square.transform.parent = parent;
                square.transform.localPosition = position;

                MeshFilter meshFilter = square.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = square.AddComponent<MeshRenderer>();

                meshFilter.mesh = mesh;
                meshRenderer.material = color;
                return square;
    }

    private Mesh createSquare()
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
        return mesh;
    }
}
