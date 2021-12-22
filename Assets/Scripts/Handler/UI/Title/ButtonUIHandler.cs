using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUIHandler : MonoBehaviour
{
    // Ÿ��Ʋ�� ��ư�� ����ϴ� UI�ڵ鷯

    public Transform topPanel;
    public Transform bottomPanel;

    public Text jemText;

    public Button settingBtn;

    public UIHandler settingHandler;

    private void Start()
    {
        jemText.text = GameManager.Instance.gameInfo.jemCount.ToString();

        settingBtn.onClick.AddListener(() => 
        {
            settingHandler.Open();
        });
    }
}
