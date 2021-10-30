using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StageSelectUIHandler : UI
{
    [Header("�������� UI ����")]
    [SerializeField] private List<StageBtnScript> stageButtons = new List<StageBtnScript>();

    public Button cancelBtn;

    [Header("é�� UI")]
    public UI chapterSelectHandler;

    private Color colorTrans = new Color(1f, 1f, 1f, 0f);
    private Vector3 themeStartPos = Vector3.zero;

    private void Start()
    {
        Close();

        cancelBtn.onClick.AddListener(() => 
        {
            chapterSelectHandler.isOpened = true;
            Close();
        });

        for (int i = 0; i < stageButtons.Count; i++)
        {
            int a = i;

            stageButtons[i].button.onClick.AddListener(() => 
            {
                GameManager.Instance.stage = a;

                // �ش� ���������� �̵��ϴ� ���� ����
                GameManager.Instance.LoadScene("InGame");
            });
        }
    }

    public override void Open()
    {
        base.Open();

        cvs.alpha = 1;

        for (int i = 0; i < stageButtons.Count; i++)
        {
            stageButtons[i].ShowStage(i * 0.2f + 0.3f);
        }
    }

    public override void Close()
    {
        base.Close();

        cvs.alpha = 0;
    }
}
