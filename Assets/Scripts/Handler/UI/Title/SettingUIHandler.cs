using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingUIHandler : UIHandler
{
    // 설정 UI를 관리하는 핸들러

    public UIHandler chapterSelectUIHandler;

    public Button closeBtn;

    public Button creditBtn;
    public Button settingBtn;

    public GameObject settingBG;
    public GameObject creditBG;

    [Header("소리 버튼")]
    public Button bgmBtn;
    public Text bgmText;
    private bool isBgm = true;

    public Button sfxBtn;
    public Text sfxText;
    private bool isSfx = true;

    private readonly string bgm = "BGM";
    private readonly string effect = "EFFECT";

    public AudioMixer am;

    private void Start()
    {
        cvs = GetComponent<CanvasGroup>();

        #region Init UI
        cvs.alpha = 0f;
        cvs.blocksRaycasts = false;
        cvs.interactable = false;
        #endregion

        // UI 열릴 때
        onOpenUI += OpenSetting;

        closeBtn.onClick.AddListener(() => Close());

        creditBtn.onClick.AddListener(() => 
        {
            settingBG.SetActive(false);
            creditBG.SetActive(true);
        });

        settingBtn.onClick.AddListener(() =>
        {
            settingBG.SetActive(true);
            creditBG.SetActive(false);
        });

        #region hard coding of audio mixer
        am.SetFloat(bgm, GameManager.Instance.gameInfo.bgmVolume);
        am.SetFloat(effect, GameManager.Instance.gameInfo.effectVolume);

        am.GetFloat(bgm, out float bgmVal);
        if (bgmVal.Equals(-80f))
        {
            bgmText.text = "Off";
            isBgm = false;
        }
        else
        {
            bgmText.text = "On";
            isBgm = true;
        }

        am.GetFloat(effect, out float effectVal);
        if (effectVal.Equals(-80f))
        {
            sfxText.text = "Off";
            isSfx = false;
        }
        else
        {
            sfxText.text = "On";
            isSfx = true;
        }

        bgmBtn.onClick.AddListener(() =>
        {
            if (isBgm)
            {
                bgmText.text = "Off";
                am.SetFloat(bgm, -80f);

                GameManager.Instance.gameInfo.bgmVolume = -80f;
            }
            else
            {
                bgmText.text = "On";
                am.SetFloat(bgm, 0f);

                GameManager.Instance.gameInfo.bgmVolume = 0f;
            }

            isBgm = !isBgm;

            GameManager.Instance.SaveGameInfo();
        });

        sfxBtn.onClick.AddListener(() =>
        {
            if (isSfx)
            {
                sfxText.text = "Off";
                am.SetFloat(effect, -80f);

                GameManager.Instance.gameInfo.effectVolume = -80f;
            }
            else
            {
                sfxText.text = "On";
                am.SetFloat(effect, 0f);

                GameManager.Instance.gameInfo.effectVolume = 0f;
            }

            isSfx = !isSfx;

            GameManager.Instance.SaveGameInfo();
        });
        #endregion

        settingBG.SetActive(true);
        creditBG.SetActive(false);
    }

    private void OpenSetting()
    {
        chapterSelectUIHandler.isOpened = false;

        cvs.alpha = 1f;
        cvs.blocksRaycasts = true;
        cvs.interactable = true;

        settingBG.SetActive(true);
        creditBG.SetActive(false);
    }

    public override void Close()
    {
        base.Close();

        cvs.alpha = 0f;
        cvs.blocksRaycasts = false;
        cvs.interactable = false;

        chapterSelectUIHandler.isOpened = true;
    }
}
