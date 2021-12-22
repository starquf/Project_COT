using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialPanel : MonoBehaviour
{
    // 1-1 스테이지의 튜토리얼을 보여주는 UI 핸들러

    private CanvasGroup cvs;

    public Button closeBtn;

    private void Start()
    {
        cvs = GetComponent<CanvasGroup>();

        cvs.alpha = 0f;
        cvs.blocksRaycasts = false;
        cvs.interactable = false;

        ShowTutorial();

        closeBtn.onClick.AddListener(() =>
        {
            cvs.DOFade(0f, 1f)
                .SetEase(Ease.Linear);

            cvs.blocksRaycasts = false;
            cvs.interactable = false;
        });
    }

    private void ShowTutorial()
    {
        int chapter = GameManager.Instance.chapter;
        int stage = GameManager.Instance.stage;

        if (chapter == 0 && stage == 0)
        {
            cvs.alpha = 1f;
            cvs.blocksRaycasts = true;
            cvs.interactable = true;
        }
    }
}
