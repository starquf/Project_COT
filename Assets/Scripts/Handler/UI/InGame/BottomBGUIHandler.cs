using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBGUIHandler : MonoBehaviour
{
    // 현재 스테이지에 따라 배경을 바꿔주는 스크립트

    public List<GameObject> bgs = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < bgs.Count; i++)
        {
            bgs[i].SetActive(i == GameManager.Instance.chapter);
        }
    }
}
