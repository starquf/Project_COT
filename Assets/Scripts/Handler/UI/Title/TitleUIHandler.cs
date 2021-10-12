using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleUIHandler : UI
{
    public UI chapterSelectHandler;

    private void Start()
    {
        isOpened = true;
    }

    protected override void OnOpenUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OpenUI(chapterSelectHandler);
        }
    }

    public override void Close()
    {
        base.Close();
        this.transform.DOLocalMoveY(transform.localPosition.y + 1920f, 1.6f).SetEase(Ease.InOutQuad);
    }
}
