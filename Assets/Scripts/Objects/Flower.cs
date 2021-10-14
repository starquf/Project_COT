using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Flower : StageObj
{
    public SpriteRenderer flowerSr;

    private Vector3 normalSize;
    private Vector3 bigSize;

    private void Start()
    {
        normalSize = flowerSr.transform.localScale;
        bigSize = new Vector3(1.2f, 1.2f, 1f);

        GameManager.Instance.onChangeTime.AddListener((time, color, dur) =>
        {
            if (time.Equals(TimeDay.AFTERNOON))
            {
                flowerSr.DOFade(0f, dur).SetEase(Ease.OutQuad);
                flowerSr.transform.DOScale(Vector3.zero, dur).SetEase(Ease.InQuad);
            }
            else if (time.Equals(TimeDay.DAWN))
            {
                flowerSr.DOFade(1f, dur);
                flowerSr.transform.DOScale(normalSize, dur).SetEase(Ease.OutBack);
            }
        });
    }

    public override bool CheckMoveable(Vector3 dir)
    {
        TimeDay time = GameManager.Instance.timeDayhandler.currentTimeDay;

        return time.Equals(TimeDay.NIGHT) || time.Equals(TimeDay.AFTERNOON);
    }

    public override void Interact()
    {
        Sequence bigSeq = DOTween.Sequence()
            .Append(transform.DOScale(bigSize, 0.2f))
            .Append(transform.DOScale(Vector3.one, 0.2f));
    }
}
