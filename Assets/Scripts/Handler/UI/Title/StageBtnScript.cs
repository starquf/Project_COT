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

    public List<Image> lineImgs = new List<Image>();
    private List<Tween> lineChangeTween;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        cvs = GetComponent<CanvasGroup>();

        lineChangeTween = new List<Tween>();
    }

    public void ShowStage(float delay)
    {
        changeTween?.Kill();

        cvs.alpha = 0f;
        cvs.interactable = false;


        changeTween = cvs.DOFade(1f, 0.9f)
            .SetDelay(delay)
            .SetEase(Ease.Linear)
            .OnStart(() => ShowLine())
            .OnComplete(() => { cvs.interactable = true; });
    }

    private void ShowLine()
    {
        for (int i = 0; i < lineChangeTween.Count; i++)
        {
            lineChangeTween[i].Kill();
        }

        lineChangeTween.Clear();

        for (int i = 0; i < lineImgs.Count; i++)
        {
            var color = lineImgs[i].color;
            color.a = 0f;

            lineImgs[i].color = color;

            lineChangeTween.Add(lineImgs[i].DOFade(1f, 0.3f).SetDelay(i * 0.1f + 0.1f).SetEase(Ease.Linear));
        }
    }
}
