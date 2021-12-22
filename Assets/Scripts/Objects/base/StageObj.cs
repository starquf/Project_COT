using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageObj : MonoBehaviour, IMoveable, IInteractable
{
    // 스테이지 요소 기본 뼈대

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
