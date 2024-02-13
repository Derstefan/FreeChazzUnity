using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public Piece[,] pieces;
    public List<Piece> pieceList = new List<Piece>();
    public int turn = 0;
    public int maxLoadedTurn = 0;
    public string nextTurn;

    public GameState(int width, int height)
    {
        pieces = new Piece[width, height];
    }


    public void addPiece(Piece p)
    {

        pieces[p.pos.x, p.pos.y] = p;
        pieceList.Add(p);
    }

    public void movePiece(Piece p, Pos pos)
    {
        pieces[p.pos.x, p.pos.y] = null;
        Piece pAtToPos = pieces[pos.x, pos.y];
        if (pAtToPos != null)
        {
            //pieces[pAtToPos.pos.x, pAtToPos.pos.y] = null;
            //pieceList.Remove(p);
            destroy(pAtToPos);
        }

        pieces[pos.x, pos.y] = p;
        p.pos.x = pos.x;
        p.pos.y = pos.y;



    }

    public void swapPieces(Pos pos1, Pos pos2)
    {
        Debug.Log("Swapping pieces");
        Piece p1 = pieces[pos1.x, pos1.y];
        Piece p2 = pieces[pos2.x, pos2.y];
        if (p1 == null || p2 == null)
        {
            Debug.LogError("Tried to swap null piece");
        }
        pieces[pos1.x, pos1.y] = p2;
        pieces[pos2.x, pos2.y] = p1;
        p1.pos.x = pos2.x;
        p1.pos.y = pos2.y;
        p2.pos.x = pos1.x;
        p2.pos.y = pos1.y;
    }

    public void destroy(Piece p)
    {
        Destroy(p.gameObject);
        pieces[p.pos.x, p.pos.y] = null;
        pieceList.Remove(p);
    }

    public Piece getPiece(int i, int j)
    {
        return pieces[i, j];
    }

    public Piece getPiece(Pos pos)
    {
        return pieces[pos.x, pos.y];
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
