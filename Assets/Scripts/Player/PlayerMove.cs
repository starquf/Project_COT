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

    private bool isCleared = false;
    private bool isEnable = true;

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

        GameManager.Instance.onClear.AddListener(() => 
        { 
            isEnable = false;
            isCleared = true;
        });

        GameManager.Instance.onFailed.AddListener(() => isEnable = false);
    }

    private void Update()
    {
        if (!isEnable) return;

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

    private bool CheckCurrentGround()
    {
        Collider2D coll = Physics2D.OverlapPoint(transform.position, whatIsGround);

        // �̵� �������� üũ
        return CheckMoveable(Vector3.zero, coll);
    }

    private bool CheckGround(Vector3 dir, out IInteractable it, out TileType tileType)
    {
        it = null;
        tileType = TileType.NONE;

        // ���� �ִ��� üũ
        if (Physics2D.Raycast(transform.position, dir, 1f, whatIsWall))
        {
            return false;
        }

        Collider2D coll = Physics2D.OverlapPoint(transform.position + dir, whatIsGround);

        if (coll != null)
        {
            tileType = coll.GetComponent<Tile>().tileType;
        }

        // �̵� �������� üũ
        return CheckMoveable(dir, coll, out it);
    }

    private bool CheckCurrentObjMoveable()
    {
        Collider2D coll = Physics2D.OverlapPoint(transform.position, whatIsObj);

        // �������� ������
        if (coll == null)
        {
            return true;
        }

        // �������� �ִµ� �̵� �������� üũ
        return CheckMoveable(Vector3.zero, coll);
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

    private bool CheckMoveable(Vector3 dir, Collider2D coll)
    {
        if (coll == null)
        {
            return false;
        }

        IMoveable moveable = coll.GetComponent<IMoveable>();

        // ���� �ִ����� �̵� ������ ������ üũ
        if (moveable == null || !moveable.CheckMoveable(dir))
        {
            return false;
        }

        return true;
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

    private void Move(Vector3 dir, bool isRepeat = false)
    {
        IInteractable groundIt = null;
        IInteractable objIt = null;

        TileType tileType;

        GameManager.Instance.timeDayhandler.CanChangeTime = false;

        // ���� �� + ���� ���� Ȯ��
        if (!CheckCurrentGround() || !CheckCurrentObjMoveable())
        {
            // �̵� �Ұ�
            CantMove(isRepeat);

            return;
        }
            
        // �� üũ
        if (CheckGround(dir, out groundIt, out tileType))
        {
            // ������ üũ
            if (!CheckObjMoveable(dir, out objIt))
            {
                CantMove(isRepeat);

                return;
            }

            canMove = false;

            Sequence bigSeq = DOTween.Sequence()
            .Append(transform.DOScale(bigScale, 0.15f))
            .Append(transform.DOScale(normalScale, 0.15f));

            groundIt?.Interact();
            objIt?.Interact();

            if (!isRepeat)
            {
                GameManager.Instance.moveLimit--;

                if (!isCleared && GameManager.Instance.moveLimit <= 0)
                {
                    // ���� �̰� ������
                    GameManager.Instance.onFailed?.Invoke();
                }

                GameManager.Instance.onUpdateUI.Invoke();
            }

            switch (tileType)
            {
                case TileType.WATER:

                    transform.DOMove(transform.position + dir, moveDur).OnComplete(() => { Move(dir, true); });
                    return;
            }

            transform.DOMove(transform.position + dir, moveDur).OnComplete(() => 
            { 
                canMove = true;
                GameManager.Instance.timeDayhandler.CanChangeTime = true; 
            });
        }
        else
        {
            // �̵� �Ұ�
            CantMove(isRepeat);
        }
    }

    private void CantMove(bool isRepeat)
    {
        if (!isRepeat)
        {
            Camera.main.DOShakePosition(0.2f, 0.02f, 22, 90, false);
        }

        GameManager.Instance.timeDayhandler.CanChangeTime = true;
        canMove = true;
    }
}
