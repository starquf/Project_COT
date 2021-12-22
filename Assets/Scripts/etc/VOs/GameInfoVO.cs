using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameInfoVO
{
    public int jemCount = 0;

    public float masterVolume = 0f;
    public float bgmVolume = 0f;
    public float effectVolume = 0f;

    public List<ChapterInfoVO> chapters = new List<ChapterInfoVO>();
}
