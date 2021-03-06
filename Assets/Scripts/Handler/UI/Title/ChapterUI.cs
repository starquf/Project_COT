using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChapterUI : MonoBehaviour
{
    // é?? ?????? ?⺻ ????

    private CanvasGroup cvs;

    public Image image = null;
    private Shadow imgShadow = null;

    public Button btn = null;

    public float rotateDur = 0.5f;
    private WaitForSeconds changeColorWait;

    public CanvasGroup lockBG;
    private bool isLocked = false;

    public Text reqText;
    public int requireJem;

    private Color originalColor;
    public Color changeColor;

    private readonly Vector3 originSize = new Vector3(1f, 1f, 1f);
    private readonly Vector3 bigSize = new Vector3(1.15f, 1.15f, 1f);
    private readonly Vector3 rot = new Vector3(0f, 180f, 0f);

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
        imgShadow = image.GetComponent<Shadow>();

        reqText.text = requireJem.ToString();
        lockBG.alpha = 0f;
    }

    private void Start()
    {
        originalColor = image.color;
        changeColorWait = new WaitForSeconds(rotateDur / 2f);
    }

    // é?Ͱ? ?????? ????
    public void ShowStage()
    {
        bool b = false;

        image.transform.DORotate(rot, rotateDur).OnUpdate(() => {
            if (image.transform.eulerAngles.y > 90f && !b)
            {
                b = true;

                image.color = changeColor;
                imgShadow.effectDistance = new Vector2(-imgShadow.effectDistance.x, imgShadow.effectDistance.y);
            }
        });

        image.transform.DOScale(bigSize, rotateDur).SetEase(Ease.OutBack);
    }

    // ?????? ????
    public void CloseStage()
    {
        bool b = false;

        image.transform.DORotate(Vector3.zero, rotateDur).OnUpdate(() => {
            if (image.transform.eulerAngles.y < 90f && !b)
            {
                b = true;

                image.color = originalColor;
                imgShadow.effectDistance = new Vector2(-imgShadow.effectDistance.x, imgShadow.effectDistance.y);
            }
        });

        image.transform.DOScale(originSize, rotateDur);
    }

    // ??ȣ?ۿ? ???? ????
    public void SetInteract(bool active)
    {
        if (isLocked) return;

        cvs.interactable = active;
        cvs.blocksRaycasts = active;
    }

    // ???? é?͸? ???״? ?Լ?
    public void SetLock()
    {
        SetInteract(false);

        isLocked = true;
        lockBG.alpha = 1f;
    }
}
