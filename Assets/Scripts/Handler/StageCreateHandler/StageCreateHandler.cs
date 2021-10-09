using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreateHandler : MonoBehaviour
{
    [SerializeField] private StageInfo stageInfo;

    [Header("챕터 정보들")]
    [SerializeField] public List<ChapterInfo> chapters = new List<ChapterInfo>();

    [Header("생성한 옵젝들의 부모")]
    public Transform tileTrans;
    public Transform wallTrans;
    public Transform objTrans;

    [Header("타일 프리팹")]
    public List<GameObject> tileObjs = new List<GameObject>();

    [Header("벽 프리팹")]
    public List<GameObject> wallObjs = new List<GameObject>();

    [Header("옵젝 프리팹")]
    public List<GameObject> stageObjs = new List<GameObject>();

    private void Start()
    {
        int chapterIdx = GameManager.Instance.chapter;
        int stageIdx = GameManager.Instance.stage;

        stageInfo = null;
        stageInfo = chapters[chapterIdx].stages[stageIdx].stage;

        if (stageInfo != null)
        {
            CreateStage();
        }
        else
        {
            Debug.LogError("해당 스테이지를 불러올 수 없습니다!");
        }
    }

    private void CreateStage()
    {
        int createIdx;

        Camera.main.orthographicSize = stageInfo.cameraSize;

        // 타일 생성
        foreach (var tileInfo in stageInfo.tileInfos)
        {
            createIdx = (int)tileInfo.tileType;

            Instantiate(tileObjs[createIdx], tileInfo.position, Quaternion.identity, tileTrans);
        }

        // 벽 생성
        foreach (var wallInfo in stageInfo.wallInfos)
        {
            createIdx = (int)wallInfo.wallType;

            Instantiate(wallObjs[createIdx], wallInfo.position, Quaternion.AngleAxis(wallInfo.rotation, Vector3.forward), wallTrans);
        }

        foreach (var objInfo in stageInfo.objInfos)
        {
            createIdx = (int)objInfo.objType;

            Instantiate(stageObjs[createIdx], objInfo.position, Quaternion.identity, objTrans);
        }
    }
}
