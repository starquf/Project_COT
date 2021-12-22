using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClockHandler : MonoBehaviour
{
    // 시계 UI를 관리해주는 핸들러

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
            // 시간이 바뀌는 도중이거나 시간을 바꿀 수 없는 상태면 리턴
            if (!canChangeTime || !GameManager.Instance.timeDayhandler.CanChangeTime) return;

            canChangeTime = false;

            TimeDayHander th = GameManager.Instance.timeDayhandler;
            int timeIdx = (int)th.currentTimeDay;

            // 다음 시간을 나타내는 인덱스
            timeIdx = (timeIdx + 1) % ((int)TimeDay.NIGHT + 1);

            // 만약 다음 시간이 새벽이라면
            if (((TimeDay)timeIdx).Equals(TimeDay.DAWN))
            {
                GameManager.Instance.timeLimit--;

                if (GameManager.Instance.timeLimit <= -1)
                {
                    GameManager.Instance.timeLimit = 0;
                    return;
                }
            }

            th.ChangeTime((TimeDay)timeIdx);

            nextTimeImg.DOColor(th.GetTimeColor((TimeDay)((timeIdx + 1) % ((int)TimeDay.NIGHT + 1))), th.ColorChangeDur);

            timeImg.DORotate(new Vector3(0f, 0f, -90f * timeIdx), timeChangeDur, RotateMode.Fast)
                .OnComplete(() => { canChangeTime = true; });

            GameManager.Instance.onUpdateUI.Invoke();
        });
    }

    /*
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
    }
    */
}
