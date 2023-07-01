using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public Piece[,] pieces;
    public List<Piece> pieceList = new List<Piece>();
    public int turn=0;
    public string nextTurn;

    public GameState(int width,int height){
        pieces = new Piece[width,height];
    }


    public void addPiece(Piece p){
        pieces[p.pos.x, p.pos.y] = p;
        pieceList.Add(p);
    }

    public void movePiece(Piece p, Pos pos){
        pieces[p.pos.x, p.pos.y] = null;
        pieces[pos.x, pos.y] = p;
        p.pos.x = pos.x;
        p.pos.y = pos.y;
    }

    public void destroy(Piece p){
        pieces[p.pos.x, p.pos.y] = null;
        pieceList.Remove(p);
    }

    public Piece getPiece(int i,int j){
        return pieces[i,j];
    }

    public Piece getPieceById(string pieceId)
    {
        foreach (Piece piece in pieceList)
        {
            if (piece != null && piece.pieceId == pieceId)
            {
                return piece;
            }
        }
        return null;
    }
}
