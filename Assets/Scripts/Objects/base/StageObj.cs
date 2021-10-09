using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObj : MonoBehaviour, IMoveable, IInteractable
{
    public StageObjType objType;

    public virtual bool CheckMoveable(Vector3 dir)
    {
        return true;
    }

    public virtual void Interact()
    {
        // 자식에 구현
    }
}
