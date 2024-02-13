
using UnityEngine;

public class AnimationUtil
{
    public static Vector3 getVector3FromPos(Pos pos, float gameSize)
    {
        return (pos.invertY().GetVector3(-10f) + new Vector3(0.5f, -0.5f, -10f)) * gameSize;
    }

}
