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
                    Debug.LogError("���Ӹ޴����� �������� �ʽ��ϴ�!");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)                       // ���� instance�� ����ִٸ�
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)                   // ������� ������ instance�� �ڽ��� �ƴ϶��
        {
            Destroy(this.gameObject);
        }

        SetResolution();
        Application.targetFrameRate = 60;
    }

    private void SetResolution()
    {
        int setWidth = 1080; // ����� ���� �ʺ�
        int setHeight = 1920; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)                      // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);                  // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);// ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);                // ���ο� Rect ����
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
