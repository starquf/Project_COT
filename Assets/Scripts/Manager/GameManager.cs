using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class OnChangeTimeHandler : UnityEvent<TimeDay, Color, float>
{ }

public class GameManager : MonoBehaviour
{
    #region SingleTon
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    Debug.LogError("게임메니져가 존재하지 않습니다!");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)                       // 만약 instance가 비어있다면
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)                   // 비어있진 않은데 instance가 자신이 아니라면
        {
            Destroy(this.gameObject);
        }

        SetResolution();
        Application.targetFrameRate = 60;
    }

    private void SetResolution()
    {
        int setWidth = 1080; // 사용자 설정 너비
        int setHeight = 1920; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)                      // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);                  // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);// 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);                // 새로운 Rect 적용
        }
    }
    #endregion

    public int chapter = 1;
    public int stage = 1;

    public int moveLimit = 10;
    public int timeLimit = 3;

    [HideInInspector]
    public TimeDayHander timeDayhandler = null;

    public OnChangeTimeHandler onChangeTime = new OnChangeTimeHandler();

    [HideInInspector]
    public UnityEvent onUpdateUI = new UnityEvent();
    [HideInInspector]
    public UnityEvent onClear = new UnityEvent();
    [HideInInspector]
    public UnityEvent onFailed = new UnityEvent();

    public void LoadScene(string sceneName)
    {
        moveLimit = 10;
        timeLimit = 3;

        DOTween.KillAll();
        SceneManager.LoadScene(sceneName);
    }
}
