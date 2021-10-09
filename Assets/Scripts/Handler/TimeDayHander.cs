using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;


public class TimeDayHander : MonoBehaviour
{
    public TimeDay currentTimeDay = TimeDay.DAWN;

    [SerializeField] private List<Color> timeColors = new List<Color>();

    [SerializeField] private float colorChangeDur = 1f;
    public float ColorChangeDur => colorChangeDur;

    private void Start()
    {
        GameManager.Instance.timeDayhandler = this;
    }

    public void ChangeTime(TimeDay time)
    {
        int timeIdx = (int)time;
        currentTimeDay = time;

        Camera.main.DOColor(timeColors[timeIdx], colorChangeDur);

        GameManager.Instance.onChangeTime?.Invoke(time, timeColors[timeIdx], colorChangeDur);
    }

    public Color GetTimeColor(TimeDay time)
    {
        int timeIdx = (int)time;

        return timeColors[timeIdx];
    }
}
