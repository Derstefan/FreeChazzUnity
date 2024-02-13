using System.Collections.Generic;
using Assets.Scenes.Match.drawer;
using UnityEngine;

public class BoardRenderer : MonoBehaviour
{


    public static float Z_BOARD = 3f;
    public static float Z_PIECE = -10f;
    public static float Z_AURA = -1f; //relative from the Z_PIECE
    public static float Z_POSSIBLE_MOVE = -5f;
    public static float Z_SELECTED_MOVE_SMALL_SQUARE = -1f; //relative from the Z_POSSIBLE_MOVE

    private Mesh mesh;
    private Mesh meshSmall;
    private float smallScale = 0.6f;


    private Transform transform { get; set; }
    private float size { get; set; }
    private int width { get; set; }
    private int height { get; set; }

    private List<GameObject> possibleSquares = new List<GameObject>();
    private GameObject selectedPiece;

    private List<GameObject> mouseOverpossibleSquares = new List<GameObject>();
    private GameObject mouseOverPiece;

    private string player;


    private static Material vertexColorMaterial = RenderUtil.DEFAULT_MATERIAL;

    public GameObject aura;
    public GameObject auraEnemy;


    public BoardRenderer(Transform transform, int width, int height, float size, string player)
    {
        this.transform = transform;
        this.size = size;
        this.width = width;
        this.height = height;
        this.mesh = createSquare();
        this.meshSmall = createSquareSmall();
        this.player = player;

        this.aura = createOwnerAura(size, Color.black);
        this.auraEnemy = createOwnerAura(size, new Color(0.8f, 0.1f, 0.1f));
        aura.SetActive(false);
        auraEnemy.SetActive(false);
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
        Vector3 vec = new Vector3(piece.pos.x * size + size / 2f, -piece.pos.y * size - size / 2f, Z_PIECE);
        GameObject gameObject = PieceRenderer.createPieceObject(piece.pieceId, vec, piece.pieceTypeId, size, piece.owner == "P1");
        gameObject.transform.parent = transform.GetChild(1).transform;

        GameObject shaddow = piece.owner == player ? Instantiate(aura) : Instantiate(auraEnemy);
        shaddow.SetActive(true);
        shaddow.transform.parent = gameObject.transform;
        shaddow.transform.localPosition = new Vector3(0f, 0f, Z_AURA);


        // Check if the piece is a king and add a shadow at the bottom
        if (piece.isKing)
        {
            Debug.Log("King");
            GameObject shadow = Instantiate(aura); // Assuming the aura prefab is suitable for shadow
            shadow.SetActive(true);
            shadow.transform.parent = gameObject.transform;
            shadow.transform.localPosition = new Vector3(0f, -size / 2f, Z_AURA); // Position at the bottom
        }

        piece.gameObject = gameObject;
    }


    public void switchOwner(Piece piece)
    {
        //Destroy the old aura
        Destroy(piece.gameObject.transform.GetChild(0).gameObject);
        //Create the new aura
        GameObject shaddow = piece.owner == player ? Instantiate(aura) : Instantiate(auraEnemy);
        shaddow.SetActive(true);
        shaddow.transform.parent = piece.gameObject.transform;
        shaddow.transform.localPosition = new Vector3(0f, 0f, Z_AURA);
    }











    public void CreateChessboard(Material lightSquareMaterial, Material darkSquareMaterial)
    {

        Mesh mesh = createSquare();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject square = new GameObject("Square (" + i + "," + j + ")");
                square.transform.parent = transform.GetChild(0).transform;
                square.transform.localPosition = new Vector3(i * size, -j * size, Z_BOARD);

                MeshFilter meshFilter = square.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = square.AddComponent<MeshRenderer>();


                meshFilter.mesh = mesh;
                meshRenderer.material = (i + j) % 2 == 0 ? lightSquareMaterial : darkSquareMaterial;
            }
        }
    }




    public void drawPossibleMoves(MoveSet moveSet, bool isOwner)
    {
        foreach (ActionPos pos in moveSet.possibleMoves)
        {
            possibleSquares.Add(createPossibleMoveSquare("PossibleMove (" + pos.x + "," + pos.y + ")", transform.GetChild(0).transform, new Vector3(pos.x * size, -pos.y * size, Z_POSSIBLE_MOVE), pos.tag, isOwner));
        }
    }

    public void drawSelected(Pos piecePos, bool isOwner)
    {
        selectedPiece = createPossibleMoveSquare("PiecePos (" + piecePos.x + "," + piecePos.y + ")", transform.GetChild(0).transform, new Vector3(piecePos.x * size, -piecePos.y * size, Z_POSSIBLE_MOVE), "TODO:tag", isOwner);
    }

    public void removePossibleMoves()
    {
        foreach (GameObject square in possibleSquares)
        {
            Destroy(square);
        }
        possibleSquares.Clear();
    }

    public void removeSelected()
    {
        Destroy(selectedPiece);
    }



    public void drawMouseOverPossibleMoves(MoveSet moveSet, bool isOwner)
    {
        foreach (ActionPos pos in moveSet.possibleMoves)
        {
            mouseOverpossibleSquares.Add(createPossibleMoveSquare("PossibleMove (" + pos.x + "," + pos.y + ")", transform.GetChild(0).transform, new Vector3(pos.x * size, -pos.y * size, Z_POSSIBLE_MOVE), pos.tag, isOwner));
        }
    }

    public void drawMouseOverSelected(Pos piecePos, bool isOwner)
    {
        mouseOverPiece = createPossibleMoveSquare("PiecePos (" + piecePos.x + "," + piecePos.y + ")", transform.GetChild(0).transform, new Vector3(piecePos.x * size, -piecePos.y * size, Z_POSSIBLE_MOVE), "TODO:tag", isOwner);
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




    private GameObject createPossibleMoveSquare(string name, Transform parent, Vector3 position, string type, bool isOwner)
    {
        GameObject parentSquare = new GameObject(name);
        parentSquare.transform.parent = parent;
        parentSquare.transform.localPosition = position;


        //actioncolor square
        GameObject square = new GameObject(name);
        square.transform.parent = parentSquare.transform;
        square.transform.localPosition = new Vector3((1f - smallScale) * size * 0.5f, -(1f - smallScale) * size * 0.5f, Z_SELECTED_MOVE_SMALL_SQUARE);

        MeshFilter meshFilter = square.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = square.AddComponent<MeshRenderer>();

        meshFilter.mesh = meshSmall;
        meshRenderer.material = RenderUtil.getMaterialByType(type);


        GameObject square2 = new GameObject(name);
        square2.transform.parent = parentSquare.transform;
        square2.transform.localPosition = Vector3.zero;

        MeshFilter meshFilter2 = square2.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer2 = square2.AddComponent<MeshRenderer>();

        meshFilter2.mesh = mesh;
        meshRenderer2.material = isOwner ? RenderUtil.green : RenderUtil.red;

        return parentSquare;

    }

    private GameObject createSquareObject(string name, Transform parent, Vector3 position, string type)
    {
        GameObject square = new GameObject(name);
        square.transform.parent = parent;
        square.transform.localPosition = position;

        MeshFilter meshFilter = square.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = square.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;
        meshRenderer.material = RenderUtil.getMaterialByType(type);
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

    private Mesh createSquareSmall()
    {

        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, -1f*size*smallScale,0),
            new Vector3(1f*size * smallScale, -1f*size * smallScale,0),
            new Vector3(1f*size * smallScale, 0,0),
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
            colors[0] = new Color(color.r, color.g, color.b, i == 2 ? 0.6f : 0.1f);   // Center vertex (no transparency)
            colors[1] = new Color(color.r, color.g, color.b, i == 3 ? 0.6f : 0.1f);   // Top-left vertex (transparent)
            colors[2] = new Color(color.r, color.g, color.b, i == 0 ? 0.6f : 0.1f);   // Top-right vertex (transparent)
            colors[3] = new Color(color.r, color.g, color.b, i == 1 ? 0.6f : 0.1f);   // Bottom-right vertex (transparent)

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


    /*
    private GameObject createOwnerAura(float size, Color color)
    {

        int pixelSize = 128;
        // Create a new texture
        Texture2D texture = new Texture2D(pixelSize, pixelSize);

        // Draw a black square in the center
        int borderWidth = 10; // Adjust the size of the transparent border
        for (int x = borderWidth; x < pixelSize - borderWidth; x++)
        {
            for (int y = borderWidth; y < pixelSize - borderWidth; y++)
            {
                float diffFromCenter = 1f - Mathf.Abs(x - pixelSize * 0.5f) / (pixelSize * 0.5f);

                texture.SetPixel(x, y, new Color(0, 0, 0, diffFromCenter));
            }
        }

        texture.Apply();

        // Create a Sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 10);

        // Create a GameObject with a SpriteRenderer
        GameObject squareObject = new GameObject("SquareMeshParent");
        squareObject.transform.position = Vector3.zero; // Set the position as needed

        SpriteRenderer spriteRenderer = squareObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        return squareObject;
    }

    */
}
