using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleUIHandler : UIHandler
{
    // 타이틀을 보여주는 UI 핸들러

    public UIHandler chapterSelectHandler;
    public Text touchText;

    private Tween touchTween;

    private void Start()
    {
        isOpened = true;

        touchTween = touchText.DOFade(0f, 0.8f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
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

        touchTween.Kill();
        touchText.DOFade(0f, 0.33f);
    }
}
