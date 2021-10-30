using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChapterUI : MonoBehaviour
{
    private CanvasGroup cvs;

    public Image image = null;
    public Button btn = null;

    public float rotateDur = 0.5f;
    private WaitForSeconds changeColorWait;

    private Color originalColor;
    public Color changeColor;

    private readonly Vector3 originSize = new Vector3(1f, 1f, 1f);
    private readonly Vector3 bigSize = new Vector3(1.15f, 1.15f, 1f);
    private readonly Vector3 rot = new Vector3(0f, 180f, 0f);

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        originalColor = image.color;
        changeColorWait = new WaitForSeconds(rotateDur / 2f);
    }

    public void ShowStage()
    {
        image.transform.DORotate(rot, rotateDur).OnUpdate(() => {
            if (image.transform.eulerAngles.y > 90f)
            {
                image.color = changeColor;
            }
        });

        image.transform.DOScale(bigSize, rotateDur).SetEase(Ease.OutBack);
    }

    public void CloseStage()
    {
        image.transform.DORotate(Vector3.zero, rotateDur).OnUpdate(() => {
            if (image.transform.eulerAngles.y < 90f)
            {
                image.color = originalColor;
            }
        });

        image.transform.DOScale(originSize, rotateDur);
    }

    public void SetInteract(bool active)
    {
        cvs.interactable = active;
        cvs.blocksRaycasts = active;
    }
}
