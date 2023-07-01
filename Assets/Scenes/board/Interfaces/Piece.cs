using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Piece
{
    public string pieceId;
        //PieceTypeData
    public PieceTypeId pieceTypeId;

    public Pos pos;
    public string symbol;
    public Pos[] possibleMoves;
    public string owner;

    public GameObject gameObject;


    public Piece(PieceDTO pieceDTO)
    {
        pieceId = pieceDTO.pieceId;
        pieceTypeId = pieceDTO.pieceTypeId;
        pos = pieceDTO.pos;
        symbol = pieceDTO.symbol;
        possibleMoves = pieceDTO.possibleMoves;
        owner = pieceDTO.owner;
    }


    public void assignGameObject(GameObject obj){
        gameObject = obj;
    }

//        public static Piece mockPiece1 =  new Piece(PieceDTO.mockPieceDTO1);
//        public static Piece mockPiece2 =  new Piece(PieceDTO.mockPieceDTO2);
    
}