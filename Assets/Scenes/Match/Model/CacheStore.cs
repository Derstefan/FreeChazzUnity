
using System;
using System.Collections.Generic;
using UnityEngine;

public class CachedStore
{
    // Array of PieceTypes
    public PieceTypeDTO[] pieceTypeDTOs = null;

    // Constructor
    public CachedStore()
    {
    }


    // Get pieceType by PieceTypeid
    public PieceTypeDTO GetPieceTypeDTO(PieceTypeId pieceTypeId)
    {
        if (pieceTypeDTOs == null) return null;

        foreach (PieceTypeDTO pieceTypeDTO in pieceTypeDTOs)
        {
            if (pieceTypeDTO.pieceTypeId.Equals(pieceTypeId))
            {
                return pieceTypeDTO;
            }
        }

        return null;
    }
}
