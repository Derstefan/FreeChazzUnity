using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

namespace Assets.Scenes.Match.drawer.generators
{
    public class PolygonGenerator
    {
        private static int MAX_LVL = 5;
        private static VectorUtils.TessellationOptions tessOptions = new VectorUtils.TessellationOptions()
        {
            StepDistance = 10.0f,
            MaxCordDeviation = 0.5f,
            MaxTanAngleDeviation = 0.1f,
            SamplingStepSize = 0.01f
        };

        public static List<VectorUtils.Geometry> generateGeoms(float size, PieceTypeId pieceTypeId, bool isP1)
        {
            int numPolygons = 5;
            UnityEngine.Random.InitState(GeneratorUtil.StringToSeed(pieceTypeId.pieceTypeId));


            List<Shape> shapes = new List<Shape>();
            for (int i = 0; i < numPolygons; i++)
            {

                Color color = GenerateRandomColor(pieceTypeId.lvl, isP1);
                List<Shape> pair = createShapePair(5, size, color);
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
        private static List<Shape> createShapePair(int numPoints, float size, Color color)
        {

            //var fill1 = new TextureFill() { Mode = FillMode.OddEven, Texture = PieceTextureGenerator.GenerateColorfulNoiseTexture() };
            var fill1 = new SolidFill() { Color = color };


            Vector2[] points = GeneratorUtil.GenerateRandomPoints(numPoints, 0f, 0f, size, size);
            Vector2[] points2 = GeneratorUtil.MirrorPoints(points, size);

            List<BezierPathSegment> segments1 = GenerateLinearSegments(points);
            List<BezierPathSegment> segments2 = GenerateLinearSegments(points2);

            // Build the scene
            var contour1 = new BezierContour() { Segments = segments1.ToArray(), Closed = true };
            var contour2 = new BezierContour() { Segments = segments2.ToArray(), Closed = true };

            List<Shape> shapes = new List<Shape>();
            shapes.Add(new Shape() { Contours = new BezierContour[] { contour1 }, Fill = fill1 });
            shapes.Add(new Shape() { Contours = new BezierContour[] { contour2 }, Fill = fill1 });
            return shapes;
        }

        public static Color GenerateRandomColor(int lvl, bool isP1)
        {
            float saturationBase = 0f;
            float saturationMultiplier = ((float)lvl + saturationBase) / ((float)MAX_LVL + saturationBase);


            float r, b, g;

            r = (UnityEngine.Random.Range(0, 200f));
            g = (UnityEngine.Random.Range(0f, 200f));
            b = (UnityEngine.Random.Range(0f, 200f));

            float a = 255;

            float average = (r + g + b) / 3f;



            r = Mathf.Lerp(r, average, 1 - saturationMultiplier) * (1 - saturationMultiplier * 0.3f);
            g = Mathf.Lerp(g, average, 1 - saturationMultiplier) * (1 - saturationMultiplier * 0.3f);
            b = Mathf.Lerp(b, average, 1 - saturationMultiplier) * (1 - saturationMultiplier * 0.3f);



            Color randomColor = new Color32((byte)r, (byte)g, (byte)b, (byte)a);

            return randomColor;
        }


        private static List<BezierPathSegment> GenerateLinearSegments(Vector2[] points)
        {
            List<BezierPathSegment> segments = new List<BezierPathSegment>();


            for (int i = 0; i < points.Length; i++)
            {
                var nextIndex = i + 1 == points.Length ? 0 : i + 1;
                var segment = new BezierPathSegment()
                {
                    P0 = new Vector3(points[i].x, points[i].y, 0f),
                    P1 = new Vector3(points[i].x, points[i].y, 0f),
                    P2 = new Vector3(points[nextIndex].x, points[nextIndex].y, 0f),
                };

                segments.Add(segment);
            }

            return segments;
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


    }
}