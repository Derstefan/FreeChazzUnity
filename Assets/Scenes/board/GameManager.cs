
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

    
    private int selectedX = -1; //-1,-1 means there is no selection
    private int selectedY = -1;
    private List<Vector2> possibleMoves = new List<Vector2>();



    //History


    //technical stuff
    
    public GameObject chessboard;
    public Material lightSquareMaterial;
    public Material darkSquareMaterial; 
    public Material green;
    public Material red; 
    public GameObject pieceView;
    private float size;
    private int width;
    private int height;

    private BoardDrawer drawer;



    private float timer = 0.0f;
    private float interval = 2.5f; // 200ms


    void Start()
    {
       StartCoroutine(GameService.createGame(createGame));
    }


    //first load of data
    void createGame(UpdateDataDTO updateDataDTO){

    if(updateDataDTO==null){
        Debug.Log("no connection");
    }

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
            StartCoroutine(GameService.checkUpdate(gameState.turn+1,UpdateMatchData));
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

    foreach (PieceDTO pDTO in updateData.pieceDTOs)
    {
        Piece existingPiece = gameState.getPieceById(pDTO.id);
        if (existingPiece != null)
        {
            if(!existingPiece.pos.equals(pDTO.pos)){
                movePiece(existingPiece,pDTO.pos);
            }else {
                //case no change
            }
        }
        else
        {
            piecesToAdd.Add(new Piece(pDTO));
        }
    }

    foreach (Piece p in gameState.pieceList)
    {
        if(getPieceDTOById(p.id,updateData.pieceDTOs)==null){
            piecesToDestroy.Add(p);
        }
    }


    foreach (Piece p in piecesToAdd)
    {
        addPiece(p);
    }
    foreach (Piece p in piecesToDestroy)
    {
        destroy(p);
    }

}





    private void addPiece(Piece p){
        gameState.addPiece(p);
        
        drawer.CreatePieceObject(p);
       // Debug.Log("added Piece " + p.id + " to (" + p.pos.x +","+ p.pos.y+")");
    }

    private void movePiece(Piece p, Pos pos){
        gameState.movePiece(p,pos);

        p.gameObject.transform.localPosition = new Vector3(p.pos.x * size + size/2f, -p.pos.y * size - size/2f, -1);
       // Debug.Log("moved Piece " + p.id + " to (" + pos.x +","+pos.y+")");
    }

    private void destroy(Piece p){
        gameState.destroy(p);

        Destroy(p.gameObject);
       // Debug.Log("destroyed Piece " + p.id);
    }


private PieceDTO getPieceDTOById(string id, PieceDTO[] pDTOs)
{
    foreach (PieceDTO pDTO in pDTOs)
    {
        if (pDTO != null && pDTO.id == id)
        {
            return pDTO;
        }
    }
    return null;
}





    private void click(int x,int y)
    {
            
        if(isPossibleMove(x,y)) {
            drawer.removePossibleMoves();
            drawer.removeSelected();
            StartCoroutine(GameService.play(new Vector2(selectedX,selectedY),new Vector2(x,y)));
            selectedX = -1;
            selectedY = -1;
        }
        else if(gameState.pieces[x,y]!=null){
            selectedX = x;
            selectedY = y;
            drawer.removePossibleMoves();
            drawer.removeSelected();
            bool isOwner = gameState.pieces[x,y].owner==gameState.nextTurn;
            drawer.drawPossibleMoves(gameState.pieces[x,y].possibleMoves,isOwner?green:red);
            drawer.drawSelected(new Pos(selectedX,selectedY),isOwner?green:red);
        }
        else {
            drawer.removePossibleMoves();
            drawer.removeSelected();
            selectedX = -1;
            selectedY = -1;
        }
    }

    private bool isPossibleMove(int x,int y){
        if(selectedX==-1 && selectedY==-1)return false;
        if(gameState.pieces[selectedX, selectedY]==null)Debug.Log("is null");
        Pos[] possibleMoves = gameState.pieces[selectedX, selectedY].possibleMoves;

        foreach (Pos pos in gameState.pieces[selectedX, selectedY].possibleMoves)
        {
            if(pos.x==x && pos.y==y){
                return true;
            }
        }

        return false;
    }



    //-------------------------------------rest-----------------------------------


    void drawChessboard(){
        drawer.CreateChessboard(lightSquareMaterial, darkSquareMaterial);
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
