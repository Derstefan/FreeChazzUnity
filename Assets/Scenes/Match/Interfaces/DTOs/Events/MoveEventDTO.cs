
using System;
using UnityEditor;

[Serializable]

public class MoveEventDTO : EventDTO
{
    public Pos fromPos;
    public string pieceId;
    public Pos toPos;
}
