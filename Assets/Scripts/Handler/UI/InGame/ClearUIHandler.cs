using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClearUIHandler : MonoBehaviour
{
    private CanvasGroup cvs;
    private CanvasGroup clearBG;

    public Text clearTxt;
    
    [Space(10f)]
    public Button replayBtn;
    public Button homeBtn;
    public Button nextBtn;

    private void Start()
    {
        cvs = GetComponent<CanvasGroup>();
        clearBG = transform.GetChild(0).GetComponent<CanvasGroup>();

        clearBG.transform.localScale = new Vector3(0f, 0f, 1f);

        GameManager.Instance.onClear.AddListener(ShowClear);
        GameManager.Instance.onFailed.AddListener(ShowFail);

        homeBtn.onClick.AddListener(() => { GameManager.Instance.LoadScene("Title"); });
    }

    private void ShowClear()
    {
        clearTxt.text = "Stage Clear";

        cvs.blocksRaycasts = true;

        Sequence seq = DOTween.Sequence()
            .Append(cvs.DOFade(1f, 0.55f))
            .AppendInterval(0.15f)
            .AppendCallback(() => clearBG.alpha = 1f)
            .Append(clearBG.transform.DOScale(Vector3.one, 0.46f).SetEase(Ease.OutBack))
            .AppendCallback(() => cvs.interactable = true);
    }

    private void ShowFail()
    {
        clearTxt.text = "Stage Failed";

        cvs.blocksRaycasts = true;

        Sequence seq = DOTween.Sequence()
            .Append(cvs.DOFade(1f, 0.55f))
            .AppendInterval(0.15f)
            .AppendCallback(() => clearBG.alpha = 1f)
            .Append(clearBG.transform.DOScale(Vector3.one, 0.46f).SetEase(Ease.OutBack))
            .AppendCallback(() => cvs.interactable = true);
    }
}
