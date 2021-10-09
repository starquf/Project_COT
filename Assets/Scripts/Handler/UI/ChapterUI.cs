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
        image.transform.DORotate(new Vector3(0f, 180f), rotateDur).OnUpdate(() => {
            if (image.transform.eulerAngles.y > 90f)
            {
                image.color = changeColor;
            }
        });

        image.transform.DOScale(new Vector3(1.2f, 1.2f), rotateDur).SetEase(Ease.OutBack);
    }

    public void CloseStage()
    {
        image.transform.DORotate(new Vector3(0f, 0f), rotateDur).OnUpdate(() => {
            if (image.transform.eulerAngles.y < 90f)
            {
                image.color = originalColor;
            }
        });

        image.transform.DOScale(new Vector3(1f, 1f), rotateDur);
    }

    public void SetInteract(bool active)
    {
        cvs.interactable = active;
        cvs.blocksRaycasts = active;
    }
}
