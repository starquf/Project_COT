using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaterTile : Tile
{
    private SpriteRenderer iceSpr;

    private readonly Color colorTrans = new Color(1f, 1f, 1f, 0f);

    protected override void Start()
    {
        base.Start();

        iceSpr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        iceSpr.color = colorTrans;

        GameManager.Instance.onChangeTime.AddListener((time, color, dur) => 
        {
            if (time.Equals(TimeDay.NIGHT))
            {
                iceSpr.DOFade(1f, dur);
            }
            else
            {
                iceSpr.DOFade(0f, dur);
            }
        });
    }
}
