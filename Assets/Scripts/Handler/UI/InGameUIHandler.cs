using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIHandler : MonoBehaviour
{
    public Text moveText;
    public Text timeText;

    private void Start()
    {
        GameManager.Instance.onUpdateUI.AddListener(UpdateUI);

        UpdateUI();
    }

    public void UpdateUI()
    {
        moveText.text = $"{GameManager.Instance.moveLimit}»∏";
        timeText.text = $"{GameManager.Instance.timeLimit}¿œ";
    }
}
