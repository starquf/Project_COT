using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIHandler : MonoBehaviour
{
    // ������ ������ ����� UI���� ������ �Ǵ� �θ� ��ũ��Ʈ

    protected CanvasGroup cvs;
    public bool isOpened = false;

    // UI�� ���� �� ȣ��
    public event Action onOpenUI;
    // UI�� ���� �� ȣ��
    public event Action onCloseUI;

    protected virtual void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
    }

    // �� UI�� �� �� �����ϴ� �Լ�
    public virtual void Open()
    {
        onOpenUI?.Invoke();

        isOpened = true;

        cvs.blocksRaycasts = true;
        cvs.interactable = true;
    }

    // �� UI�� ���� �� �����ϴ� �Լ�
    public virtual void Close()
    {
        onCloseUI?.Invoke();

        isOpened = false;

        cvs.blocksRaycasts = false;
        cvs.interactable = false;
    }

    // �ٸ� UI�� ���� �ڽ��� ���� �� ���Ǵ� �Լ�
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

    // UI�� ���� ������ �� �� �����Ӹ��� ����Ǵ� �Լ�
    protected virtual void OnOpenUI()
    {
        // ��� �߰��ٶ�
    }
}
