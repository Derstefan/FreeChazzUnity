using System;
using UnityEditor;

[Serializable]

public class DestroyEventDTO : EventDTO
{
    public string pieceId;
    public Pos pos;
}
