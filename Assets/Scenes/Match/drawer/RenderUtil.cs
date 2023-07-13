using UnityEditor;
using UnityEngine;


    public class RenderUtil
    {



    public static Material materialF = new Material(Shader.Find("Sprites/Default")) { color = new Color(3.2f, 2.8f, 0.8f, 0.3f) };
    public static Material materialE = new Material(Shader.Find("Sprites/Default")) { color = new Color(0.8f, 3.6f, 2.4f, 0.3f) };
    public static Material materialX = new Material(Shader.Find("Sprites/Default")) { color = new Color(0.4f, 4.0f, 3.2f, 0.3f) };
    public static Material materialM = new Material(Shader.Find("Sprites/Default")) { color = new Color(0.8f, 0.4f, 3.2f, 0.3f) };
    public static Material materialS = new Material(Shader.Find("Sprites/Default")) { color = new Color(3.2f, 1.2f, 0.0f, 0.3f) };
    public static Material materialR = new Material(Shader.Find("Sprites/Default")) { color = new Color(2.8f, 0.0f, 0.8f, 0.3f) };
    public static Material materialC = new Material(Shader.Find("Sprites/Default")) { color = new Color(2.8f, 2.8f, 0.8f, 0.3f) };
    public static Material materialY = new Material(Shader.Find("Sprites/Default")) { color = new Color(0.0f, 2.8f, 0.8f, 0.3f) };
    public static Material materialZ = new Material(Shader.Find("Sprites/Default")) { color = new Color(2.8f, 0.8f, 0.8f, 0.3f) };
    public static Material materialA = new Material(Shader.Find("Sprites/Default")) { color = new Color(2.4f, 0.8f, 1.2f, 0.3f) };
    public static Material materialQ = new Material(Shader.Find("Sprites/Default")) { color = new Color(0.8f, 3.2f, 2.4f, 0.3f) };
    public static Material materialL = new Material(Shader.Find("Sprites/Default")) { color = new Color(0.4f, 2.4f, 2.4f, 0.3f) };
    public static Material materialDefault = new Material(Shader.Find("Sprites/Default")) { color = new Color(0.2f, 0.3f, 0.3f, 0.3f) };


    public static Material getMaterialByType(string type)
    {
        switch (type)
        {
            case "F":
                return materialF;
            case "E":
                return materialE;
            case "X":
                return materialX;
            case "M":
                return materialM;
            case "S":
                return materialS;
            case "R":
                return materialR;
            case "C":
                return materialC;
            case "Y":
                return materialY;
            case "Z":
                return materialZ;
            case "A":
                return materialA;
            case "Q":
                return materialQ;
            case "L":
                return materialL;
            default:
                return materialDefault;
        }
    }

}
