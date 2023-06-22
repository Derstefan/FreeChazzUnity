#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
    public class UpdateDataDTO
    {
        public String gameId;
        public PlayerDTO player1;
        public PlayerDTO player2;
        public int width;
        public int height;
        public int turn;
        public string nextTurn;
        public string lastActionTime;
        public string winner;
        public bool draw;
        public PieceDTO[] pieceDTOs;

    /*  
    public static UpdateDataDTO mockUpdateData1 = new UpdateDataDTO
{
    drawCounter = 3,
    lastDraw = 1623478800, // Example timestamp
    pieceDTOs = new List<PieceDTO>
    {
       PieceDTO.mockPieceDTO1,
       PieceDTO.mockPieceDTO2,
       PieceDTO.mockPieceDTO3,
    }
};

    public static UpdateDataDTO mockUpdateData2 = new UpdateDataDTO
{
    drawCounter = 4,
    lastDraw = 1623478800232, // Example timestamp
    winner = EPlayer.P1,
    pieceDTOs = new List<PieceDTO>
    {
       PieceDTO.mockPieceDTO2Moved,
       PieceDTO.mockPieceDTO3,
       PieceDTO.mockPieceDTO4,
    }
};
*/
}
