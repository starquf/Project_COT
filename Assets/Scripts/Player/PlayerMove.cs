using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput playerInput = null;

    Vector3 clickPos = Vector3.zero;

    [Header("이동 관련 파라미터")]
    [SerializeField] private float moveDur = 0.5f;
    private bool canMove = true;

    private LayerMask whatIsWall;
    private LayerMask whatIsGround;
    private LayerMask whatIsObj;

    private Vector3 bigScale = Vector3.zero;
    private Vector3 normalScale = Vector3.zero;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        whatIsWall = LayerMask.GetMask("WALL");
        whatIsGround = LayerMask.GetMask("GROUND");
        whatIsObj = LayerMask.GetMask("STAGEOBJ");

        normalScale = transform.localScale;
        bigScale = new Vector3(normalScale.x + 0.1f, normalScale.y + 0.1f, 1f);
    }

    private void Update()
    {
        GetDrag();
    }

    private void GetDrag()
    {
        if (playerInput.clickDown)
        {
            clickPos = playerInput.GetMouseWorldPos();
        }
        else if (playerInput.clickUp && canMove)
        {
            Vector2 dir = playerInput.GetMouseWorldPos() - clickPos;

            if (dir.sqrMagnitude < (0.5f * 0.5f)) return;

            if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))   // x방향으로 이동
            {
                if (dir.x >= 0)                         // x가 0보다 크다면
                {
                    Move(Vector3.right);
                }
                else
                {
                    Move(Vector3.left);
                }
            }
            else                                        // y방향으로 이동
            {
                if (dir.y >= 0)                         // y가 0보다 크다면
                {
                    Move(Vector3.up);
                }
                else
                {
                    Move(Vector3.down);
                }
            }
        }
    }

    private bool CheckGround(Vector3 dir, out IInteractable it)
    {
        it = null;

        // 벽이 있는지 체크
        if (Physics2D.Raycast(transform.position, dir, 1f, whatIsWall))
        {
            return false;
        }

        Collider2D coll = Physics2D.OverlapPoint(transform.position + dir, whatIsGround);

        // 이동 가능한지 체크
        return CheckMoveable(dir, coll, out it);
    }

    private bool CheckObjMoveable(Vector3 dir, out IInteractable it)
    {
        it = null;

        Collider2D coll = Physics2D.OverlapPoint(transform.position + dir, whatIsObj);

        // 아이템이 없으면
        if (coll == null)
        {
            return true;
        }

        // 아이템이 있는데 이동 가능한지 체크
        return CheckMoveable(dir, coll, out it);
    }

    private bool CheckMoveable(Vector3 dir, Collider2D coll, out IInteractable it)
    {
        it = null;

        if (coll == null)
        {
            return false;
        }

        IMoveable moveable = coll.GetComponent<IMoveable>();
        it = coll.GetComponent<IInteractable>();

        // 땅이 있는지와 이동 가능한 땅인지 체크
        if (moveable == null || !moveable.CheckMoveable(dir))
        {
            return false;
        }

        return true;
    }

    private void Move(Vector3 dir)
    {
        IInteractable groundIt = null;
        IInteractable objIt = null;

        // 땅 체크
        if (CheckGround(dir, out groundIt))
        {
            // 아이템 체크
            if (!CheckObjMoveable(dir, out objIt))
            {
                return;
            }

            canMove = false;
            transform.DOMove(transform.position + dir, moveDur).OnComplete(() => { canMove = true; });

            Sequence bigSeq = DOTween.Sequence()
            .Append(transform.DOScale(bigScale, 0.15f))
            .Append(transform.DOScale(normalScale, 0.15f));
            
            GameManager.Instance.moveLimit--;
            GameManager.Instance.onUpdateUI.Invoke();

            groundIt?.Interact();
            objIt?.Interact();
        }
        else
        {
            // 이동 불가
            Camera.main.DOShakePosition(0.2f, 0.02f, 22, 90, false);
        }
    }
}
