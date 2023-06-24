using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PieceDTO
{
    public string id;
    public PieceTypeDTO pieceType;
    public Pos pos;
    public string symbol;
    public Pos[] possibleMoves;
    public string owner;


/*
    public static PieceDTO mockPieceDTO1 =  new PieceDTO{
            id = "1",
            pos = new Pos(2,6),
            pieceType = PieceTypeDTO.mockType1,
            symbol = "P",
            possibleMoves = new List<Pos>
            {
                new Pos ( 1, 1 ),
                new Pos ( 3, 2 )
            },
            owner = EPlayer.P1
        };

            public static PieceDTO mockPieceDTO2 =  new PieceDTO{
            id = "2",
            pos = new Pos(2,2),
            pieceType = PieceTypeDTO.mockType2,
            symbol = "X",
            possibleMoves = new List<Pos>
            {
                new Pos ( 2, 3 ),
                new Pos ( 3, 2 )
            },
            owner = EPlayer.P2
        };

        
            public static PieceDTO mockPieceDTO2Moved =  new PieceDTO{
            id = "2",
            pos = new Pos(3,3),
            pieceType = PieceTypeDTO.mockType2,
            symbol = "X",
            possibleMoves = new List<Pos>
            {
                new Pos ( 2, 3 ),
                new Pos ( 3, 2 )
            },
            owner = EPlayer.P2
        };


            public static PieceDTO mockPieceDTO3 =  new PieceDTO{
            id = "3",
            pos = new Pos(7,7),
            pieceType = PieceTypeDTO.mockType3,
            symbol = "Q",
            possibleMoves = new List<Pos>
            {
                new Pos ( 2, 3 ),
                new Pos ( 3, 2 )
            },
            owner = EPlayer.P2
        };

        public static PieceDTO mockPieceDTO4 =  new PieceDTO{
            id = "4",
            pos = new Pos(0,3),
            pieceType = PieceTypeDTO.mockType4,
            symbol = "R",
            possibleMoves = new List<Pos>
            {
                new Pos ( 2, 3 ),
                new Pos ( 3, 2 )
            },
            owner = EPlayer.P2
        };

        */
}
