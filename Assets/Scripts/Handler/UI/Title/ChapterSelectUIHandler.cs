using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class ChapterSelectUIHandler : UI
{
    private float sizeX = 550f;

    private readonly Vector3 bigSize = new Vector3(0.9f, 0.9f, 1f);
    private readonly Vector3 smallSize = new Vector3(0.7f, 0.7f, 1f);

    private Vector3 dragStartPos = Vector3.zero;
    private Vector3 dragPos = Vector3.zero;

    [SerializeField] private List<ChapterUI> chapterUIs = new List<ChapterUI>();

    private int maxChapterIdx;
    private int currentChapterIdx = 0;

    private Tween moveChapter;

    public UI stageSelectHandler;

    private void Start()
    {
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

    protected override void OnOpenUI()
    {
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

        transform.localPosition += dir.normalized * dir.magnitude * 2f;
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
        this.transform.DOLocalMoveY(transform.localPosition.y + 1920f, 1.2f).SetEase(Ease.InOutQuad);
    }
}
