using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBGUIHandler : MonoBehaviour
{
    // ���� ���������� ���� ����� �ٲ��ִ� ��ũ��Ʈ

    public List<GameObject> bgs = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < bgs.Count; i++)
        {
            bgs[i].SetActive(i == GameManager.Instance.chapter);
        }
    }
}
