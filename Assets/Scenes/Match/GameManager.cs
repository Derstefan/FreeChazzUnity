using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private string playerType; //is the current player
    private bool isHotSeat;
    private bool isAutomatic;

    // Gamestate
    public GameState gameState;
    private string winner = ""; // "" means no winner

    //Frontend State


    private Piece selectedPiece;
    private Piece mouseOverPiece;
    private Pos playSelectedPos; // 2 click check für aktion

    private bool isInHistory = false;
    private bool initialLoaded = false;
    private bool isUpdating = true;
    private bool isAnimating = false;

    //cached data
    private CachedStore cachedStore = new CachedStore();



    //History
    public History history = new History();


    //technical stuff

    private Config config;
    public float size;
    public int width;
    public int height;
    private float timer = 0.0f;
    private float updateCheckInterval = 0.5f;



    private float mouseX = 0;
    private float mouseY = 0;

    //View
    public GameObject chessboard;
    public GameObject pieceView;
    public GameObject configObject;

    private BoardRenderer boardRenderer;

    private Animator animator = null;

    private SpriteRenderer historyGray;
    private MatchUiScript uiScript;

    private MatchData matchData;

    void Start()
    {
        historyGray = GameObject.Find("HistoryGray").GetComponent<SpriteRenderer>();
        uiScript = GameObject.Find("UIDocument").GetComponent<MatchUiScript>();


        string gameId = PlayerPrefs.GetString("gameId");

        uiScript.setGameId(gameId);
        matchData = Params.getMatchDataByGameId(gameId);
        if (matchData == null)
        {
            Debug.LogError("no matchData found");
            return;
        }
        config = configObject.GetComponent<Config>();

        width = matchData.width;
        height = matchData.height;
        size = config.maxBoardSize / (float)height;
        gameState = new GameState(width, height);



        isHotSeat = !matchData.p1IsBot && !matchData.p2IsBot;
        isAutomatic = matchData.p1IsBot && matchData.p2IsBot;
        gameState.turn = matchData.turns;

        for (int i = 0; i < gameState.turn; i++)
        {
            StartCoroutine(GameService.checkUpdate(gameId, i, history.Add));
        }

        gameState.nextTurn = matchData.playerTurn;
        gameState.maxLoadedTurn = matchData.maxTurns;

        uiScript.updateTurn();

        StartCoroutine(GameService.checkUpdate(gameId, gameState.turn, UpdateMatchDataWithAnimation));

    }

    void initGame(UpdateDataDTO updateDataDTO)
    {
        if (updateDataDTO == null)
        {
            Debug.LogError("no connection");
            initialLoaded = false;
        }






        boardRenderer = new BoardRenderer(chessboard.transform, width, height, size, updateDataDTO.player1.playerType);





        drawChessboard();
        gameState.turn = updateDataDTO.turn;
        gameState.nextTurn = updateDataDTO.nextTurn;
        playerType = updateDataDTO.player1.playerType;//TODO: change it for network play
        history.Add(updateDataDTO.turn, updateDataDTO);


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
        if (initialLoaded && mouseX != Input.mousePosition.x || mouseY != Input.mousePosition.y && initialLoaded)
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
        if (!gameIsOver() && initialLoaded && isUpdating && (!isAnimating || isAutomatic))
        {
            timer += Time.deltaTime;
            if (timer >= updateCheckInterval)
            {
                Debug.Log("check update " + (gameState.turn + 1));
                StartCoroutine(GameService.checkUpdate(PlayerPrefs.GetString("gameId"), gameState.turn + 1, UpdateMatchDataWithAnimation));
                timer = 0.0f; // Reset the timer
            }
        }

        // animate
        if (isAnimating && !isInHistory)
        {
            bool finished = animator.animate(Time.deltaTime);
            if (finished)
            {
                // Debug.Log("animation finished of turn " + gameState.turn);
                instantUpdatePieces(animator.updateData.pieceDTOs);
                isAnimating = false;
                animator = null;
            }
        }
    }

    private void UpdateMatchDataWithAnimation(UpdateDataDTO updateData)
    {
        if (!initialLoaded)
        {
            initGame(updateData);
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



        //history view for loading old games
        gameState.turn = updateData.turn;
        matchData.turns = updateData.turn;

        if (gameState.turn > gameState.maxLoadedTurn)
        {
            gameState.maxLoadedTurn = gameState.turn;
            history.Add(gameState.turn, updateData);
        }
        gameState.nextTurn = updateData.nextTurn;




        uiScript.updateTurn();
        if (isHotSeat)
        {
            playerType = updateData.nextTurn;
        }

        if (updateData.winner != "")
        {

            Debug.Log("winner is " + updateData.winner);
            matchData.winner = updateData.winner;
            winner = updateData.winner;
            uiScript.writeLog("winner is " + updateData.winner);
            StartCoroutine(SmoothlyChangeAlpha(0.4f, 0.5f));
        }
    }



    private void startAnimation(UpdateDataDTO updateData)
    {
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
                updatePiece(existingPiece, pDTO);
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


    private void updatePiece(Piece existingPiece, PieceDTO pDTO)
    {
        if (!existingPiece.pos.equals(pDTO.pos))
        {
            movePiece(existingPiece, pDTO.pos);
        }
        if (existingPiece.owner != pDTO.owner)
        {
            existingPiece.owner = pDTO.owner;
            boardRenderer.switchOwner(existingPiece);
        }
        if (existingPiece.pieceTypeId != pDTO.pieceTypeId)
        {
            existingPiece.pieceTypeId = pDTO.pieceTypeId;
        }
        existingPiece.showsMoveSet = true;
        existingPiece.moveSet = pDTO.moveSet;
    }



    private void loadPieceTypes()
    {
        StartCoroutine(GameService.loadpieceTypes(PlayerPrefs.GetString("gameId"), gameState.turn, updatePieceTypes));
    }

    private void updatePieceTypes(PieceTypeDTOCollection pieceTypeDTOCollection)
    {
        cachedStore.pieceTypeDTOs = pieceTypeDTOCollection.pieceTypeDTOs;

        if (selectedPiece != null)
        {
            PieceTypeDTO pieceTypeDTO = cachedStore.GetPieceTypeDTO(selectedPiece.pieceTypeId);
            if (pieceTypeDTO != null)
            {
                uiScript.showPiece(selectedPiece, cachedStore.GetPieceTypeDTO(selectedPiece.pieceTypeId));
            }
        }
    }


    private void connectPiecesToView()
    {
        foreach (Piece p in gameState.pieces)
        {
            boardRenderer.CreateAndConnectGameObject(p);
        }
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
    }

    private void addPiece(Piece p)
    {
        gameState.addPiece(p);
        boardRenderer.CreateAndConnectGameObject(p);

    }



    private void click(int x, int y)
    {
        if (isPossibleMove(x, y))
        {
            if (!isAutomatic)
            {
                play(x, y);
            }
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
    }



    private void unselectPiece(int x, int y)
    {
        boardRenderer.removePossibleMoves();
        boardRenderer.removeSelected();
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

        if (!mouseOverPiece.showsMoveSet)
        {
            return;
        }
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
        if (gameIsOver() || isAnimating || isInHistory)
        {
            return;
        }
        if (selectedPiece.owner != playerType)
        {
            return;
        }
        if (playSelectedPos == null)
        {
            playSelectedPos = new Pos(x, y);
            return;
        }
        if (!playSelectedPos.equals(new Pos(x, y)))
        {
            return;
        }

        boardRenderer.removePossibleMoves();
        boardRenderer.removeSelected();
        StartCoroutine(GameService.play(PlayerPrefs.GetString("gameId"), selectedPiece.pos, new Pos(x, y), UpdateMatchDataWithAnimation));
        unselectPiece(x, y);

        playSelectedPos = null;
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
        if (isHotSeat)
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
        if (history.get(turn) == null)
        {
            Debug.LogError("History not found for turn " + turn);
            return;
        }

        if (turn < gameState.maxLoadedTurn)
        {
            isUpdating = false;
            isInHistory = true;
            gameState.turn = turn;
            instantUpdatePieces(history.get(turn).pieceDTOs);

            StartCoroutine(SmoothlyChangeAlpha(0.4f, 0.5f));

        }
        else if (turn == gameState.maxLoadedTurn)
        {
            isUpdating = true;
            isInHistory = false;
            gameState.turn = turn;
            instantUpdatePieces(history.get(turn).pieceDTOs);

            StartCoroutine(SmoothlyChangeAlpha(0.0f, 0.5f));
        }

        uiScript.updateTurn();
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
