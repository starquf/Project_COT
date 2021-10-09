using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : StageObj
{
    public GameObject playerObj = null;

    private void Start()
    {
        Instantiate(playerObj, transform.position, Quaternion.identity);
    }
}
