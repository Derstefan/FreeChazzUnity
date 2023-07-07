using System;

[Serializable]
public class PieceTypeId
{
        public string pieceTypeId;
        public long seed;
        public int lvl;
        public string generatorVersion;

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
