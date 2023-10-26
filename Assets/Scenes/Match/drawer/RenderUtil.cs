using System.Collections.Generic;
using UnityEngine;






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
    public string action;
}

public class RenderUtil
{
    private static List<ElementConfig> elementConfigs;

    public static Material DEFAULT_MATERIAL = new Material(Shader.Find("Sprites/Default")); //{ color = new Color(1.0f, 1.0f, 1.0f, 3.0f) }; // Default
    public static Shader DEFAULT_SHADER = Shader.Find("Sprites/Default");
    public static Material VECTOR_GRADIEND_MATERIAL = new Material(Shader.Find("Unlit/VectorGradient"));
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
                    ""color"": [0.2, 0.8, 0.8, 0.3],
                    ""name"": ""Yellowish"",
                    ""action"": ""move""
                },
                {
                    ""symbol"": ""E"",
                    ""color"": [0.8, 0.6, 0.4, 0.3],
                    ""name"": ""Peachy"",
""action"": ""defend""
                },
                {
                    ""symbol"": ""X"",
                    ""color"": [0.7, 0.7, 0.7, 0.3],
                    ""name"": ""Aquamarine"",
""action"": ""attack""
                },
                {
                    ""symbol"": ""M"",
                    ""color"": [0.8, 0.4, 0.2, 0.3],
                    ""name"": ""Purple-ish"",
""action"": ""walk""
                },
                {
                    ""symbol"": ""S"",
                    ""color"": [0.6, 0.6, 0.0, 0.3],
                    ""name"": ""Orange"",
""action"": ""swap""
                },
                {
                    ""symbol"": ""R"",
                    ""color"": [0.8, 0.0, 0.8, 0.3],
                    ""name"": ""Reddish"",
""action"": ""rush""
                },
                {
                    ""symbol"": ""C"",
                    ""color"": [0.8, 0.8, 0.8, 0.3],
                    ""name"": ""Light Yellow"",
""action"": ""cross""
                },
                {
                    ""symbol"": ""Y"",
                    ""color"": [0.0, 0.8, 0.8, 0.3],
                    ""name"": ""Teal"",
""action"": ""explode""
                },
                {
                    ""symbol"": ""Z"",
                    ""color"": [0.5, 0.6, 0.6, 0.3],
                    ""name"": ""Light Gray"",
""action"": ""zombie""
                },
                {
                    ""symbol"": ""A"",
                    ""color"": [0.4, 0.8, 0.2, 0.3],
                    ""name"": ""Pinkish"",
""action"": ""ranged""
                },
                {
                    ""symbol"": ""Q"",
                    ""color"": [0.8, 0.2, 0.4, 0.3],
                    ""name"": ""Coral"",
""action"": ""convert""
                },
                {
                    ""symbol"": ""L"",
                    ""color"": [0.7, 0.4, 0.4, 0.3],
                    ""name"": ""Light Blue"",
""action"": ""legion""
                },
                {
                    ""symbol"": ""5"",
                    ""color"": [0.4, 0.7, 0.4, 0.4],
                    ""name"": ""Blue"",
""action"": ""magic""
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
            Material mat = new Material(DEFAULT_SHADER) { color = new Color(elementConfig.color[0], elementConfig.color[1], elementConfig.color[2], elementConfig.color[3]) };
            return mat;
        }

        return materialDefault;
    }


    public static string getActionByType(string type)
    {
        ElementConfig elementConfig = elementConfigs.Find(config => config.symbol == type);
        if (elementConfig != null)
        {
            return elementConfig.action;
        }
        return "not found";//TODO make "" out of it

    }

    public static Color getColorByType(string type)
    {
        ElementConfig elementConfig = elementConfigs.Find(config => config.symbol == type);
        if (elementConfig != null)
        {
            return new Color(elementConfig.color[0], elementConfig.color[1], elementConfig.color[2], 1.0f);
        }
        return Color.black;

    }
}
