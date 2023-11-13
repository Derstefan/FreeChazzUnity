using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Unity.VectorGraphics;
using UnityEngine;

namespace Assets.Scenes.Match.drawer
{
    public class PieceRenderer
    {


        private static int MAX_LVL = 5;
        private static float PIECE_SICE = 13f;



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
            Sprite sprite = generateSprite(size * PIECE_SICE, pieceTypeId, isP1);
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
            var geoms = generateGeoms(size, pieceTypeId, isP1);
            var sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 16, true);
            return sprite;
        }


        //this method is for UI
        public static Texture2D render(PieceTypeId pieceTypeId, float size, bool isP1)
        {
            return generateTexture(size, pieceTypeId, isP1);
        }

        private static Texture2D generateTexture(float size, PieceTypeId pieceTypeId, bool isP1)
        {
            var sprite = generateSprite(size, pieceTypeId, isP1);
            var texture = VectorUtils.RenderSpriteToTexture2D(sprite, 1000, 1000, RenderUtil.VECTOR_GRADIEND_MATERIAL, 2, false);
            return texture;
        }


        private static List<VectorUtils.Geometry> generateGeoms(float size, PieceTypeId pieceTypeId, bool isP1)
        {
            int numPolygons = getShapeNumberByLvl(pieceTypeId.lvl);

            UnityEngine.Random.InitState(StringToSeed(pieceTypeId.pieceTypeId));
            var tessOptions = new VectorUtils.TessellationOptions()
            {
                StepDistance = 10.0f,
                MaxCordDeviation = 0.5f,
                MaxTanAngleDeviation = 0.1f,
                SamplingStepSize = 0.01f
            };

            List<Shape> shapes = new List<Shape>();
            for (int i = 0; i < numPolygons; i++)
            {

                Color color = GenerateRandomColor(pieceTypeId.lvl, isP1);
                List<Shape> pair = createShapePair(getPointsByLvl(pieceTypeId.lvl), size, color);
                shapes.Add(pair[0]);
                shapes.Add(pair[1]);
            }


            var scene = new Scene()
            {
                Root = new SceneNode()
                {
                    Shapes = shapes
                }
            };

            // Dynamically import the SVG data, and tessellate the resulting vector scene.
            return VectorUtils.TessellateScene(scene, tessOptions);
        }

        public static int getPointsByLvl(int lvl)
        {
            return lvl < 4 ? 2 : 3;
        }

        public static int getShapeNumberByLvl(int lvl)
        {
            return lvl < 4 ? 2 : 3;
        }


        public static Color GenerateRandomColor(int lvl, bool isP1)
        {
            float saturationBase = 0f;
            float saturationMultiplier = ((float)lvl + saturationBase) / ((float)MAX_LVL + saturationBase);


            float r, b, g;

            r = (UnityEngine.Random.Range(0, 200f));
            g = (UnityEngine.Random.Range(0f, 200f));
            b = (UnityEngine.Random.Range(0f, 200f));
            /*
                        if (isP1)
                        {
                            r = (UnityEngine.Random.Range(0f, 50));
                            g = (UnityEngine.Random.Range(0f, 100));
                            b = (UnityEngine.Random.Range(100f, 200f));
                        }
                        else
                        {
                            r = (UnityEngine.Random.Range(100f, 200f));
                            g = (UnityEngine.Random.Range(0f, 100));
                            b = (UnityEngine.Random.Range(0f, 50));
                        }
            */
            float a = 255;

            float average = (r + g + b) / 3f;



            r = Mathf.Lerp(r, average, 1 - saturationMultiplier) * (1 - saturationMultiplier * 0.3f);
            g = Mathf.Lerp(g, average, 1 - saturationMultiplier) * (1 - saturationMultiplier * 0.3f);
            b = Mathf.Lerp(b, average, 1 - saturationMultiplier) * (1 - saturationMultiplier * 0.3f);



            Color randomColor = new Color32((byte)r, (byte)g, (byte)b, (byte)a);

            return randomColor;
        }




        private static List<Shape> createShapePair(int numPoints, float size, Color color)
        {

            float multiplayer = 1.0f;

            var fill1 = new SolidFill() { Mode = FillMode.OddEven, Color = color };

            // Generate random points and colors
            List<BezierPathSegment> segments1 = new List<BezierPathSegment>();
            List<BezierPathSegment> segments2 = new List<BezierPathSegment>();

            for (int i = 0; i < numPoints; i++)
            {
                // Create a random BezierPathSegment
                var segment = new BezierPathSegment()
                {
                    P0 = new Vector3(UnityEngine.Random.Range(0, size * multiplayer), UnityEngine.Random.Range(0, size * multiplayer), 0),
                    P1 = new Vector3(UnityEngine.Random.Range(0, size * multiplayer), UnityEngine.Random.Range(0, size * multiplayer), 0),
                    P2 = new Vector3(UnityEngine.Random.Range(0, size * multiplayer), UnityEngine.Random.Range(0, size * multiplayer), 0),
                };

                segments1.Add(segment);

                // Create a mirrored BezierPathSegment
                var mirroredSegment = new BezierPathSegment()
                {
                    P0 = new Vector3(size - segment.P0.x, segment.P0.y, 0),
                    P1 = new Vector3(size - segment.P1.x, segment.P1.y, 0),
                    P2 = new Vector3(size - segment.P2.x, segment.P2.y, 0),
                };

                segments2.Add(mirroredSegment);
            }

            // Build the scene
            var contour1 = new BezierContour()
            {
                Segments = segments1.ToArray(),
                Closed = true
            };
            var contour2 = new BezierContour()
            {
                Segments = segments2.ToArray(),
                Closed = true
            };

            List<Shape> shapes = new List<Shape>();
            shapes.Add(new Shape() { Contours = new BezierContour[] { contour1 }, Fill = fill1 });
            shapes.Add(new Shape() { Contours = new BezierContour[] { contour2 }, Fill = fill1 });
            return shapes;
        }




        /*
                private static Sprite generateSpriteSVG(int numPoints, float size, string seed, int numPolygons)
                {


                    var tessOptions = new VectorUtils.TessellationOptions()
                    {
                        StepDistance = 100.0f,
                        MaxCordDeviation = 0.5f,
                        MaxTanAngleDeviation = 0.1f,
                        SamplingStepSize = 0.01f
                    };

                    // Dynamically import the SVG data, and tessellate the resulting vector scene.
                    var sceneInfo = SVGParser.ImportSVG(new StringReader(SVGs.svg3));
                    var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);

                    return VectorUtils.BuildSprite(geoms, 1000f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);
                }
        */

        //--------------------- Helper functions ---------------------

        // Function to convert a string seed to an integer seed
        private static int StringToSeed(string seedString)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(seedString));
                int seed = 0;
                for (int i = 0; i < 4; i++)
                {
                    seed ^= ((int)hashBytes[i]) << (i * 8);
                }
                return seed;
            }
        }


    }
}