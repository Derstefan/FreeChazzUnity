
using System;
using UnityEditor;

[Serializable]

public class MoveAndDestroyEventDTO : EventDTO
{
    public Pos fromPos;
    public string pieceId;
    public Pos toPos;
    public string targetPieceId;
}
