using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        int tl = GameManager.Instance.timeLimit;
        int ml = GameManager.Instance.moveLimit;

        moveText.text = ml.ToString();
        timeText.text = $"D-{(tl == 0 ? "Day" : tl.ToString())}";
    }
}
