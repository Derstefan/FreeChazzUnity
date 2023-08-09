using UnityEditor;
using UnityEngine;
using System.Collections.Generic;






[System.Serializable]
public class ElementConfigData
{
    public List<ElementConfig> elements;
}

[System.Serializable]
public class ElementConfig
{
    public string symbol;
    public float[] color;
    public string name;
}

public class RenderUtil
{
    private static List<ElementConfig> elementConfigs;

    public static Material materialDefault = new Material(Shader.Find("Sprites/Default")) { color = new Color(0.2f, 0.3f, 0.3f, 0.3f) }; // Default
    public static Material red = new Material(Shader.Find("Sprites/Default")) { color = new Color(3.2f, 0.8f, 0.8f, 0.3f) };
    public static Material green = new Material(Shader.Find("Sprites/Default")) { color = new Color(0.2f, 2.8f, 0.2f, 0.3f) };

    static RenderUtil()
    {
        LoadElementConfigs();
    }

    private static void LoadElementConfigs()
    {
        string jsonText = @"
        {
            ""elements"": [
                {
                    ""symbol"": ""F"",
                    ""color"": [3.2, 2.8, 0.8, 0.3],
                    ""name"": ""Yellowish""
                },
                {
                    ""symbol"": ""E"",
                    ""color"": [0.8, 3.6, 2.4, 0.3],
                    ""name"": ""Peachy""
                },
                {
                    ""symbol"": ""X"",
                    ""color"": [0.4, 4.0, 3.2, 0.3],
                    ""name"": ""Aquamarine""
                },
                {
                    ""symbol"": ""M"",
                    ""color"": [0.8, 0.4, 3.2, 0.3],
                    ""name"": ""Purple-ish""
                },
                {
                    ""symbol"": ""S"",
                    ""color"": [3.2, 1.2, 0.0, 0.3],
                    ""name"": ""Orange""
                },
                {
                    ""symbol"": ""R"",
                    ""color"": [2.8, 0.0, 0.8, 0.3],
                    ""name"": ""Reddish""
                },
                {
                    ""symbol"": ""C"",
                    ""color"": [2.8, 2.8, 0.8, 0.3],
                    ""name"": ""Light Yellow""
                },
                {
                    ""symbol"": ""Y"",
                    ""color"": [0.0, 2.8, 0.8, 0.3],
                    ""name"": ""Teal""
                },
                {
                    ""symbol"": ""Z"",
                    ""color"": [2.8, 0.8, 0.8, 0.3],
                    ""name"": ""Light Gray""
                },
                {
                    ""symbol"": ""A"",
                    ""color"": [2.4, 0.8, 1.2, 0.3],
                    ""name"": ""Pinkish""
                },
                {
                    ""symbol"": ""Q"",
                    ""color"": [0.8, 3.2, 2.4, 0.3],
                    ""name"": ""Coral""
                },
                {
                    ""symbol"": ""L"",
                    ""color"": [0.4, 2.4, 2.4, 0.3],
                    ""name"": ""Light Blue""
                },
                {
                    ""symbol"": ""5"",
                    ""color"": [0.4, 0.4, 2.4, 0.4],
                    ""name"": ""Blue""
                }
            ]
        }";

        ElementConfigData data = JsonUtility.FromJson<ElementConfigData>(jsonText);
        elementConfigs = data.elements;
    }

    public static Material getMaterialByType(string type)
    {
        ElementConfig elementConfig = elementConfigs.Find(config => config.symbol == type);
        if (elementConfig != null)
        {
            Material mat = new Material(Shader.Find("Sprites/Default")) { color = new Color(elementConfig.color[0], elementConfig.color[1], elementConfig.color[2], elementConfig.color[3]) };
            return mat;
        }

        return materialDefault;
    }
}
