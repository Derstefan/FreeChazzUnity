using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Unity.VectorGraphics;
using UnityEngine;

namespace Assets.Scenes.Match.drawer
{
    public class PieceRenderer
    {





        public static GameObject createPieceObject(string name, Vector3 vec, string seed, float size)
        {

            GameObject pieceObject = new GameObject(name);
            pieceObject.transform.localPosition = vec;

            GameObject renderedPiece = new GameObject("RenderedPiece");
            renderedPiece.transform.parent = pieceObject.transform;
            renderedPiece.transform.localPosition = new Vector3(0, 0, -10f);
            SpriteRenderer sr = renderedPiece.AddComponent<SpriteRenderer>();
            sr.material = RenderUtil.VECTOR_GRADIEND_MATERIAL;
            sr.sprite = generateSprite(2, size * 12f, seed, 2);
            return pieceObject;
        }


        //this method is for UI
        public static Texture2D render(PieceTypeId pieceTypeId, float size, string playerType)
        {
            return generateTexture(2, 5f, pieceTypeId.pieceTypeId + "", 2);
        }



        private static Texture2D generateTexture(int numPoints, float size, string seed, int numPolygons)
        {
            var sprite = generateSprite(numPoints, size, seed, numPolygons);
            var texture = VectorUtils.RenderSpriteToTexture2D(sprite, 1000, 1000, RenderUtil.VECTOR_GRADIEND_MATERIAL, 2, false);
            return texture;
        }


        private static List<VectorUtils.Geometry> generateGeoms(int numPoints, float size, string seed, int numPolygons)
        {


            UnityEngine.Random.InitState(StringToSeed(seed));
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
                List<Shape> pair = createShapePair(numPoints, size);
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



        private static List<Shape> createShapePair(int numPoints, float size)
        {
            var fill1 = new SolidFill() { Mode = FillMode.OddEven, Color = new Color32((byte)UnityEngine.Random.Range(0, 200), (byte)UnityEngine.Random.Range(0, 200), (byte)UnityEngine.Random.Range(0, 200), 255) };


            // Generate random points and colors
            List<BezierPathSegment> segments1 = new List<BezierPathSegment>();
            List<BezierPathSegment> segments2 = new List<BezierPathSegment>();

            for (int i = 0; i < numPoints; i++)
            {
                // Create a random BezierPathSegment
                var segment = new BezierPathSegment()
                {
                    P0 = new Vector3(UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size), 0),
                    P1 = new Vector3(UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size), 0),
                    P2 = new Vector3(UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size), 0),
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


        public static Sprite generateSprite(int numPoints, float size, string seed, int numPolygons)
        {
            var geoms = generateGeoms(numPoints, size, seed, numPolygons);
            var sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 16, true);
            return sprite;
        }


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