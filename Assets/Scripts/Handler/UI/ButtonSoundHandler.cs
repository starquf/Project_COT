using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundHandler : MonoBehaviour
{
    public GameObject btnSoundObj;

    private void Start()
    {
        PoolManager.CreatePool<Sound_BtnClick>(btnSoundObj, null, 10);

        Button[] btns = transform.GetComponentsInChildren<Button>();

        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].onClick.AddListener(() =>
            {
                Sound_BtnClick obj = PoolManager.GetItem<Sound_BtnClick>();
            });
        }
    }
}
