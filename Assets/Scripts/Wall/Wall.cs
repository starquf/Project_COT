using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wall : MonoBehaviour
{
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

    private void InitWall()
    {
        if (wallTime.Equals(TimeDay.DAWN))
        {
            coll.enabled = false;
            sr.color = transColor;
        }
    }

    private void OnChangeTime(TimeDay time, Color dayColor, float changeDur)
    {
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

    private IEnumerator EnableWall(bool enable, float changeDur)
    {
        sr.DOFade(enable ? 1f : 0f, changeDur + 0.1f);
        yield return changeWait;
        coll.enabled = enable;
    }
}
