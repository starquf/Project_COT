using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClockHandler : MonoBehaviour
{
    public Button clockBtn = null;
    public Image nextTimeImg = null;

    public Transform timeImg = null;

    [Space(10f)]
    [SerializeField] private float timeChangeDur = 1f;

    private bool canChangeTime = true;

    private void Start()
    {
        clockBtn.onClick.AddListener(() =>
        {
            if (!canChangeTime || !GameManager.Instance.timeDayhandler.CanChangeTime) return;

            canChangeTime = false;

            TimeDayHander th = GameManager.Instance.timeDayhandler;
            int timeIdx = (int)th.currentTimeDay;

            timeIdx = (timeIdx + 1) % ((int)TimeDay.NIGHT + 1);

            if (((TimeDay)timeIdx).Equals(TimeDay.DAWN))
            {
                GameManager.Instance.timeLimit--;

                if (GameManager.Instance.timeLimit <= -1)
                {
                    // ¤»¤» ÀÌ°É ¸ø±ú´©
                    GameManager.Instance.timeLimit = 0;
                    return;
                }
            }

            th.ChangeTime((TimeDay)timeIdx);

            nextTimeImg.DOColor(th.GetTimeColor((TimeDay)((timeIdx + 1) % ((int)TimeDay.NIGHT + 1))), th.ColorChangeDur);
            // nextTimeImg.DOColor(th.GetTimeColor((TimeDay)timeIdx), th.ColorChangeDur);

            timeImg.DORotate(new Vector3(0f, 0f, -90f * timeIdx), timeChangeDur, RotateMode.Fast)
                .OnComplete(() => { canChangeTime = true; });

            //ChangeTimeText(th.currentTimeDay);

            GameManager.Instance.onUpdateUI.Invoke();
        });
    }

    private void ChangeTimeText(TimeDay time)
    {

        switch (time)
        {
            case TimeDay.DAWN:      // AM 5:00

                break;

            case TimeDay.MORNING:   // PM 1:00

                break;

            case TimeDay.AFTERNOON: // PM 7:00

                break;

            case TimeDay.NIGHT:     // AM 12:00

                break;
        }

        /*
        Sequence timeSeq = DOTween.Sequence()
            .Append(
                DOTween.To(() => currentTime,
                x =>
                {
                    currentTime = x;
                    timeText.text = $"{currentTime}:00";
                },
                targetTime,
                timeChangeDur))
            .InsertCallback(timeChangeDur / 2f, () => { ampmText.text = isAM ? "AM" : "PM"; })
            .AppendCallback(() => { currentTime = targetTime; });
        */
    }
}
