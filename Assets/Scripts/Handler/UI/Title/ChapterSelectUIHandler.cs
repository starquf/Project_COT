using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class ChapterSelectUIHandler : UIHandler
{
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

    [Header("��ưUI")]
    public CanvasGroup buttonUI;
    private ButtonUIHandler buttonUIHandler;

    [Header("�޹��")]
    public List<CanvasGroup> bgGroup = new List<CanvasGroup>();

    private bool canInteract = false;

    private void Start()
    {
        GameInfoVO gameInfo = GameManager.Instance.gameInfo;

        if (gameInfo.chapters.Count < chapterUIs.Count)
        {
            while (gameInfo.chapters.Count < chapterUIs.Count)
            {
                gameInfo.chapters.Add(new ChapterInfoVO());
            }

            GameManager.Instance.SaveGameInfo();
        }

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

    private void StartDrag()
    {
        moveChapter?.Kill();
        dragStartPos.x = Input.mousePosition.x;
    }

    private void OnDrag()
    {
        dragPos.x = Input.mousePosition.x;

        Vector3 dir = dragPos - dragStartPos;

        transform.localPosition += dir.normalized * dir.magnitude;
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -sizeX * maxChapterIdx, 0f), 
                                  transform.localPosition.y);

        dragStartPos = dragPos;

        if (transform.localPosition.x < (currentChapterIdx * -sizeX) - (sizeX / 2f))
        {
            chapterUIs[currentChapterIdx].transform.DOScale(smallSize, 0.33f);
            currentChapterIdx++;
            chapterUIs[currentChapterIdx].transform.DOScale(bigSize, 0.33f);

            for (int i = 0; i < chapterUIs.Count; i++)
            {
                chapterUIs[i].SetInteract(i == currentChapterIdx);
            }
        }

        if (transform.localPosition.x > (currentChapterIdx * -sizeX) + (sizeX / 2f))
        {
            chapterUIs[currentChapterIdx].transform.DOScale(smallSize, 0.33f);
            currentChapterIdx--;
            chapterUIs[currentChapterIdx].transform.DOScale(bigSize, 0.33f);

            for (int i = 0; i < chapterUIs.Count; i++)
            {
                chapterUIs[i].SetInteract(i == currentChapterIdx);
            }
        }
    }

    private void EndDrag()
    {
        ShowBG(currentChapterIdx);

        GameManager.Instance.chapter = currentChapterIdx;
        moveChapter = transform.DOLocalMoveX(-sizeX * currentChapterIdx, 0.33f);
    }

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

    private void ShowBG(int idx)
    {
        for (int i = 0; i < bgGroup.Count; i++)
        {
            bgGroup[i].DOFade((i.Equals(idx) ? 1f : 0f), 0.7f)
                .SetEase(Ease.OutSine);
        }
    }
}
