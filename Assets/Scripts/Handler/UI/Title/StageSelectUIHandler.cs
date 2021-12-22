using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StageSelectUIHandler : UIHandler
{
    // 스테이지 UI를 관리하는 핸들러

    [Header("스테이지 UI 설정")]
    [SerializeField] private List<StageBtnScript> stageButtons = new List<StageBtnScript>();

    public Button cancelBtn;

    [Header("챕터 UI")]
    public UIHandler chapterSelectHandler;

    private Color colorTrans = new Color(1f, 1f, 1f, 0f);
    private Vector3 themeStartPos = Vector3.zero;

    private readonly string inGameScene = "InGame";

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
                GameManager.Instance.LoadScene(inGameScene);
            });
        }
    }

    public override void Open()
    {
        base.Open();

        cvs.alpha = 1;

        GameInfoVO gameInfo = GameManager.Instance.gameInfo;
        ChapterInfoVO chapterInfo = gameInfo.chapters[GameManager.Instance.chapter];
        StageInfoVO lastStageInfo = null;

        print(GameManager.Instance.chapter);

        for (int i = 0; i < stageButtons.Count; i++)
        {
            StageInfoVO stageInfo = chapterInfo.stages[i];

            stageButtons[i].ShowStar(stageInfo.starCount);
            stageButtons[i].ShowStage(i * 0.1f + 0.3f, lastStageInfo == null || lastStageInfo.isCleared);

            lastStageInfo = stageInfo;
        }
    }

    public override void Close()
    {
        base.Close();

        cvs.alpha = 0;
    }
}
