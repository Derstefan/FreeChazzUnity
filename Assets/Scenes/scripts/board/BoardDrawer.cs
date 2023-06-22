using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDrawer : MonoBehaviour 
{

    public Transform transform { get; set; }
    public float size { get; set; }
    public int width { get; set; }
    public int height { get; set; }

    private List<GameObject> possibleSquares = new List<GameObject>();


    public BoardDrawer(Transform transform, int width, int height,float size)
    {
        this.transform = transform;
        this.size = size;
        this.width = width;
        this.height = height;
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
        GameObject pieceObject = new GameObject(piece.symbol + ": " + piece.id);
        pieceObject.transform.parent = transform;
        pieceObject.transform.localPosition = new Vector3(piece.pos.x * size + size/2f, -piece.pos.y * size - size/2f, 0);

        TextMesh textMesh = pieceObject.AddComponent<TextMesh>();
        textMesh.text = piece.symbol;
        textMesh.characterSize = 0.2f; // Adjust the size of the text
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.fontSize = (int)(size*50); // Adjust the font size
        textMesh.color = Color.black; // Adjust the color of the text

        piece.gameObject =  pieceObject;
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


    

    public void drawPossibleMoves(Pos piecePos,Pos[] possibleMoves,Material color){

        Mesh mesh = createSquare();
        foreach (Pos pos in possibleMoves)
        {
                GameObject square = new GameObject("PossibleMove ("+pos.x+","+pos.y+")");
                square.transform.parent = transform.GetChild(0).transform;
                square.transform.localPosition = new Vector3(pos.x * size, -pos.y * size,-2.1f);

                MeshFilter meshFilter = square.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = square.AddComponent<MeshRenderer>();

                meshFilter.mesh = mesh;
                meshRenderer.material = color;
                possibleSquares.Add(square);
        }


                GameObject square2 = new GameObject("PiecePos ("+piecePos.x+","+piecePos.y+")");
                square2.transform.parent = transform.GetChild(0).transform;
                square2.transform.localPosition = new Vector3(piecePos.x * size, -piecePos.y * size,-2.1f);

                MeshFilter meshFilter2 = square2.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer2 = square2.AddComponent<MeshRenderer>();

                meshFilter2.mesh = mesh;
                meshRenderer2.material = color;
                possibleSquares.Add(square2);


    }

    public void removePossibleMoves(){
        foreach (GameObject square in possibleSquares)
        {
            Destroy(square);
        }
        possibleSquares.Clear();
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
