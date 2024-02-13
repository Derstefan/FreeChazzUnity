using UnityEngine;


public class Piece
{
    public string pieceId;
    //PieceTypeData
    public PieceTypeId pieceTypeId;

    public Pos pos;
    public string symbol;
    public MoveSet moveSet;
    public string owner;

    public bool isKing;

    public GameObject gameObject;

    public bool showsMoveSet = true;

    public Piece(PieceDTO pieceDTO)
    {
        pieceId = pieceDTO.pieceId;
        pieceTypeId = pieceDTO.pieceTypeId;
        pos = new Pos(pieceDTO.pos.x, pieceDTO.pos.y);
        symbol = pieceDTO.symbol;
        moveSet = pieceDTO.moveSet;
        owner = pieceDTO.owner;
        isKing = pieceDTO.king;
    }


    public void assignGameObject(GameObject obj)
    {
        gameObject = obj;
    }

    //        public static Piece mockPiece1 =  new Piece(PieceDTO.mockPieceDTO1);
    //        public static Piece mockPiece2 =  new Piece(PieceDTO.mockPieceDTO2);


    public string toString()
    {
        return "Piece: " + pieceId + " " + pieceTypeId + " " + pos + " " + symbol + " " + moveSet + " " + owner;
    }
}