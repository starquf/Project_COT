using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameInfoVO
{
    public int jemCount = 0;

    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float effectVolume = 1f;

    public List<ChapterInfoVO> chapters = new List<ChapterInfoVO>();
}
