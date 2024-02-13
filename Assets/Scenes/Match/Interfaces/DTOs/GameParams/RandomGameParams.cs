namespace Assets.Scenes.Match.Interfaces.DTOs.GameParams
{
    public class RandomGameParams : GameParams
    {
        public enum ESize
        {
            tiny,
            small,
            medium,
            big
        }

        public ESize size;
        public bool withSeed;
        public long seed;
        public bool samePieces;
        public bool isBotEnemy;
        public bool isNetworkGame;
        public bool isAutomatic;

        public string ToString()
        {
            return "RandomGameParams{" +
                   "size=" + size +
                   ", withSeed=" + withSeed +
                   ", seed=" + seed +
                   ", samePieces=" + samePieces +
                   ", isBotEnemy=" + isBotEnemy +
                   ", isNetworkGame=" + isNetworkGame +
                   ", isAutomatic=" + isAutomatic +
                   '}';
        }
    }


}