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


    


    public BoardDrawer(Transform transform, int width, int height,float size)
    {
        this.transform = transform;
        this.size = size;
        this.width = width;
        this.height = height;
        this.mesh = createSquare();
    }


    public void RenderChessPieces(List<Piece> pieceList)
    {
        foreach (Piece piece in pieceList)
        {
            CreateAndConnectGameObject(piece);
        }
    }



    public void CreateAndConnectGameObject(Piece piece)
    {
        Vector3 vec = new Vector3(piece.pos.x * size + size/2f, -piece.pos.y * size - size/2f, -1);
        GameObject gameObject = PieceDrawer.generatePieceObject(piece.pieceId,vec,piece.pieceTypeId.pieceTypeId,size);
        gameObject.transform.parent = transform.GetChild(1).transform;
        piece.gameObject =  gameObject;
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
