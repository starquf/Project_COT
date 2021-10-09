using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreateHandler : MonoBehaviour
{
    [SerializeField] private StageInfo stageInfo;

    [Header("é�� ������")]
    [SerializeField] public List<ChapterInfo> chapters = new List<ChapterInfo>();

    [Header("������ �������� �θ�")]
    public Transform tileTrans;
    public Transform wallTrans;
    public Transform objTrans;

    [Header("Ÿ�� ������")]
    public List<GameObject> tileObjs = new List<GameObject>();

    [Header("�� ������")]
    public List<GameObject> wallObjs = new List<GameObject>();

    [Header("���� ������")]
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
            Debug.LogError("�ش� ���������� �ҷ��� �� �����ϴ�!");
        }
    }

    private void CreateStage()
    {
        int createIdx;

        Camera.main.orthographicSize = stageInfo.cameraSize;

        // Ÿ�� ����
        foreach (var tileInfo in stageInfo.tileInfos)
        {
            createIdx = (int)tileInfo.tileType;

            Instantiate(tileObjs[createIdx], tileInfo.position, Quaternion.identity, tileTrans);
        }

        // �� ����
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
