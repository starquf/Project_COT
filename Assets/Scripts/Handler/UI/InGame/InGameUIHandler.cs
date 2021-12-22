using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameUIHandler : MonoBehaviour
{
    // 인 게임 전체적인 UI를 담당하는 스크립트

    public Text moveText;
    public Text timeText;
    public Image timeImg;

    private TimeDay prevDay = TimeDay.DAWN;
    private Color timeCol;

    private int prevMove = 0;

    [Header("인디케이터")]
    public GameObject indicatorObj;
    public Transform canvas;

    private readonly Vector3 bigScale = new Vector3(1.2f, 1.2f, 1f);
    private readonly Vector3 indiEndPos = new Vector3(0f, 30f);

    private void Start()
    {
        GameManager.Instance.onUpdateUI.AddListener(UpdateUI);

        PoolManager.CreatePool<Indicator>(indicatorObj, transform, 3);

        InitUI();
    }

    private void InitUI()
    {
        int tl = GameManager.Instance.timeLimit;
        int ml = GameManager.Instance.moveLimit;

        moveText.text = ml.ToString();
        timeText.text = $"D-{(tl == 0 ? "Day" : tl.ToString())}";

        prevMove = ml;
    }

    public void UpdateUI()
    {
        int tl = GameManager.Instance.timeLimit;
        int ml = GameManager.Instance.moveLimit;

        moveText.text = ml.ToString();
        timeText.text = $"D-{(tl == 0 ? "Day" : tl.ToString())}";

        var td = GameManager.Instance.timeDayhandler.currentTimeDay;
        var changeDur = GameManager.Instance.timeDayhandler.ColorChangeDur;

        if (prevMove != ml)
        {
            prevMove = ml;

            HighlightText(moveText);
            ShowIndicator(canvas.InverseTransformPoint(moveText.transform.position));
        }

        if (prevDay.Equals(td))
            return;

        prevDay = td;
        timeCol = GameManager.Instance.timeDayhandler.GetTimeColor(td);


        switch (td)
        {
            case TimeDay.DAWN:

                timeImg.fillAmount = 1f;

                timeImg.DOFade(1f, 0.44f).From(0f);
                //.SetEase(Ease);

                timeImg.DOColor(timeCol, changeDur);

                HighlightText(timeText);
                ShowIndicator(canvas.InverseTransformPoint(timeText.transform.position));

                break;

            case TimeDay.MORNING:

                DOTween.To(() => timeImg.fillAmount, x => timeImg.fillAmount = x, 1f / 3f * 2f, changeDur)
                    .SetEase(Ease.OutQuad);

                timeImg.DOColor(timeCol, changeDur);

                break;

            case TimeDay.AFTERNOON:

                DOTween.To(() => timeImg.fillAmount, x => timeImg.fillAmount = x, 1f / 3f, changeDur)
                    .SetEase(Ease.OutQuad);

                timeImg.DOColor(timeCol, changeDur);

                break;

            case TimeDay.NIGHT:

                DOTween.To(() => timeImg.fillAmount, x => timeImg.fillAmount = x, 0f, changeDur)
                    .SetEase(Ease.OutQuad);

                timeImg.DOColor(timeCol, changeDur);

                if (tl == 0)
                {
                    timeText.DOColor(Color.red, 0.45f)
                        .SetEase(Ease.Linear)
                        .SetLoops(-1, LoopType.Yoyo);
                }

                break;
        }
    }

    private void ShowIndicator(Vector3 startPos)
    {
        startPos.y += 30f;

        Indicator indi = PoolManager.GetItem<Indicator>();

        indi.transform.localPosition = startPos;
        indi.transform.DOLocalMove(startPos + indiEndPos, 0.6f);
    }

    private void HighlightText(Text txt)
    {
        Sequence seq = DOTween.Sequence()
            .Append(txt.transform.DOScale(bigScale, 0.1f))
            .Append(txt.transform.DOScale(Vector3.one, 0.1f));
    }
}
