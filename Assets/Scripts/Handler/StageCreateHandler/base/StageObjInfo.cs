using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageObjInfo
{
    public StageObjType objType;
    public Vector3 position;

    public StageObjInfo(StageObjType objType, Vector3 position)
    {
        this.objType = objType;
        this.position = position;
    }
}
