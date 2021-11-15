using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageInfoPanelHandler : MonoBehaviour
{
    private CanvasGroup cvs;

    public Text stageText;
    public Text timeLimitText;
    public Text moveLimitText;

    private void Start()
    {
        cvs = GetComponent<CanvasGroup>();

        cvs.alpha = 1f;

        stageText.text = $"Stage {GameManager.Instance.chapter + 1}-{GameManager.Instance.stage + 1}";

        timeLimitText.text = $"D-{GameManager.Instance.timeLimit}";
        moveLimitText.text = GameManager.Instance.moveLimit.ToString();

        cvs.DOFade(0f, 1f)
            .SetDelay(1.1f)
            .SetEase(Ease.Linear);
    }
}
