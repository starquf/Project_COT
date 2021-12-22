using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageBtnScript : MonoBehaviour
{
    // 스테이지 버튼 프리팹 기본 정보

    private CanvasGroup cvs;
    public Button button;

    private Tween changeTween;

    public List<Image> starImgs = new List<Image>();
    public Sprite emptyStarImg;
    public Sprite fullStarImg;

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

    // 스테이지의 별을 보여준다
    public void ShowStar(int starCount)
    {
        for (int i = 0; i < starImgs.Count; i++)
        {
            starImgs[i].sprite = i <= starCount - 1 ? fullStarImg : emptyStarImg;
        }
    }

    // 스테이지 연출
    public void ShowStage(float delay, bool isShow)
    {
        changeTween?.Kill();

        cvs.alpha = 0f;
        cvs.interactable = false;

        if (isShow)
        {
            changeTween = cvs.DOFade(1f, 0.9f)
                .SetDelay(delay)
                .SetEase(Ease.Linear)
                .OnStart(() => ShowLine())
                .OnComplete(() => { cvs.interactable = true; });
        }
    }

    // 다음 스테이지로 이어지는 라인 연출
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
