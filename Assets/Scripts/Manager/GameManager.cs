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
        }
        else if (instance != this)                   // 비어있진 않은데 instance가 자신이 아니라면
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

    [ContextMenu("현재 정보 저장하기")]
    public void SaveGameInfo()
    {
        string json = JsonUtility.ToJson(gameInfo);

        File.WriteAllText(path, json);
    }
}