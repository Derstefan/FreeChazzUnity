using System;
using System.Collections;
using System.Collections.Generic;
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

        public Vector2 GetVector2()
    {
        return new Vector2(x, y);
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode();
    }

    
}
