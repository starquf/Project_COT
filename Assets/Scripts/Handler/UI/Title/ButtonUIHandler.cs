using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUIHandler : MonoBehaviour
{
    // 타이틀의 버튼을 담당하는 UI핸들러

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
