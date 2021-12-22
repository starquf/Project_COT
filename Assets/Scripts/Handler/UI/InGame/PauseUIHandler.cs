using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIHandler : MonoBehaviour
{
    // 일시정지를 할 떄 보여주는 UI 핸들러

    public Button pauseBtn;

    [Space(10f)]
    public Button replayBtn;
    public Button homeBtn;
    public Button cancelBtn;

    private CanvasGroup cvs;

    private void Start()
    {
        cvs = GetComponent<CanvasGroup>();

        cvs.alpha = 0f;
        cvs.blocksRaycasts = false;
        cvs.interactable = false;

        pauseBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.onPause?.Invoke();

            cvs.alpha = 1f;
            cvs.blocksRaycasts = true;
            cvs.interactable = true;
        });

        cancelBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.onUnPause?.Invoke();

            cvs.alpha = 0f;
            cvs.blocksRaycasts = false;
            cvs.interactable = false;
        });

        replayBtn.onClick.AddListener(() => 
        {
            GameManager.Instance.LoadScene("InGame");

            cvs.blocksRaycasts = false;
            cvs.interactable = false;
        });

        homeBtn.onClick.AddListener(() => 
        {
            GameManager.Instance.LoadScene("Title");

            cvs.blocksRaycasts = false;
            cvs.interactable = false;
        });
    }
}
