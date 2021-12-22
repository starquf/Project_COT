using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIHandler : MonoBehaviour
{
    // 열리고 닫히는 방식의 UI들의 바탕이 되는 부모 스크립트

    protected CanvasGroup cvs;
    public bool isOpened = false;

    // UI가 열릴 때 호출
    public event Action onOpenUI;
    // UI가 닫힐 때 호출
    public event Action onCloseUI;

    protected virtual void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
    }

    // 이 UI를 열 때 실행하는 함수
    public virtual void Open()
    {
        onOpenUI?.Invoke();

        isOpened = true;

        cvs.blocksRaycasts = true;
        cvs.interactable = true;
    }

    // 이 UI를 닫을 때 실행하는 함수
    public virtual void Close()
    {
        onCloseUI?.Invoke();

        isOpened = false;

        cvs.blocksRaycasts = false;
        cvs.interactable = false;
    }

    // 다른 UI를 열고 자신을 닫을 때 사용되는 함수
    protected void OpenUI(UIHandler ui)
    {
        ui.Open();
        this.Close();
    }

    protected virtual void Update()
    {
        if (!isOpened) return;
        OnOpenUI();
    }

    // UI가 열린 상태일 때 매 프레임마다 실행되는 함수
    protected virtual void OnOpenUI()
    {
        // 기능 추가바람
    }
}
