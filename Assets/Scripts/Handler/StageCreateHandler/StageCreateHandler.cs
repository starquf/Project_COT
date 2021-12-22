using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreateHandler : MonoBehaviour
{
    // �������� ������ �������� ���� ���۵� �� ���������� �������ִ� ��ũ��Ʈ

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

    [Header("������ ����")]
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

            Instantiate(tileObjs[createIdx], tileInfo.position + offset, Quaternion.identity, tileTrans);
        }

        // �� ����
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
