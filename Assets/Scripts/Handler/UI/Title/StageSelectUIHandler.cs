using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StageSelectUIHandler : UI
{
    [Header("스테이지 UI 설정")]
    [SerializeField] private List<StageBtnScript> stageButtons = new List<StageBtnScript>();

    public Button cancelBtn;

    [Header("테마")]
    public Transform themeTrans;
    public Text themeName;

    [Header("챕터 UI")]
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

                // 해당 스테이지로 이동하는 로직 구현
                GameManager.Instance.LoadScene("InGame");
            });
        }

        themeStartPos = themeTrans.localPosition;
    }

    public override void Open()
    {
        base.Open();

        cvs.alpha = 1;

        themeTrans.localPosition = themeStartPos;
        themeTrans.DOLocalMoveX(-800f, 0.65f).From();

        for (int i = 0; i < stageButtons.Count; i++)
        {
            stageButtons[i].ShowStage(i / 7f + 0.3f);
        }
    }

    public override void Close()
    {
        base.Close();

        cvs.alpha = 0;
    }
}
