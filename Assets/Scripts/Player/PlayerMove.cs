using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput playerInput = null;

    Vector3 clickPos = Vector3.zero;

    [Header("�̵� ���� �Ķ����")]
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

            if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))   // x�������� �̵�
            {
                if (dir.x >= 0)                         // x�� 0���� ũ�ٸ�
                {
                    Move(Vector3.right);
                }
                else
                {
                    Move(Vector3.left);
                }
            }
            else                                        // y�������� �̵�
            {
                if (dir.y >= 0)                         // y�� 0���� ũ�ٸ�
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

        // ���� �ִ��� üũ
        if (Physics2D.Raycast(transform.position, dir, 1f, whatIsWall))
        {
            return false;
        }

        Collider2D coll = Physics2D.OverlapPoint(transform.position + dir, whatIsGround);

        // �̵� �������� üũ
        return CheckMoveable(dir, coll, out it);
    }

    private bool CheckObjMoveable(Vector3 dir, out IInteractable it)
    {
        it = null;

        Collider2D coll = Physics2D.OverlapPoint(transform.position + dir, whatIsObj);

        // �������� ������
        if (coll == null)
        {
            return true;
        }

        // �������� �ִµ� �̵� �������� üũ
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

        // ���� �ִ����� �̵� ������ ������ üũ
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

        // �� üũ
        if (CheckGround(dir, out groundIt))
        {
            // ������ üũ
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
            // �̵� �Ұ�
            Camera.main.DOShakePosition(0.2f, 0.02f, 22, 90, false);
        }
    }
}
