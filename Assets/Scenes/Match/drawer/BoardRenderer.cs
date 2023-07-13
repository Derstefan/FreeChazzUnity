using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardRenderer : MonoBehaviour 
{

    public Mesh mesh;

    private Transform transform { get; set; }
    private float size { get; set; }
    private int width { get; set; }
    private int height { get; set; }

    private List<GameObject> possibleSquares = new List<GameObject>();
    private GameObject selectedPiece;

    private List<GameObject> mouseOverpossibleSquares = new List<GameObject>();
    private GameObject mouseOverPiece;

    private string player;


    private static Material vertexColorMaterial = new Material(Shader.Find("Sprites/Default"));

    private GameObject aura1;
    private GameObject aura2;


    public BoardRenderer(Transform transform, int width, int height,float size, string player)
    {
        this.transform = transform;
        this.size = size;
        this.width = width;
        this.height = height;
        this.mesh = createSquare();
        this.player = player;

        this.aura1 = createOwnerAura(size, Color.black);
        aura1.SetActive(false);
        this.aura2 = createOwnerAura(size, Color.white);
        aura2.SetActive(false);
    }


    //Render all Pieces and conenct them to the Piece object
    public void RenderChessPieces(List<Piece> pieceList)
    {
        foreach (Piece piece in pieceList)
        {
            CreateAndConnectGameObject(piece);
        }
    }


    //Create a GameObject for a Piece and connect it to the Piece object
    public void CreateAndConnectGameObject(Piece piece)
    {
        Vector3 vec = new Vector3(piece.pos.x * size + size/2f, -piece.pos.y * size - size/2f, -1);
        GameObject gameObject = PieceDrawer.generatePieceObject(piece.pieceId,vec,piece.pieceTypeId.pieceTypeId,size);
        gameObject.transform.parent = transform.GetChild(1).transform;

        GameObject ownerAura = piece.owner == player ? Instantiate(aura2) : Instantiate(aura1);
        ownerAura.SetActive(true);
        ownerAura.transform.parent = gameObject.transform;
        ownerAura.transform.localPosition = Vector3.zero;


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


    

    public void drawPossibleMoves(MoveSet moveSet, bool isOwner)
    {
        foreach (Pos pos in moveSet.possibleMoves)
        {
                possibleSquares.Add(createSquareObject("PossibleMove ("+pos.x+","+pos.y+")",transform.GetChild(0).transform, new Vector3(pos.x * size, -pos.y * size,-10.1f),pos.tag));
        }
    }

    public void drawSelected(Pos piecePos, bool isOwner)
    {
        selectedPiece = createSquareObject("PiecePos ("+piecePos.x+","+piecePos.y+")",transform.GetChild(0).transform, new Vector3(piecePos.x * size, -piecePos.y * size,-10.1f),piecePos.tag);
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



    public void drawMouseOverPossibleMoves(MoveSet moveSet,bool isOwner)
    {
        foreach (Pos pos in moveSet.possibleMoves)
        {
            mouseOverpossibleSquares.Add(createSquareObject("PossibleMove (" + pos.x + "," + pos.y + ")", transform.GetChild(0).transform, new Vector3(pos.x * size, -pos.y * size, -10.1f), pos.tag));
        }
    }

    public void drawMouseOverSelected(Pos piecePos, bool isOwner)
    {
        mouseOverPiece = createSquareObject("PiecePos (" + piecePos.x + "," + piecePos.y + ")", transform.GetChild(0).transform, new Vector3(piecePos.x * size, -piecePos.y * size, -10.1f), piecePos.tag);
    }

    public void removeMouseOverPossibleMoves()
    {
        foreach (GameObject square in mouseOverpossibleSquares)
        {
            Destroy(square);
        }
        mouseOverpossibleSquares.Clear();
    }

    public void removeMouseOverSelected()
    {
        Destroy(mouseOverPiece);
    }



    private GameObject createSquareObject(string name,Transform parent,Vector3 position,string type){
                GameObject square = new GameObject(name);
                square.transform.parent = parent;
                square.transform.localPosition = position;

                MeshFilter meshFilter = square.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = square.AddComponent<MeshRenderer>();

                meshFilter.mesh = mesh;
                meshRenderer.material = RenderUtil.getMaterialByType(type);
                Debug.Log(type);
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


    private GameObject createOwnerAura(float size, Color color)
    {
        // Create a new empty GameObject to hold the parent
        GameObject parentObject = new GameObject("SquareMeshParent");

        // Calculate half size to position vertices correctly
        float halfSize = size * 0.5f;

        // Define the positions of the four squares
        Vector3[] squarePositions = new Vector3[4];
        squarePositions[0] = new Vector3(-halfSize * 0.25f, -halfSize * 0.25f, 0f);
        squarePositions[1] = new Vector3(-halfSize * 0.25f, halfSize * 0.25f, 0f);
        squarePositions[2] = new Vector3(halfSize * 0.25f, halfSize * 0.25f, 0f);
        squarePositions[3] = new Vector3(halfSize * 0.25f, -halfSize * 0.25f, 0f);

        for (int i = 0; i < 4; i++)
        {
            // Create a new empty GameObject to hold the mesh
            GameObject meshObject = new GameObject("SquareMesh" + i);

            // Add required components
            meshObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = vertexColorMaterial;

            // Create a new mesh
            Mesh mesh = new Mesh();

            // Define vertices for the smaller square
            Vector3[] vertices = new Vector3[4];
            vertices[0] = new Vector3(squarePositions[i].x - halfSize * 0.5f, squarePositions[i].y - halfSize * 0.5f, 0f);
            vertices[1] = new Vector3(squarePositions[i].x - halfSize * 0.5f, squarePositions[i].y + halfSize * 0.5f, 0f);
            vertices[2] = new Vector3(squarePositions[i].x + halfSize * 0.5f, squarePositions[i].y + halfSize * 0.5f, 0f);
            vertices[3] = new Vector3(squarePositions[i].x + halfSize * 0.5f, squarePositions[i].y - halfSize * 0.5f, 0f);

            // Define triangles
            int[] triangles = { 0, 1, 2, 0, 2, 3 };

            // Define UVs
            Vector2[] uv = new Vector2[4];
            uv[0] = new Vector2(0f, 0f);
            uv[1] = new Vector2(0f, 1f);
            uv[2] = new Vector2(1f, 1f);
            uv[3] = new Vector2(1f, 0f);

            // Define colors with transparency
            Color[] colors = new Color[4];
            colors[0] = new Color(color.r, color.g, color.b, i == 2 ? 1f : 0.3f);   // Center vertex (no transparency)
            colors[1] = new Color(color.r, color.g, color.b, i == 3 ? 1f : 0.3f);   // Top-left vertex (transparent)
            colors[2] = new Color(color.r, color.g, color.b, i == 0 ? 1f : 0.3f);   // Top-right vertex (transparent)
            colors[3] = new Color(color.r, color.g, color.b, i == 1 ? 1f : 0.3f);   // Bottom-right vertex (transparent)

            // Assign vertices, triangles, UVs, and colors to the mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.colors = colors;

            // Update mesh properties
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            // Assign the created mesh to the mesh filter component
            meshObject.GetComponent<MeshFilter>().mesh = mesh;

            // Set the position of the smaller square
            meshObject.transform.position = squarePositions[i];

            // Make the smaller square a child of the parent GameObject
            meshObject.transform.parent = parentObject.transform;
        }

        return parentObject;
    }
}
