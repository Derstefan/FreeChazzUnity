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
    private string playerType; //is the current player

    // Gamestate
    public GameState gameState;
    private string winner = ""; // "" means no winner

    //Frontend State


    private Piece selectedPiece;
    private Piece mouseOverPiece;


    //cached data
    private CachedStore cachedStore = new CachedStore();



    //History
    private History history = new History();
    private bool isInHistory = false;


    //technical stuff

    private Config config;
    public float size;
    public int width;
    public int height;
    private float timer = 0.0f;
    private float updateCheckInterval = 0.5f;

    private bool initialLoaded = false;
    private bool isUpdating = true;

    private float mouseX = 0;
    private float mouseY = 0;

    //View
    public GameObject chessboard;
    public GameObject pieceView;
    public GameObject configObject;

    private BoardRenderer boardRenderer;
    private PieceViewRenderer pieceViewRenderer;

    private bool isAnimating = false;
    private Animator animator = null;

    private SpriteRenderer historyGray;
    private MatchUiScript uiScript;



    void Start()
    {
        config = configObject.GetComponent<Config>();
        StartCoroutine(GameService.createGame(Params.isHotSeat, initGame));
    }

    //first load of data
    void initGame(UpdateDataDTO updateDataDTO)
    {
        if (updateDataDTO == null)
        {
            Debug.LogError("no connection");
            initialLoaded = false;
        }
        width = updateDataDTO.width;
        height = updateDataDTO.height;
        size = config.maxBoardSize / (float)height;

        gameState = new GameState(width, height);

        boardRenderer = new BoardRenderer(chessboard.transform, width, height, size, updateDataDTO.player1.playerType);


        pieceViewRenderer = new PieceViewRenderer(pieceView.transform);
        historyGray = GameObject.Find("HistoryGray").GetComponent<SpriteRenderer>();
        uiScript = GameObject.Find("UIDocument").GetComponent<MatchUiScript>();


        drawChessboard();
        gameState.turn = updateDataDTO.turn;
        gameState.nextTurn = updateDataDTO.nextTurn;
        playerType = updateDataDTO.player1.playerType;//TODO: change it for network play
        history.Add(updateDataDTO);
        history.get(0);


        instantUpdatePieces(updateDataDTO.pieceDTOs);
        loadPieceTypes();
        initialLoaded = true;

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

        //check mousemove
        if (mouseX != Input.mousePosition.x || mouseY != Input.mousePosition.y && initialLoaded)
        {
            mouseX = Input.mousePosition.x;
            mouseY = Input.mousePosition.y;

            int i, j;
            if (TryGetGridPosition(out i, out j))
            {
                mouseMove(i, j);
            }
        }

        // check for updates
        if (!gameIsOver() && initialLoaded && isUpdating && !isAnimating)
        {
            timer += Time.deltaTime;
            if (timer >= updateCheckInterval)
            {
                StartCoroutine(GameService.checkUpdate(gameState.turn + 1, UpdateMatchDataWithAnimation));
                timer = 0.0f; // Reset the timer
            }
        }

        // animate
        if (isAnimating && !isInHistory)
        {
            bool finished = animator.animate(Time.deltaTime);
            if (finished)
            {
                Debug.Log("animation finished");
                instantUpdatePieces(animator.updateData.pieceDTOs);
                isAnimating = false;
                animator = null;
            }
        }
    }



    private void UpdateMatchDataWithAnimation(UpdateDataDTO updateData)
    {
        if (updateData.winner != "")
        {
            winner = updateData.winner;
            uiScript.writeLog("winner is " + updateData.winner);
            return;
        }

        if (updateData.pieceDTOs == null || updateData.pieceDTOs.Length == 0 || isAnimating)
        {
            return;
        }



        //Debug.Log(" ------------------------------ new Board Update turn:" + updateData.turn);
        if (gameState.turn + 1 == updateData.turn && updateData.drawEvent != null)
        {
            startAnimation(updateData);
        }

        gameState.turn = updateData.turn;

        if (gameState.turn > gameState.maxTurns)
        {
            gameState.maxTurns = gameState.turn;
            history.Add(updateData);
        }
        gameState.nextTurn = updateData.nextTurn;
        uiScript.updateTurn();
        if (Params.isHotSeat)
        {
            playerType = updateData.nextTurn;
        }
    }



    private void startAnimation(UpdateDataDTO updateData)
    {
        Debug.Log("startAnimation");

        animator = new Animator(updateData, this);
        isAnimating = true;

    }



    // this put all positions an possibleMoves
    private void instantUpdatePieces(PieceDTO[] newPieceDTOs)
    {
        List<Piece> piecesToDestroy = new List<Piece>();
        List<Piece> piecesToAdd = new List<Piece>();

        foreach (Piece p in gameState.pieceList)
        {
            if (Util.getPieceDTOById(p.pieceId, newPieceDTOs) == null)
            {
                piecesToDestroy.Add(p);
            }
        }

        foreach (Piece p in piecesToDestroy)
        {
            destroy(p);
        }

        foreach (PieceDTO pDTO in newPieceDTOs)
        {
            Piece existingPiece = gameState.getPieceById(pDTO.pieceId);
            if (existingPiece != null)
            {

                //Update Piece values
                if (!existingPiece.pos.equals(pDTO.pos))
                {
                    movePiece(existingPiece, pDTO.pos);
                }
                existingPiece.moveSet = pDTO.moveSet;
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


    private void addPiece(Piece p)
    {
        gameState.addPiece(p);
        boardRenderer.CreateAndConnectGameObject(p);
    }

    private void movePiece(Piece p, Pos pos)
    {
        Pos oldPos = new Pos(p.pos.x, p.pos.y);

        gameState.movePiece(p, new Pos(pos.x, pos.y));
        p.gameObject.transform.localPosition = new Vector3(
            p.pos.x * size + size / 2f,
            -p.pos.y * size - size / 2f,
            -1
        );
    }

    private void destroy(Piece p)
    {
        gameState.destroy(p);
        Destroy(p.gameObject);
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



    private void selectPiece(int x, int y)
    {
        selectedPiece = gameState.pieces[x, y];
        boardRenderer.removePossibleMoves();
        boardRenderer.removeSelected();
        bool isOwner = selectedPiece.owner == playerTurn();
        boardRenderer.drawPossibleMoves(
            selectedPiece.moveSet, isOwner);
        boardRenderer.drawSelected(selectedPiece.pos, isOwner);

        //check if pieceType needs to load
        if (cachedStore.GetPieceTypeDTO(selectedPiece.pieceTypeId) == null)
        {
            loadPieceTypes();
            return;
        }
        uiScript.showPiece(selectedPiece, cachedStore.GetPieceTypeDTO(selectedPiece.pieceTypeId));
        pieceViewRenderer.render(selectedPiece, cachedStore.GetPieceTypeDTO(selectedPiece.pieceTypeId));
    }

    private void unselectPiece(int x, int y)
    {
        boardRenderer.removePossibleMoves();
        boardRenderer.removeSelected();
        pieceViewRenderer.hide();
        uiScript.hidePiece();
        selectedPiece = null;
    }



    private void mouseMove(int x, int y)
    {
        if (gameState.getPiece(x, y) != null)
        {
            mouseEnterPiece(x, y);
        }
        else
        {
            mouseLeavePiece(x, y);
        }
    }


    private void mouseEnterPiece(int x, int y)
    {
        mouseOverPiece = gameState.pieces[x, y];
        boardRenderer.removeMouseOverPossibleMoves();
        boardRenderer.removeMouseOverSelected();
        bool isOwner = mouseOverPiece.owner == playerTurn();

        boardRenderer.drawMouseOverPossibleMoves(
            mouseOverPiece.moveSet,
            isOwner
        );
        boardRenderer.drawMouseOverSelected(mouseOverPiece.pos, isOwner);
    }


    private void mouseLeavePiece(int x, int y)
    {
        boardRenderer.removeMouseOverPossibleMoves();
        boardRenderer.removeMouseOverSelected();
        mouseOverPiece = null;
    }



    private void play(int x, int y)
    {
        if (gameIsOver() || isAnimating)
        {
            return;
        }
        if (selectedPiece.owner != playerType)
        {
            return;
        }

        boardRenderer.removePossibleMoves();
        boardRenderer.removeSelected();
        StartCoroutine(GameService.play(selectedPiece.pos, new Pos(x, y), UpdateMatchDataWithAnimation));
        unselectPiece(x, y);

    }









    //------------------------------------- Helper Functions -----------------------------------


    void drawChessboard()
    {
        boardRenderer.CreateChessboard(config.lightSquareMaterial, config.darkSquareMaterial);
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







    // ------------------------------- GETTER ----------------------------------

    public Piece getPieceById(string pieceId)
    {
        for (int i = 0; i < gameState.pieceList.Count; i++)
        {
            Piece piece = gameState.pieceList[i];
            if (piece.pieceId == pieceId)
            {
                return piece;
            }
        }
        return null;
    }

    public Piece getPieceByPos(Pos pos)
    {
        return gameState.pieces[pos.x, pos.y];
    }

    private bool gameIsOver()
    {
        return winner != "";
    }

    private string playerTurn()
    {
        if (Params.isHotSeat)
        {
            return gameState.nextTurn;
        }
        return playerType;
    }

    private bool isPossibleMove(int x, int y)
    {
        if (selectedPiece == null)
        {
            return false;
        }
        if (selectedPiece.moveSet == null)
        {
            return false;
        }
        if (selectedPiece.moveSet.possibleMoves == null)
        {
            return false;
        }

        foreach (Pos pos in selectedPiece.moveSet.possibleMoves)
        {
            if (pos.x == x && pos.y == y)
            {
                return true;
            }
        }

        return false;
    }



    // ------------------------------  History Functions ------------------------------

    public void loadTurn(int turn)
    {
        if (turn < gameState.maxTurns)
        {
            isUpdating = false;
            isInHistory = true;
            gameState.turn = turn;
            instantUpdatePieces(history.get(turn).pieceDTOs);

            StartCoroutine(SmoothlyChangeAlpha(0.4f, 0.5f));

        }
        else if (turn == gameState.maxTurns)
        {
            isUpdating = true;
            isInHistory = false;
            gameState.turn = turn;
            instantUpdatePieces(history.get(turn).pieceDTOs);
            StartCoroutine(SmoothlyChangeAlpha(0.0f, 0.5f));

        }
    }

    private IEnumerator SmoothlyChangeAlpha(float targetAlpha, float duration)
    {
        Color startColor = historyGray.color;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            Color lerpedColor = historyGray.color;
            lerpedColor.a = Mathf.Lerp(startColor.a, targetAlpha, t);
            historyGray.color = lerpedColor;
            yield return null;
        }

        Color finalColor = historyGray.color;
        finalColor.a = targetAlpha;
        historyGray.color = finalColor;
    }
}
