using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class ChapterSelectUIHandler : UIHandler
{
    // 챕터 선택을 담당하는 UI핸들러

    private float sizeX = 650f;

    private readonly Vector3 bigSize = new Vector3(1f, 1f, 1f);
    private readonly Vector3 smallSize = new Vector3(0.8f, 0.8f, 1f);

    private Vector3 dragStartPos = Vector3.zero;
    private Vector3 dragPos = Vector3.zero;

    [SerializeField] private List<ChapterUI> chapterUIs = new List<ChapterUI>();

    private int maxChapterIdx;
    private int currentChapterIdx = 0;

    private Tween moveChapter;

    public UIHandler stageSelectHandler;

    [Header("버튼UI")]
    public CanvasGroup buttonUI;
    private ButtonUIHandler buttonUIHandler;

    [Header("뒷배경")]
    public List<CanvasGroup> bgGroup = new List<CanvasGroup>();

    private bool canInteract = false;

    private void Start()
    {
        #region Save Init
        GameInfoVO gameInfo = GameManager.Instance.gameInfo;

        if (gameInfo.chapters.Count < chapterUIs.Count)
        {
            while (gameInfo.chapters.Count < chapterUIs.Count)
            {
                gameInfo.chapters.Add(new ChapterInfoVO());
            }

            GameManager.Instance.SaveGameInfo();
        }
        #endregion

        LockChapter(gameInfo.jemCount);

        buttonUIHandler = buttonUI.GetComponent<ButtonUIHandler>();

        buttonUI.alpha = 0f;
        buttonUI.interactable = false;
        buttonUI.blocksRaycasts = false;

        maxChapterIdx = chapterUIs.Count - 1;

        for (int i = 0; i < chapterUIs.Count; i++)
        {
            int a = i;

            chapterUIs[i].transform.localScale = i > 0 ? smallSize : bigSize;
            chapterUIs[i].SetInteract(i == 0);

            chapterUIs[i].btn.onClick.AddListener(() =>
            {
                EndDrag();

                Action close = () => { };

                close = () =>
                {
                    chapterUIs[a].CloseStage();
                    stageSelectHandler.onCloseUI -= close;
                };

                stageSelectHandler.onCloseUI += close;

                chapterUIs[a].ShowStage();
                ShowStageUI();
            });
        }
    }

    // 보석을 비교하여 챕터를 잠근다
    private void LockChapter(int jemCount)
    {
        for (int i = 0; i < chapterUIs.Count; i++)
        {
            if (chapterUIs[i].requireJem > jemCount)
            {
                chapterUIs[i].SetLock();
            }
        }
    }

    protected override void OnOpenUI()
    {
        // 상호작용 할 수 없다면
        if (!canInteract) return;

        if (Input.GetMouseButtonDown(0))
        {
            StartDrag();
        }

        if (Input.GetMouseButton(0))
        {
            OnDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    // 드레그 시작
    private void StartDrag()
    {
        moveChapter?.Kill();
        dragStartPos.x = Input.mousePosition.x;
    }

    // 드레그 도중
    private void OnDrag()
    {
        dragPos.x = Input.mousePosition.x;

        // 드레그 하는 방향벡터
        Vector3 dir = dragPos - dragStartPos;

        // 드레그 하는 만큼 이동
        transform.localPosition += dir.normalized * dir.magnitude;
        // 드레그 할 수 있는 최대값 조정
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -sizeX * maxChapterIdx, 0f), 
                                  transform.localPosition.y);

        // 이동 후 다시 초기화
        dragStartPos = dragPos;

        // 현재 선택된 챕터의 크기를 넘어서 이동을 했다면
        if (transform.localPosition.x < (currentChapterIdx * -sizeX) - (sizeX / 2f))
        {
            chapterUIs[currentChapterIdx].transform.DOScale(smallSize, 0.33f);

            // 다음 챕터를 가르킨다
            currentChapterIdx++;
            chapterUIs[currentChapterIdx].transform.DOScale(bigSize, 0.33f);

            for (int i = 0; i < chapterUIs.Count; i++)
            {
                chapterUIs[i].SetInteract(i == currentChapterIdx);
            }
        }

        // 현재 선택된 챕터의 크기를 넘어서 이동을 했다면
        if (transform.localPosition.x > (currentChapterIdx * -sizeX) + (sizeX / 2f))
        {
            chapterUIs[currentChapterIdx].transform.DOScale(smallSize, 0.33f);

            // 이전 챕터를 가르킨다
            currentChapterIdx--;
            chapterUIs[currentChapterIdx].transform.DOScale(bigSize, 0.33f);

            for (int i = 0; i < chapterUIs.Count; i++)
            {
                chapterUIs[i].SetInteract(i == currentChapterIdx);
            }
        }
    }

    // 드레그 끝
    private void EndDrag()
    {
        ShowBG(currentChapterIdx);

        // 선택된 챕터를 현재 가르키는 챕터로 바꾼다
        GameManager.Instance.chapter = currentChapterIdx;

        // 챕터 하이라이트 연출
        moveChapter = transform.DOLocalMoveX(-sizeX * currentChapterIdx, 0.33f);
    }

    // 스테이지를 선택했을 때
    private void ShowStageUI()
    {
        stageSelectHandler.Open();
        isOpened = false;
    }

    public override void Open()
    {
        base.Open();
        this.transform.DOLocalMoveY(transform.localPosition.y + 1920f, 1.33f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => {
                canInteract = true;
            });

        buttonUI.DOFade(1f, 0.5f)
            .SetDelay(1f);

        buttonUIHandler.topPanel.DOLocalMoveY(350f, 0.63f)
            .SetDelay(1f)
            .From(true)
            .OnStart(() => {
                //buttonUI.alpha = 1f;
                buttonUI.interactable = true;
                buttonUI.blocksRaycasts = true;
                ShowBG(currentChapterIdx);
            })
            .SetEase(Ease.OutSine);

        buttonUIHandler.bottomPanel.DOLocalMoveY(-350f, 0.63f)
            .SetDelay(1f)
            .From(true)
            .SetEase(Ease.OutSine);
    }

    // 배경을 보여주는 함수
    private void ShowBG(int idx)
    {
        for (int i = 0; i < bgGroup.Count; i++)
        {
            bgGroup[i].DOFade((i.Equals(idx) ? 1f : 0f), 0.7f)
                .SetEase(Ease.OutSine);
        }
    }
}
