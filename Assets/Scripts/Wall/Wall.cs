using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wall : MonoBehaviour
{
    // 벽 스크립트

    private Collider2D coll = null;
    private SpriteRenderer sr;

    public TimeDay wallTime;

    private Coroutine changeCor = null;

    private Color transColor = Color.white;

    private readonly WaitForSeconds changeWait = new WaitForSeconds(0.2f);

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        transColor = sr.color;
        transColor.a = 0f;

        GameManager.Instance.onChangeTime.AddListener(OnChangeTime);
        InitWall();
    }

    // 초기화
    private void InitWall()
    {
        if (wallTime.Equals(TimeDay.DAWN))
        {
            coll.enabled = false;
            sr.color = transColor;
        }
    }

    // 시간이 바뀔 떄
    private void OnChangeTime(TimeDay time, Color dayColor, float changeDur)
    {
        // 현재 벽의 색깔과 바뀌는 시간의 색깔이 같다면
        if (time.Equals(wallTime))
        {
            changeCor = StartCoroutine(EnableWall(false, changeDur));
        }
        else if (!coll.enabled)
        {
            if (changeCor != null)
            {
                StopCoroutine(changeCor);
            }

            sr.DOFade(1f, changeDur);
            coll.enabled = true;
        }
    }

    // 벽 콜라이더 on/off
    private IEnumerator EnableWall(bool enable, float changeDur)
    {
        sr.DOFade(enable ? 1f : 0f, changeDur + 0.1f);
        yield return changeWait;
        coll.enabled = enable;
    }
}
