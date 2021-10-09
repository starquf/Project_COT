using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI : MonoBehaviour
{
    protected CanvasGroup cvs;
    public bool isOpened = false;

    public event Action onOpenUI;
    public event Action onCloseUI;

    protected virtual void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
    }

    public virtual void Open()
    {
        onOpenUI?.Invoke();

        isOpened = true;

        cvs.blocksRaycasts = true;
        cvs.interactable = true;
    }

    public virtual void Close()
    {
        onCloseUI?.Invoke();

        isOpened = false;

        cvs.blocksRaycasts = false;
        cvs.interactable = false;
    }

    protected void OpenUI(UI ui)
    {
        ui.Open();
        this.Close();
    }

    protected virtual void Update()
    {
        if (!isOpened) return;
        OnOpenUI();
    }

    protected virtual void OnOpenUI()
    {
        // 기능 추가바람
    }
}
