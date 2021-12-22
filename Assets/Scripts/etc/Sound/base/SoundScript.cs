using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    // ���尡 �����ǰ��� ��Ȱ��ȭ�� ���ִ� ��ũ��Ʈ

    [SerializeField]
    protected float waitTime = 1f;

    protected WaitForSeconds activeWait;

    protected void Start()
    {
        activeWait = new WaitForSeconds(waitTime);
    }

    protected void OnEnable()
    {
        StartCoroutine(SetDisable());
    }

    protected IEnumerator SetDisable()
    {
        yield return null;
        yield return activeWait;

        gameObject.SetActive(false);
    }
}
