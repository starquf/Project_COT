using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndPos : StageObj
{
    private Vector3 bigScale;
    private Vector3 normalScale;

    private void Start()
    {
        normalScale = transform.localScale;
        bigScale = new Vector3(normalScale.x + 0.25f, normalScale.y + 0.25f, 1f);
    }

    public override void Interact()
    {
        Sequence bigSeq = DOTween.Sequence()
            .Append(transform.DOScale(bigScale, 0.17f))
            .Append(transform.DOScale(normalScale, 0.17f));

        GameManager.Instance.onClear?.Invoke();
    }
}
