using UnityEngine;


[System.Serializable]
public class Pos
{
    public int x;
    public int y;

    public Pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public bool equals(Pos other)
    {
        return this.x == other.x && this.y == other.y;
    }

    public Pos invertY()
    {
        return new Pos(x, -y);
    }

    public Vector2 GetVector2()
    {
        return new Vector2(x, y);
    }

    public Vector3 GetVector3(float z)
    {
        return new Vector3(x, y, z);
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode();
    }



    public string ToString()
    {
        return "(" + x + "," + y + ")";
    }


    public static float distance(Pos p1, Pos p2)
    {
        Vector3 position1 = new Vector3(p1.x, p1.y);
        Vector3 position2 = new Vector3(p2.x, p2.y);

        return Vector3.Distance(position1, position2);
    }
}
