using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VectorGraphics;
using UnityEngine;

public class SVGRuntimeLoad2 : MonoBehaviour
{
    void Start()
    {
        string svg =
            @"<svg height=""40"" width=""45"">
<path id=""lineAB"" d=""M 10 35 l 15 -30"" stroke=""red"" stroke-width=""3"" fill=""none"" />
  <path id=""lineBC"" d=""M 25 50 l 15 30"" stroke=""red"" stroke-width=""3"" fill=""none"" />
  <path d=""M 175 20 l 15 0"" stroke=""green"" stroke-width=""3"" fill=""none"" />
  <path d=""M 10 35 q 15 -30 30 0"" stroke=""blue"" stroke-width=""5"" fill=""none"" />
  <g stroke=""black"" stroke-width=""3"" fill=""black"">
    <circle id=""pointA"" cx=""10"" cy=""35"" r=""3"" />
    <circle id=""pointB"" cx=""25"" cy=""5"" r=""3"" />
    <circle id=""pointC"" cx=""4"" cy=""35"" r=""3"" />
  </g>
  <g font-size=""30"" font-family=""sans-serif"" fill=""black"" stroke=""none"" text-anchor=""middle"">
    <text x=""10"" y=""35"" dx=""-30"">A</text>
    <text x=""25"" y=""5"" dy=""-10"">B</text>
    <text x=""40"" y=""35"" dx=""30"">C</text>
  </g>
  Sorry, your browser does not support inline SVG.
</svg>";

        var tessOptions = new VectorUtils.TessellationOptions()
        {
            StepDistance = 100.0f,
            MaxCordDeviation = 0.5f,
            MaxTanAngleDeviation = 0.1f,
            SamplingStepSize = 0.01f
        };

        // Dynamically import the SVG data, and tessellate the resulting vector scene.
        var sceneInfo = SVGParser.ImportSVG(new StringReader(svg));
        var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);

        // Build a sprite with the tessellated geometry.
        var sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void OnDisable()
    {
        GameObject.Destroy(GetComponent<SpriteRenderer>().sprite);
    }
}
