using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;

public class Jem : StageObj
{
    public Transform moveObj;
    public SpriteRenderer jemSr;
    public Light2D l2d;

    private Tween moveTween;

    private readonly float changeDur = 0.6f;

    private void Start()
    {
        moveTween = moveObj.DOMoveY(transform.position.y + 0.05f, changeDur + 0.1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }

    public override void Interact()
    {
        GameManager.Instance.isGemCollected = true;

        transform.GetComponent<Collider2D>().enabled = false;

        moveTween.Kill();


        float startY = transform.position.y;

        Sequence seq = DOTween.Sequence()
            .Append(moveObj.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.15f))
            .Append(moveObj.DOScale(Vector3.one, 0.2f))
            .Insert(0f, moveObj.DOMoveY(startY + 0.5f, 0.25f))
            .Insert(0.25f, moveObj.DOMoveY(startY, 0.4f).SetEase(Ease.InQuad));

        DOTween.To(() => l2d.intensity, x => l2d.intensity = x, 0f, changeDur);

        jemSr.DOFade(0f, changeDur)
            .SetEase(Ease.Linear);
    }
}
