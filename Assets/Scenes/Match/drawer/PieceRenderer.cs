using System.Collections;
using UnityEngine;

namespace Assets.Scenes.Match.drawer
{
    public class PieceRenderer
    {


        public static GameObject createPieceObject(string name, Vector3 vec, string seed, float size)
        {
            GameObject pieceObject = PieceDrawer.generatePieceObject(name, vec, seed, size);

            return pieceObject;
        }
    }
}