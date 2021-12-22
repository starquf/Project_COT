using UnityEngine;

// 이동 가능한 것에 대한 인터페이스
public interface IMoveable
{
    public bool CheckMoveable(Vector3 dir);
}
