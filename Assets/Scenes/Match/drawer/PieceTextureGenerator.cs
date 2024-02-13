using UnityEngine;

namespace Assets.Scenes.Match.drawer
{
    public class PieceTextureGenerator
    {
        public static int textureWidth = 256;
        public static int textureHeight = 256;
        public static float scale = 3f;

        public static Texture2D GenerateColorfulNoiseTexture()
        {
            Texture2D texture = new Texture2D(textureWidth, textureHeight);

            Color color1 = new Color(1f, 0f, 0f); // Red
            Color color2 = new Color(0f, 1f, 0f); // Green
            Color color3 = new Color(0f, 0f, 1f); // Blue

            for (int y = 0; y < textureHeight; y++)
            {
                for (int x = 0; x < textureWidth; x++)
                {
                    float xCoord = x / (float)textureWidth * scale;
                    float yCoord = y / (float)textureHeight * scale;

                    float sample = Mathf.PerlinNoise(xCoord, yCoord);

                    // Interpolate between colors based on the Perlin noise value
                    Color color = Color.Lerp(color1, color2, sample);
                    color = Color.Lerp(color, color3, sample);

                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            return texture;
        }

    }
}