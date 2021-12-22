using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    // 사운드가 생성되고나서 비활성화를 해주는 스크립트

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
