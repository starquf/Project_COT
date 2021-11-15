using System.IO;
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
        }
        else if (instance != this)                   // ������� ������ instance�� �ڽ��� �ƴ϶��
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        Init();

        //SetResolution();
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

    public GameInfoVO gameInfo;
    private readonly string fileName = "gameinfo.txt";
    private string path;

    public CanvasGroup loadImg;
    private readonly float changeDur = 0.8f;

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

    public bool isGemCollected = false;

    private void Init()
    {
        Screen.SetResolution(1440, 2960, true);
        Application.targetFrameRate = 60;

        gameInfo = new GameInfoVO();

        path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            LoadGameInfo();
        }
        else
        {
            SaveGameInfo();
        }
    }

    public void LoadScene(string sceneName)
    {
        loadImg.blocksRaycasts = true;

        loadImg.DOFade(1f, changeDur)
            .OnComplete(() =>
            {
                ClearScene();
                StartCoroutine(Loading(sceneName));
            })
            .SetEase(Ease.Linear);
    }

    private void ClearScene()
    {
        moveLimit = 10;
        timeLimit = 3;

        isGemCollected = false;

        DOTween.KillAll();
        PoolManager.ResetPool();

    }

    IEnumerator Loading(string sceneName)
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;

                loadImg.blocksRaycasts = false;
                loadImg.DOFade(0f, changeDur - 0.1f)
                    .SetEase(Ease.Linear);

                yield break;
            }
        }
    }

    public void LoadGameInfo()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            gameInfo = JsonUtility.FromJson<GameInfoVO>(json);
        }
    }

    [ContextMenu("���� ���� �����ϱ�")]
    public void SaveGameInfo()
    {
        string json = JsonUtility.ToJson(gameInfo);

        File.WriteAllText(path, json);
    }
}