
using System;
using UnityEditor;

[Serializable]
public class EventDTO
{
    public string type;
    public string actName;
    public Pos fromPos;
    public Pos toPos;
    public Pos pos;
    public string pieceId;
    public string targetPieceId;
    public PieceTypeId newPieceTypeId;
    public PieceTypeId oldPieceTypeId;
}
