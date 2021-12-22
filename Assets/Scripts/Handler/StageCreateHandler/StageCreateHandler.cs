using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreateHandler : MonoBehaviour
{
    // 스테이지 정보를 바탕으로 씬이 시작될 때 스테이지를 조립해주는 스크립트

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

    [Header("오프셋 설정")]
    public Vector3 offset = Vector3.zero;

    private void Awake()
    {
        int chapterIdx = GameManager.Instance.chapter;
        int stageIdx = GameManager.Instance.stage;

        stageInfo = null;
        stageInfo = chapters[chapterIdx].stages[stageIdx].stage;

        if (stageInfo != null)
        {
            GameManager.Instance.moveLimit = stageInfo.moveLimit;
            GameManager.Instance.timeLimit = stageInfo.timeLimit;

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

            Instantiate(tileObjs[createIdx], tileInfo.position + offset, Quaternion.identity, tileTrans);
        }

        // 벽 생성
        foreach (var wallInfo in stageInfo.wallInfos)
        {
            createIdx = (int)wallInfo.wallType;

            Instantiate(wallObjs[createIdx], wallInfo.position + offset, Quaternion.AngleAxis(wallInfo.rotation, Vector3.forward), wallTrans);
        }

        foreach (var objInfo in stageInfo.objInfos)
        {
            createIdx = (int)objInfo.objType;

            Instantiate(stageObjs[createIdx], objInfo.position + offset, Quaternion.identity, objTrans);
        }
    }
}
