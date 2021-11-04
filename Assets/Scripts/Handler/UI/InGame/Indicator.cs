using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Indicator : MonoBehaviour
{
    private Text indicatorText;

    public float showTime = 1f;

    private Tween fadeTween;

    private void Awake()
    {
        indicatorText = GetComponent<Text>();
    }

    private void Start()
    {
        fadeTween =
        indicatorText.DOFade(0f, showTime)
            .SetAutoKill(false)
            .From(1f)
            .SetEase(Ease.Linear)
            .OnComplete(() => gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        fadeTween.Restart();
    }

    public void SetText(string text)
    {
        indicatorText.text = text;
    }
}
