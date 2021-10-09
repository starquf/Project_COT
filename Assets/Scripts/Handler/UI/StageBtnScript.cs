using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageBtnScript : MonoBehaviour
{
    private CanvasGroup cvs;
    public Button button;

    private Tween changeTween;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        cvs = GetComponent<CanvasGroup>();
    }

    public void ShowStage(float delay)
    {
        changeTween?.Kill();

        cvs.alpha = 0f;
        cvs.interactable = false;

        changeTween = cvs.DOFade(1f, 0.9f).SetDelay(delay).OnComplete(() => { cvs.interactable = true; });
    }
}
