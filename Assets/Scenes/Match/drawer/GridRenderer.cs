using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class GridRenderer : MonoBehaviour
{
    public static float cellSize = 0.5f; // Größe einer Zelle im Raster
    public static Color fillColor = Color.green; // Farbe zum Einfärben der Zellen

    private static Material vertexColorMaterial = new Material(Shader.Find("Sprites/Default"));


    public static void render(Transform startPoint, int width, int height, ActionDTO[] actions)
    {
        GameObject gridObject = new GameObject("Grid");
        gridObject.transform.parent = startPoint;

        GameObject cellParent = new GameObject("GridCells");
        cellParent.transform.parent = gridObject.transform;

        // Draw the start point
        Vector3 startPos = startPoint.position + new Vector3((width) * cellSize + 0.5f * cellSize, (height) * cellSize + 0.5f * cellSize, 0);
        GameObject piecePlace = drawCell(startPos, Color.green);
        piecePlace.transform.parent = cellParent.transform;

        foreach (ActionDTO action in actions)
        {
            Vector3 vec = startPoint.position + new Vector3((action.vec.x + width) * cellSize + 0.5f * cellSize, (action.vec.y + height) * cellSize + 0.5f * cellSize, 0);
            GameObject cell = drawCell(vec, getColorByType(action.type));
            cell.transform.parent = cellParent.transform; // Make the cell a child of the cellParent
        }



    }

    private static GameObject drawCell(Vector3 vec,Color color)
    {
        Vector3 cellPosition = vec;

        GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Quad);
        cell.transform.position = cellPosition;
        cell.transform.localScale = new Vector3(cellSize*0.8f, cellSize*0.8f, 1f);

        Renderer cellRenderer = cell.GetComponent<Renderer>();
        cellRenderer.material.color = color;

        return cell;
    }






    public static Color getColorByType(string type)
    {         
        switch (type)
        {
            case "F":
                return new Color(3.2f, 2.8f, 0.8f);
            case "E":
                return new Color(0.8f, 3.6f, 2.4f);
            case "X":
                return new Color(0.4f, 4.0f, 3.2f);
            case "M":
                return new Color(0.8f, 0.4f, 3.2f);
            case "S":
                return new Color(3.2f, 1.2f, 0.0f);
            case "R":
                return new Color(2.8f, 0.0f, 0.8f);
            case "C":
                return new Color(2.8f, 2.8f, 0.8f);
            case "Y":
                return new Color(0.0f, 2.8f, 0.8f);
            case "Z":
                return new Color(2.8f, 0.8f, 0.8f);
            case "A":
                return new Color(2.4f, 0.8f, 1.2f);
            case "Q":
                return new Color(0.8f, 3.2f, 2.4f);
            case "L":
                return new Color(0.4f, 2.4f, 2.4f);
            default:
                return Color.black;
        }


    }
}