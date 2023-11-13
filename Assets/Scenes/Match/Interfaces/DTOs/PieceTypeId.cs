using System;

[Serializable]
public class PieceTypeId
{
    public string pieceTypeId;
    public long seed;
    public int lvl;
    public string generatorVersion;


    public static PieceTypeId createRandomPieceTypeId()
    {
        PieceTypeId pieceTypeId = new PieceTypeId();
        pieceTypeId.pieceTypeId = Guid.NewGuid().ToString();
        pieceTypeId.seed = new System.Random().Next();
        pieceTypeId.lvl = new System.Random().Next() % 5 + 1;
        pieceTypeId.generatorVersion = "1.0";
        return pieceTypeId;

    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        PieceTypeId other = (PieceTypeId)obj;

        return pieceTypeId == other.pieceTypeId &&
            seed == other.seed &&
            lvl == other.lvl &&
            generatorVersion == other.generatorVersion;
    }
}
