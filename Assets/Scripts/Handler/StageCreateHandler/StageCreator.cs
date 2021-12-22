using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageCreator : MonoBehaviour
{
    // [인스펙터 전용] 현재 스테이지에 있는 벽, 타일등의 정보를 가져와서 스테이지 정보를 만들어주는 스크립트

    [Header("가져올 부모")]
    public Transform tileTrans;
    public Transform wallTrans;
    public Transform objTrans;

    private List<Tile> tiles = new List<Tile>();
    private List<Wall> walls = new List<Wall>();
    private List<StageObj> objs = new List<StageObj>();

    [Header("나온 스테이지 정보")]
    [SerializeField] private StageInfo stageInfo;


    [ContextMenu("현재 스테이지 정보 가져오기")]
    public void GetStageData()
    {
        stageInfo = new StageInfo();

        tiles = tileTrans.GetComponentsInChildren<Tile>().ToList();
        walls = wallTrans.GetComponentsInChildren<Wall>().ToList();
        objs = objTrans.GetComponentsInChildren<StageObj>().ToList();

        foreach (var tile in tiles)
        {
            stageInfo.tileInfos.Add(new TileInfo(tile.tileType, tile.transform.position));
        }

        foreach (var wall in walls)
        {
            stageInfo.wallInfos.Add(new WallInfo(wall.wallTime, wall.transform.position, wall.transform.eulerAngles.z));
        }

        foreach (var obj in objs)
        {
            stageInfo.objInfos.Add(new StageObjInfo(obj.objType, obj.transform.position));
        }
    }
}
