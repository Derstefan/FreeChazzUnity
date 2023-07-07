using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //data about me
    private string userid;
    private string token;

    //data about the game
    private string gameId;

    // Gamestate
    private GameState gameState;

    //Frontend State


    private Piece selectedPiece;
    private List<Vector2> possibleMoves = new List<Vector2>();

    //cached data
    private CachedStore cachedStore = new CachedStore();



    //History


    //technical stuff

    private Config config;
    private float size;
    private int width;
    private int height;
    private float timer = 0.0f;
    private float interval = 0.5f;

    private bool initialLoaded = false;

    //View
    public GameObject chessboard;
    public GameObject pieceView;
    public GameObject configObject;

    private BoardDrawer drawer;
    private PieceViewRenderer pieceViewRenderer;



    

    void Start()
    {
        config = configObject.GetComponent<Config>();
        StartCoroutine(GameService.createGame(initGame));
    }

    //first load of data
    void initGame(UpdateDataDTO updateDataDTO)
    {
        if (updateDataDTO == null)
        {
            Debug.Log("no connection");
            initialLoaded = false;
        }
        initialLoaded = true;
        width = updateDataDTO.width;
        height = updateDataDTO.height;
        size = config.maxBoardSize / (float)height;

        gameState = new GameState(width, height);

        drawer = new BoardDrawer(chessboard.transform, width, height, size);
        pieceViewRenderer = new PieceViewRenderer(pieceView.transform);

        drawChessboard();
        UpdateMatchData(updateDataDTO);
        loadPieceTypes();
    }

    void Update()
    {
        //check click input
        if (Input.GetButtonDown("Fire1"))
        {
            int i,
                j;
            if (TryGetGridPosition(out i, out j))
            {
                click(i, j);
            }
        }
        
        // check for updates
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            if (initialLoaded)
            {
                StartCoroutine(GameService.checkUpdate(gameState.turn + 1, UpdateMatchData));
            }
            timer = 0.0f; // Reset the timer
        }
    }

    private void UpdateMatchData(UpdateDataDTO updateData)
    {
        if (updateData.pieceDTOs == null || updateData.pieceDTOs.Length == 0)
        {
            return;
        }

        //Debug.Log(" ------------------------------ new Board Update turn:" + updateData.turn);
        gameState.turn = updateData.turn;
        if (gameState.turn > gameState.maxTurns)
        {
            gameState.maxTurns = gameState.turn;
        }
        gameState.nextTurn = updateData.nextTurn;


        if(updateData.drawEvent!=null)
        {
            Debug.Log(JsonUtility.ToJson(updateData.drawEvent));
        }

        //TODO move somewhere else
        loadPieces(updateData.pieceDTOs);
        
    }



    // this put all positions an possibleMoves
    private void loadPieces(PieceDTO[] pieceDTOs)
    {
        List<Piece> piecesToDestroy = new List<Piece>();
        List<Piece> piecesToAdd = new List<Piece>();

        foreach (Piece p in gameState.pieceList)
        {
            if (Util.getPieceDTOById(p.pieceId, pieceDTOs) == null)
            {
                piecesToDestroy.Add(p);
            }
        }

        foreach (Piece p in piecesToDestroy)
        {
            destroy(p);
        }

        foreach (PieceDTO pDTO in pieceDTOs)
        {
            Piece existingPiece = gameState.getPieceById(pDTO.pieceId);
            if (existingPiece != null)
            {
                //Update Piece values
                if (!existingPiece.pos.equals(pDTO.pos))
                {
                    movePiece(existingPiece, pDTO.pos);
                }

                existingPiece.possibleMoves = pDTO.possibleMoves;
            }
            else
            {
                piecesToAdd.Add(new Piece(pDTO));
            }
        }

        foreach (Piece p in piecesToAdd)
        {
            addPiece(p);
        }
    }


    private void loadPieceTypes()
    {
        StartCoroutine(GameService.loadpieceTypes(gameState.turn, updatePieceTypes));
    }

    private void updatePieceTypes(PieceTypeDTOCollection pieceTypeDTOCollection)
    {
        cachedStore.pieceTypeDTOs = pieceTypeDTOCollection.pieceTypeDTOs;

        if (selectedPiece != null)
        {
            PieceTypeDTO pieceTypeDTO = cachedStore.GetPieceTypeDTO(selectedPiece.pieceTypeId);
            if (pieceTypeDTO != null)
            {
                pieceViewRenderer.render(selectedPiece, pieceTypeDTO);
            }
        }
    }


    private void doDrawEvents()
    {
        //Events...
        //
    }

    private void doSwapEvent() { }

    private void doMoveEvent() { }

    private void doDestroyEvent() { }

    private void doChangeOwnerEvent() { }

    private void doChangeTypeEvent() { }

    private void addPiece(Piece p)
    {
        gameState.addPiece(p);

        drawer.CreateAndConnectGameObject(p);
        //Debug.Log("added Piece " + p.pieceId + " to (" + p.pos.x +","+ p.pos.y+")");
    }

    private void movePiece(Piece p, Pos pos)
    {
        gameState.movePiece(p, pos);

        p.gameObject.transform.localPosition = new Vector3(
            p.pos.x * size + size / 2f,
            -p.pos.y * size - size / 2f,
            -1
        );
        //Debug.Log("moved Piece " + p.pieceId + " to (" + pos.x +","+pos.y+")");
    }

    private void destroy(Piece p)
    {
        DestroyDrawer.startDestroyAnimation(p.gameObject, 1.4f, 3.0f);
        gameState.destroy(p);

        Destroy(p.gameObject);
        //Debug.Log("destroyed Piece " + p.pieceTypeId.pieceTypeId);
    }



    private void click(int x, int y)
    {
        if (isPossibleMove(x, y))
        {
            play(x, y);
        }
        else if (gameState.pieces[x, y] != null)
        {
            selectPiece(x, y);
        }
        else
        {
            unselectPiece(x, y);
        }
    }

    private void play(int x, int y)
    {
        drawer.removePossibleMoves();
        drawer.removeSelected();
        StartCoroutine(GameService.play(selectedPiece.pos, new Pos(x, y)));
        unselectPiece(x, y);
    }

    private void selectPiece(int x, int y)
    {
        selectedPiece = gameState.pieces[x, y];
        drawer.removePossibleMoves();
        drawer.removeSelected();
        bool isOwner = selectedPiece.owner == gameState.nextTurn;
        drawer.drawPossibleMoves(
            selectedPiece.possibleMoves,
            isOwner ? config.green : config.red
        );
        drawer.drawSelected(selectedPiece.pos, isOwner ? config.green : config.red);

        //check if pieceType needs to load
        if (cachedStore.GetPieceTypeDTO(selectedPiece.pieceTypeId) == null)
        {
            loadPieceTypes();
            return;
        }
        pieceViewRenderer.render(selectedPiece, cachedStore.GetPieceTypeDTO(selectedPiece.pieceTypeId));
    }

    private void unselectPiece(int x,int y)
    {
        drawer.removePossibleMoves();
        drawer.removeSelected();
        pieceViewRenderer.hide();
        selectedPiece = null;
    }

    private bool isPossibleMove(int x, int y)
    {
        if (selectedPiece == null)
            return false;
        Pos[] possibleMoves = selectedPiece.possibleMoves;
        foreach (Pos pos in selectedPiece.possibleMoves)
        {
            if (pos.x == x && pos.y == y)
            {
                return true;
            }
        }

        return false;
    }

    //-------------------------------------rest-----------------------------------


    void drawChessboard()
    {
        drawer.CreateChessboard(config.lightSquareMaterial, config.darkSquareMaterial);
    }

    private bool TryGetGridPosition(out int i, out int j)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        i = Mathf.FloorToInt(worldPos.x / size);
        j = Mathf.FloorToInt(-worldPos.y / size);
        if (i >= 0 && i < width && j >= 0 && j < height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
