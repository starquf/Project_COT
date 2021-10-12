using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPos : StageObj
{
    public override void Interact()
    {
        GameManager.Instance.onClear?.Invoke();
    }
}
