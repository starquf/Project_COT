using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour, IMoveable, IInteractable
{
    public TileType tileType;

    private readonly Vector3 bigScale = new Vector3(0.93f, 0.93f, 1f);
    private Vector3 originalScale = Vector3.zero;

    protected virtual void Start()
    {
        originalScale = transform.localScale;
    }

    public virtual void Interact()
    {
        Sequence bigSeq = DOTween.Sequence()
            .Append(transform.DOScale(bigScale, 0.15f))
            .Append(transform.DOScale(originalScale, 0.15f));
    }

    public virtual bool CheckMoveable(Vector3 dir)
    {
        return true;
    }
}
