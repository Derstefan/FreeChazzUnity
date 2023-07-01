
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



    //History


    //technical stuff
    
    public GameObject chessboard;
    public GameObject pieceView;
    public GameObject configObject;


    private Config config;
    private float size;
    private int width;
    private int height;

    private BoardDrawer drawer;
    private bool initialLoaded = false;



    private float timer = 0.0f;
    private float interval = 0.5f; 

    void Start()
    {
        config = configObject.GetComponent<Config>();
       StartCoroutine(GameService.createGame(createGame));
    }


    //first load of data
    void createGame(UpdateDataDTO updateDataDTO){

        if(updateDataDTO==null){
            Debug.Log("no connection");
            initialLoaded=false;
        }
        initialLoaded = true;

        width = updateDataDTO.width;
        height = updateDataDTO.height;
        size = 15f/(float)width;
        
        gameState = new GameState(width, height);
        
        
        drawer = new BoardDrawer(chessboard.transform, width, height, size);
        

       drawChessboard();
       UpdateMatchData(updateDataDTO);
    }




    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            int i, j;
            if (TryGetGridPosition(out i, out j))
            {
                click(i, j);
            }
        }


        timer += Time.deltaTime;

        if (timer >= interval)
        {
            if(initialLoaded){
                StartCoroutine(GameService.checkUpdate(gameState.turn+1,UpdateMatchData));
            }
            timer = 0.0f; // Reset the timer
        }
    }


    
private void UpdateMatchData(UpdateDataDTO updateData)
{
    if(updateData.pieceDTOs==null || updateData.pieceDTOs.Length == 0){
        return;
    }
    
    Debug.Log(" ------------------------------ new Board Update turn:" + updateData.turn);
    gameState.turn = updateData.turn;
    gameState.nextTurn = updateData.nextTurn;
    
    
    
    List<Piece> piecesToDestroy = new List<Piece>();
    List<Piece> piecesToAdd = new List<Piece>();



    foreach (Piece p in gameState.pieceList)
    {
        if(getPieceDTOById(p.pieceId,updateData.pieceDTOs)==null){
            piecesToDestroy.Add(p);
        }
    }

    foreach (Piece p in piecesToDestroy)
    {
        destroy(p);
    }

    foreach (PieceDTO pDTO in updateData.pieceDTOs)
    {
        Piece existingPiece = gameState.getPieceById(pDTO.pieceId);
        if (existingPiece != null)
        {
            //Update Piece values
            if(!existingPiece.pos.equals(pDTO.pos)){
                movePiece(existingPiece,pDTO.pos);
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


    // this put all positions an possibleMoves
    private void loadPieces(List<PieceDTO> pieceDTOs){

    }

    private void doDrawEvents(){
        //Events...
        //
    }

    private void doSwapEvent(){

    }

    private void doMoveEvent(){

    }

    private void doDestroyEvent(){

    }

    private void doChangeOwnerEvent(){

    }

    private void doChangeTypeEvent(){

    }







    private void addPiece(Piece p){
        gameState.addPiece(p);
        
        drawer.CreatePieceObject(p);
        //Debug.Log("added Piece " + p.pieceId + " to (" + p.pos.x +","+ p.pos.y+")");
    }

    private void movePiece(Piece p, Pos pos){
        gameState.movePiece(p,pos);

        p.gameObject.transform.localPosition = new Vector3(p.pos.x * size + size/2f, -p.pos.y * size - size/2f, -1);
        //Debug.Log("moved Piece " + p.pieceId + " to (" + pos.x +","+pos.y+")");
    }

    private void destroy(Piece p){
        
       DestroyDrawer.startDestroyAnimation(p.gameObject);
       gameState.destroy(p);

        Destroy(p.gameObject);
        //Debug.Log("destroyed Piece " + p.pieceTypeId.pieceTypeId);
    }


    private PieceDTO getPieceDTOById(string pieceId, PieceDTO[] pDTOs)
    {
        foreach (PieceDTO pDTO in pDTOs)
        {
            if (pDTO != null && pDTO.pieceId == pieceId)
            {
                return pDTO;
            }
        }
        return null;
    }





    private void click(int x,int y)
    {
            
        if(isPossibleMove(x,y)) {
            Debug.Log("possible move " + x + "," +y);
            drawer.removePossibleMoves();
            drawer.removeSelected();
            StartCoroutine(GameService.play(selectedPiece.pos,new Pos(x,y)));
            selectedPiece = null;
        }
        else if(gameState.pieces[x,y]!=null){
            Debug.Log("piece at " + x + "," +y);
            selectedPiece = gameState.pieces[x,y];
            drawer.removePossibleMoves();
            drawer.removeSelected();
            bool isOwner = selectedPiece.owner==gameState.nextTurn;
            drawer.drawPossibleMoves(selectedPiece.possibleMoves,isOwner?config.green:config.red);
            drawer.drawSelected(selectedPiece.pos,isOwner?config.green:config.red);
        }
        else {
            Debug.Log("unselect " + x + "," +y);
            drawer.removePossibleMoves();
            drawer.removeSelected();
            selectedPiece = null;
        }
    }

    private bool isPossibleMove(int x,int y){
        if(selectedPiece==null)return false;
        Pos[] possibleMoves = selectedPiece.possibleMoves;
        foreach (Pos pos in selectedPiece.possibleMoves)
        {
            if(pos.x==x && pos.y==y){
                return true;
            }
        }

        return false;
    }



  


    //-------------------------------------rest-----------------------------------


    void drawChessboard(){
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
