using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool clickDown { get; private set; }
    public bool clickUp { get; private set; }

    private void Update()
    {
        clickDown = Input.GetMouseButtonDown(0);
        clickUp = Input.GetMouseButtonUp(0);
    }

    public Vector3 GetMouseWorldPos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
