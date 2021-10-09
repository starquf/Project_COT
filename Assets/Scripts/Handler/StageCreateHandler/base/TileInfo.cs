using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileInfo
{
    public TileType tileType;
    public Vector3 position;

    public TileInfo()
    {
        tileType = TileType.NONE;
        position = Vector3.zero;
    }

    public TileInfo(TileType tileType, Vector3 pos)
    {
        this.tileType = tileType;
        position = pos;
    }
}
