using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallInfo
{
    public TimeDay wallType;
    public Vector3 position;
    public float rotation;

    public WallInfo()
    {
        wallType = TimeDay.DAWN;
        position = Vector3.zero;
        rotation = 0f;
    }

    public WallInfo(TimeDay wt, Vector3 pos, float rot)
    {
        wallType = wt;
        position = pos;
        rotation = rot;
    }
}
