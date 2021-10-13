using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;

public class SunHandler : MonoBehaviour
{
    private SpriteRenderer sr;
    private Transform lightTrans;
    private Light2D l2D;

    [Header("태양/달 이미지")]
    public Sprite sunSpr;
    public Sprite moonSpr;

    [Space(10f)]
    public Color sunColor;
    public Color sunSetColor;
    public Color moonColor;

    [Header("스테이지의 중심")]
    public Transform lightTarget;

    [Header("이동 반경")]
    public Vector2 moveX = new Vector2(-5f, 5f);    // min, max

    private void Start()
    {
        lightTrans = transform.GetChild(0);

        sr = GetComponent<SpriteRenderer>();
        l2D = lightTrans.GetComponent<Light2D>();

        ChangeLightDir(lightTarget.position);
        GameManager.Instance.onChangeTime.AddListener(ShowEffect);
    }

    public void ShowEffect(TimeDay currentDay, Color skyCol, float changeDur)
    {
        switch (currentDay)
        {
            case TimeDay.DAWN:

                ChangePos(moveX.x - 4f, (changeDur / 2f), () => ChangeLightDir(lightTarget.position), () => 
                {
                    transform.position = new Vector3(moveX.y + 4f, transform.position.y);
                    ChangeLightDir(lightTarget.position);
                    ChangeLightPar(sunColor, (changeDur / 2f), intensity:1.2f);

                    sr.sprite = sunSpr;

                    ChangePos(moveX.y, (changeDur / 2f), () => ChangeLightDir(lightTarget.position));
                }, Ease.InQuad);

                break;

            case TimeDay.MORNING:

                ChangeLightPar(sunColor, changeDur, shadow:0.1f);
                ChangePos(((moveX.x + moveX.y) / 2f), changeDur, () => ChangeLightDir(lightTarget.position));

                break;

            case TimeDay.AFTERNOON:

                ChangeLightPar(sunSetColor, changeDur, shadow:0.5f);
                ChangePos(moveX.x, changeDur, () => ChangeLightDir(lightTarget.position));

                break;

            case TimeDay.NIGHT:

                ChangePos(moveX.x - 3f, (changeDur / 2f), () => ChangeLightDir(lightTarget.position), () =>
                {
                    transform.position = new Vector3(moveX.y + 3f, transform.position.y);
                    ChangeLightDir(lightTarget.position);

                    sr.sprite = moonSpr;

                    l2D.intensity = 0f;
                    ChangeLightPar(moonColor, (changeDur / 2f), 0.6f, 0.7f);

                    ChangePos(((moveX.x + moveX.y) / 2f) + 1f, (changeDur / 2f) + 0.2f, () => ChangeLightDir(lightTarget.position));
                }, Ease.InSine);

                break;
        }
    }

    private void ChangePos(float posX, float dur, Action onChange, Ease ease = Ease.OutQuad)
    {
        transform.DOMoveX(posX, dur)
            .OnUpdate(() => onChange.Invoke())
            .SetEase(ease);
    }

    private void ChangePos(float posX, float dur, Action onChange, Action onEnd, Ease ease = Ease.OutQuad)
    {
        transform.DOMoveX(posX, dur)
            .OnUpdate(() => onChange.Invoke())
            .OnComplete(() => onEnd.Invoke())
            .SetEase(ease);
    }

    private void ChangeLightDir(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;

        lightTrans.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void ChangeLightPar(Color lightColor, float dur, float intensity = 1f, float shadow = 0.3f)
    {
        l2D.color = lightColor;
        DOTween.To(() => l2D.intensity, x => l2D.intensity = x, intensity, dur);
        DOTween.To(() => l2D.shadowIntensity, x => l2D.shadowIntensity = x, shadow, dur);
    }
}
