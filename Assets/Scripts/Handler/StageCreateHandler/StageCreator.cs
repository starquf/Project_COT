using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageCreator : MonoBehaviour
{
    // [�ν����� ����] ���� ���������� �ִ� ��, Ÿ�ϵ��� ������ �����ͼ� �������� ������ ������ִ� ��ũ��Ʈ

    [Header("������ �θ�")]
    public Transform tileTrans;
    public Transform wallTrans;
    public Transform objTrans;

    private List<Tile> tiles = new List<Tile>();
    private List<Wall> walls = new List<Wall>();
    private List<StageObj> objs = new List<StageObj>();

    [Header("���� �������� ����")]
    [SerializeField] private StageInfo stageInfo;


    [ContextMenu("���� �������� ���� ��������")]
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
