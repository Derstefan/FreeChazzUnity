using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PieceDTO
{
    public string pieceId;
    public PieceTypeId pieceTypeId;

    public Pos pos;
    public string symbol;
    public Pos[] possibleMoves;
    public string owner;

}
