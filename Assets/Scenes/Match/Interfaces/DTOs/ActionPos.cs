using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ActionPos : Pos
{
    public string tag;

    public ActionPos(int x, int y, string tag) : base(x, y)
    {
        this.x = x;
        this.y = y;
        this.tag = tag;
    }

    public string getTag()
    {
        return tag;
    }
}

