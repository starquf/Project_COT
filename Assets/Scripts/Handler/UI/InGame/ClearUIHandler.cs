using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClearUIHandler : MonoBehaviour
{
    // 스테이지가 클리어했을 때 나오는 UI

    private CanvasGroup cvs;
    private CanvasGroup clearBG;

    public Text clearTxt;
    public Color failColor;
    
    [Space(10f)]
    public Button replayBtn;
    public Button homeBtn;
    public Button nextBtn;

    [Header("별 관련")]
    public Transform starBG;

    public Sprite emptyStarImg;
    public Sprite fullStarImg;

    private List<Image> stars = new List<Image>();

    [Header("소리 관련")]
    public GameObject clearSound;
    public GameObject failSound;

    private void Start()
    {
        cvs = GetComponent<CanvasGroup>();
        clearBG = transform.GetChild(0).GetComponent<CanvasGroup>();

        #region Init UI
        starBG.GetComponentsInChildren(stars);
        stars.RemoveAt(0);

        SetStar(0);

        cvs.alpha = 0f;
        clearBG.alpha = 0f;
        #endregion

        GameManager.Instance.onClear.AddListener(ShowClear);
        GameManager.Instance.onFailed.AddListener(ShowFail);

        replayBtn.onClick.AddListener(() => { GameManager.Instance.LoadScene("InGame"); });
        homeBtn.onClick.AddListener(() => { GameManager.Instance.LoadScene("Title"); });
        nextBtn.onClick.AddListener(() => {
            GameManager.Instance.stage++;
            GameManager.Instance.LoadScene("InGame"); 
        });
    }

    // 클리어했을 때
    private void ShowClear()
    {
        clearTxt.text = "C L E A R";

        SetStar(GameManager.Instance.isGemCollected ? 3 : 2);
        cvs.blocksRaycasts = true;

        int chap = GameManager.Instance.chapter;
        int stag = GameManager.Instance.stage;

        GameInfoVO gameInfo = GameManager.Instance.gameInfo;
        StageInfoVO stageInfo = gameInfo.chapters[chap].stages[stag];

        if (stageInfo.starCount < 3)
        {
            if (GameManager.Instance.isGemCollected)
                gameInfo.jemCount++;
        }

        stageInfo.isCleared = true;
        stageInfo.starCount = GameManager.Instance.isGemCollected ? 3 : 2;

        GameManager.Instance.SaveGameInfo();

        if (GameManager.Instance.stage >= 4)
        {
            nextBtn.gameObject.SetActive(false);
        }

        Instantiate(clearSound, null);

        ShowPanel();
    }

    // 실패 했을 때
    private void ShowFail()
    {
        clearTxt.text = "F A I L E D";
        clearTxt.color = failColor;

        cvs.blocksRaycasts = true;
        nextBtn.gameObject.SetActive(false);

        Instantiate(failSound, null);

        ShowPanel();
    }

    // 패널 연출
    private void ShowPanel()
    {
        Sequence seq = DOTween.Sequence()
            .Append(cvs.DOFade(1f, 0.5f))
            .Append(clearBG.DOFade(1f, 0.85f).SetEase(Ease.OutSine))
            .Append(clearTxt.transform.DOLocalMoveY(-200f, 0.45f).From(true))
            .AppendCallback(() => ShowStar())
            .AppendInterval(0.4f)
            .AppendCallback(() => ShowButton())
            .AppendInterval(0.5f)
            .AppendCallback(() => cvs.interactable = true);
    }

    // 버튼 연출 
    private void ShowButton()
    {
        List<CanvasGroup> btns = new List<CanvasGroup>();

        btns.Add(replayBtn.GetComponent<CanvasGroup>());
        btns.Add(homeBtn.GetComponent<CanvasGroup>());
        btns.Add(nextBtn.GetComponent<CanvasGroup>());

        for (int i = 0; i < btns.Count; i++)
        {
            btns[i].alpha = 0f;
            btns[i].DOFade(1f, 1.3f)
                .SetEase(Ease.OutSine);
        }
    }

    // 별 갯수 설정
    private void SetStar(int starCount)
    {
        for (int i = 0; i < stars.Count; i++)
        {
            int a = i;

            stars[a].sprite = a < starCount ? fullStarImg : emptyStarImg;
        }
    }

    // 별 연출
    private void ShowStar()
    {
        Vector3 bigImg = new Vector3(1.3f, 1.3f, 1f);

        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].DOFade(1f, 0.3f)
                .SetDelay(i * 0.3f)
                .SetEase(Ease.Linear);

            if (stars[i].sprite == fullStarImg)
            {
                Sequence seq = DOTween.Sequence()
                    .AppendInterval(i * 0.3f)
                    .Append(stars[i].transform.DOScale(bigImg, 0.17f))
                    .Append(stars[i].transform.DOScale(Vector3.one, 0.17f));
            }
        }
    }
}
