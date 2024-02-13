using Assets.Scenes.Match.drawer.generators;
using Unity.VectorGraphics;
using UnityEngine;

namespace Assets.Scenes.Match.drawer
{
    public class PieceRenderer
    {


        private static float PIECE_SIZE = 7f;



        public static GameObject createPieceObject(string name, Vector3 vec, PieceTypeId pieceTypeId, float size, bool isP1)
        {

            GameObject pieceObject = new GameObject(name);
            pieceObject.transform.localPosition = vec;

            // Create the main piece
            GameObject renderedPiece = new GameObject("RenderedPiece");
            renderedPiece.transform.parent = pieceObject.transform;
            renderedPiece.transform.localPosition = new Vector3(0, 0, 0f);
            SpriteRenderer sr = renderedPiece.AddComponent<SpriteRenderer>();
            sr.material = RenderUtil.VECTOR_GRADIEND_MATERIAL;
            Sprite sprite = generateSprite(size * PIECE_SIZE, pieceTypeId, isP1);
            sr.sprite = sprite;

            // Create the shadow
            GameObject shadowObject = new GameObject("Shadow");
            shadowObject.transform.parent = pieceObject.transform;
            shadowObject.transform.localPosition = new Vector3(0.00f, 0.000f, 1f);
            shadowObject.transform.localScale = new Vector3(1.04f, 1.05f, 1f);
            SpriteRenderer shadowRenderer = shadowObject.AddComponent<SpriteRenderer>();
            shadowRenderer.material = RenderUtil.DEFAULT_MATERIAL;
            shadowRenderer.sprite = sprite; // Use the same sprite as the main piece
            shadowRenderer.color = new Color(0.4f, 0.4f, 0.4f, 0.6f); // Transparent gray color
            return pieceObject;

        }



        public static Sprite generateSprite(float size, PieceTypeId pieceTypeId, bool isP1)
        {
            var geoms = PolygonGenerator.generateGeoms(size, pieceTypeId, isP1);
            var sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 16, true);
            return sprite;
        }


        public static Texture2D generateTexture(float size, PieceTypeId pieceTypeId, bool isP1)
        {
            var sprite = generateSprite(size, pieceTypeId, isP1);
            var texture = VectorUtils.RenderSpriteToTexture2D(sprite, 1000, 1000, RenderUtil.VECTOR_GRADIEND_MATERIAL, 2, false);
            return texture;
        }











    }
}