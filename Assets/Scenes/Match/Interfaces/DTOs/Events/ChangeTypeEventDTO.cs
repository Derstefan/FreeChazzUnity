using System;
using UnityEditor;

[Serializable]

public class ChangeTypeEventDTO : EventDTO
{
    public string pieceId;
    public PieceTypeId newPieceTypeId;
    public PieceTypeId oldPieceTypeId;
}
