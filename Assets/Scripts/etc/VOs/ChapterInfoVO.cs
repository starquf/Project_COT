using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChapterInfoVO
{
    public List<StageInfoVO> stages = new List<StageInfoVO>();

    public ChapterInfoVO()
    {
        stages = new List<StageInfoVO>();

        for (int i = 0; i < 5; i++)
        {
            stages.Add(new StageInfoVO());
        }
    }
}
