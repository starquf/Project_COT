using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;


public class TimeDayHander : MonoBehaviour
{
    // 시간을 담당하는 핸들러

    public TimeDay currentTimeDay { get; private set; } = TimeDay.DAWN;

    [SerializeField] private List<Color> timeColors = new List<Color>();

    [SerializeField] private float colorChangeDur = 1f;
    public float ColorChangeDur => colorChangeDur;

    public bool CanChangeTime { get; set; }

    private void Start()
    {
        CanChangeTime = true;
        GameManager.Instance.timeDayhandler = this;
    }

    // 시간 변경
    public void ChangeTime(TimeDay time)
    {
        if (!CanChangeTime) return;

        int timeIdx = (int)time;
        currentTimeDay = time;

        Camera.main.DOColor(timeColors[timeIdx], colorChangeDur);

        GameManager.Instance.onChangeTime?.Invoke(time, timeColors[timeIdx], colorChangeDur);
    }

    // 시간의 색 getter
    public Color GetTimeColor(TimeDay time)
    {
        int timeIdx = (int)time;

        return timeColors[timeIdx];
    }
}
