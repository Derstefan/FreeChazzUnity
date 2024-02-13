using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

namespace Assets.Scenes.Match.drawer.generators
{
    public class DefaultGenerator
    {
        private static int MAX_LVL = 5;


        public static List<VectorUtils.Geometry> generateGeoms(float size, PieceTypeId pieceTypeId, bool isP1)
        {
            int numPolygons = getShapeNumberByLvl(pieceTypeId.lvl);

            UnityEngine.Random.InitState(GeneratorUtil.StringToSeed(pieceTypeId.pieceTypeId));
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

            //var fill1 = new TextureFill() { Mode = FillMode.OddEven, Texture = PieceTextureGenerator.GenerateColorfulNoiseTexture() };
            var fill1 = new SolidFill() { Color = color };

            // Generate random points and mirrored points
            List<BezierPathSegment> segments1 = GenerateRandomSegments(numPoints, size);
            List<BezierPathSegment> segments2 = GenerateMirroredSegments(segments1, size);

            // Build the scene
            var contour1 = new BezierContour() { Segments = segments1.ToArray(), Closed = true };
            var contour2 = new BezierContour() { Segments = segments2.ToArray(), Closed = true };

            List<Shape> shapes = new List<Shape>();
            shapes.Add(new Shape() { Contours = new BezierContour[] { contour1 }, Fill = fill1 });
            shapes.Add(new Shape() { Contours = new BezierContour[] { contour2 }, Fill = fill1 });
            return shapes;
        }

        private static List<BezierPathSegment> GenerateRandomSegments(int numPoints, float size)
        {
            float multiplayer = 1.0f;

            List<BezierPathSegment> segments = new List<BezierPathSegment>();

            for (int i = 0; i < numPoints; i++)
            {
                var segment = new BezierPathSegment()
                {
                    P0 = new Vector3(UnityEngine.Random.Range(0, size * multiplayer), UnityEngine.Random.Range(0, size * multiplayer), 0),
                    P1 = new Vector3(UnityEngine.Random.Range(0, size * multiplayer), UnityEngine.Random.Range(0, size * multiplayer), 0),
                    P2 = new Vector3(UnityEngine.Random.Range(0, size * multiplayer), UnityEngine.Random.Range(0, size * multiplayer), 0),
                };

                segments.Add(segment);
            }

            return segments;
        }

        private static List<BezierPathSegment> GenerateMirroredSegments(List<BezierPathSegment> segments, float size)
        {
            List<BezierPathSegment> mirroredSegments = new List<BezierPathSegment>();

            foreach (var segment in segments)
            {
                var mirroredSegment = new BezierPathSegment()
                {
                    P0 = new Vector3(size - segment.P0.x, segment.P0.y, 0),
                    P1 = new Vector3(size - segment.P1.x, segment.P1.y, 0),
                    P2 = new Vector3(size - segment.P2.x, segment.P2.y, 0),
                };

                mirroredSegments.Add(mirroredSegment);
            }

            return mirroredSegments;
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



    }
}