using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageInfo
{
    public List<WallInfo> wallInfos = new List<WallInfo>();
    public List<TileInfo> tileInfos = new List<TileInfo>();
    public List<StageObjInfo> objInfos = new List<StageObjInfo>();

    public int moveLimit = 10;
    public int timeLimit = 3;
    public float cameraSize = 4.5f;
}
