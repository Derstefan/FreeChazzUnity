using UnityEngine;

public class Config : MonoBehaviour
{

    public static string env = "dev";
    //    public static string env = "prod";

    public static string backendUrl = env == "dev" ? "http://127.0.0.1:8080" : "https://freechazzbe-production.up.railway.app";


    public static int Z_PIECE = 0;

    public Material lightSquareMaterial;
    public Material darkSquareMaterial;
    public Material green;
    public Material red;
    public float maxBoardSize = 15f;

}
