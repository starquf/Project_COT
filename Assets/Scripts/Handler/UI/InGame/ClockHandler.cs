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

    [Header("시간 텍스트 설정")]
    public Text ampmText = null;
    public Text timeText = null;

    private bool canChangeTime = true;
    private int currentTime = 5;

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
                    // ㅋㅋ 이걸 못깨누
                    GameManager.Instance.timeLimit = 0;
                    return;
                }
            }

            th.ChangeTime((TimeDay)timeIdx);

            nextTimeImg.DOColor(th.GetTimeColor((TimeDay)((timeIdx + 1) % ((int)TimeDay.NIGHT + 1))), th.ColorChangeDur);

            timeImg.DORotate(new Vector3(0f, 0f, -90f * timeIdx), timeChangeDur, RotateMode.Fast)
                .OnComplete(() => { canChangeTime = true; });

            ChangeTimeText(th.currentTimeDay);

            GameManager.Instance.onUpdateUI.Invoke();
        });
    }

    private void ChangeTimeText(TimeDay time)
    {
        bool isAM = false;
        int targetTime = 0;

        switch (time)
        {
            case TimeDay.DAWN:      // AM 5:00
                isAM = true;

                currentTime = 0;
                targetTime = 5;

                break;

            case TimeDay.MORNING:   // PM 1:00
                isAM = false;
                targetTime = 1;

                break;

            case TimeDay.AFTERNOON: // PM 7:00
                isAM = false;
                targetTime = 7;

                break;

            case TimeDay.NIGHT:     // AM 12:00
                isAM = true;
                targetTime = 12;

                break;
        }

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
    }
}
