using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    // 플레이어 이동 관련

    private PlayerInput playerInput = null;

    Vector3 clickPos = Vector3.zero;

    [Header("이동 관련 파라미터")]
    [SerializeField] private float moveDur = 0.5f;
    private bool canMove = true;

    private bool isCleared = false;
    private bool isEnable = true;

    private LayerMask whatIsWall;
    private LayerMask whatIsGround;
    private LayerMask whatIsObj;

    private Vector3 bigScale = Vector3.zero;
    private Vector3 normalScale = Vector3.zero;

    [Header("소리 관련")]
    public GameObject moveSound;
    public GameObject disableSound;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        #region Init
        whatIsWall = LayerMask.GetMask("WALL");
        whatIsGround = LayerMask.GetMask("GROUND");
        whatIsObj = LayerMask.GetMask("STAGEOBJ");

        normalScale = transform.localScale;
        bigScale = new Vector3(normalScale.x + 0.1f, normalScale.y + 0.1f, 1f);
        #endregion

        GameManager.Instance.onClear.AddListener(() => 
        { 
            isEnable = false;
            isCleared = true;
        });

        GameManager.Instance.onUnPause.AddListener(() => 
        {
            clickPos = playerInput.GetMouseWorldPos();

            isEnable = true;
        });

        GameManager.Instance.onPause.AddListener(() => 
        {
            //clickPos = playerInput.GetMouseWorldPos();

            isEnable = false; 
        });

        GameManager.Instance.onFailed.AddListener(() => isEnable = false);

        PoolManager.CreatePool<Sound_Disable>(disableSound, transform, 3);
        PoolManager.CreatePool<Sound_Move>(moveSound, transform, 3);
    }

    private void Update()
    {
        if (!isEnable) return;

        GetDrag();
    }

    // 드레그 체크
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

    // 현재 땅이 이동 가능한지 체크
    private bool CheckCurrentGround()
    {
        Collider2D coll = Physics2D.OverlapPoint(transform.position, whatIsGround);

        // 이동 가능한지 체크
        return CheckMoveable(Vector3.zero, coll);
    }

    // 이동하려는 방향의 땅 체크
    private bool CheckGround(Vector3 dir, out IInteractable it, out TileType tileType)
    {
        it = null;
        tileType = TileType.NONE;

        // 벽이 있는지 체크
        if (Physics2D.Raycast(transform.position, dir, 1f, whatIsWall))
        {
            return false;
        }

        Collider2D coll = Physics2D.OverlapPoint(transform.position + dir, whatIsGround);

        if (coll != null)
        {
            tileType = coll.GetComponent<Tile>().tileType;
        }

        // 이동 가능한지 체크
        return CheckMoveable(dir, coll, out it);
    }

    // 현재 아이템이 이동 가능한지 체크
    private bool CheckCurrentObjMoveable()
    {
        Collider2D coll = Physics2D.OverlapPoint(transform.position, whatIsObj);

        // 아이템이 없으면
        if (coll == null)
        {
            return true;
        }

        // 아이템이 있는데 이동 가능한지 체크
        return CheckMoveable(Vector3.zero, coll);
    }

    // 이동하려는 곳의 아이템 체크
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

    // 이동 가능한지 체크
    private bool CheckMoveable(Vector3 dir, Collider2D coll)
    {
        if (coll == null)
        {
            return false;
        }

        IMoveable moveable = coll.GetComponent<IMoveable>();

        // 땅이 있는지와 이동 가능한 땅인지 체크
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

        // 땅이 있는지와 이동 가능한 땅인지 체크
        if (moveable == null || !moveable.CheckMoveable(dir))
        {
            return false;
        }

        return true;
    }

    // 이동
    private void Move(Vector3 dir, bool isRepeat = false)
    {
        // 이동이 가능하지 않다면 리턴
        if (!isEnable) return;

        IInteractable groundIt = null;
        IInteractable objIt = null;

        TileType tileType;

        GameManager.Instance.timeDayhandler.CanChangeTime = false;

        // 현재 땅 + 현재 옵젝 확인
        if (!CheckCurrentGround() || !CheckCurrentObjMoveable())
        {
            // 이동 불가
            CantMove(isRepeat);

            return;
        }
            
        // 땅 체크
        if (CheckGround(dir, out groundIt, out tileType))
        {
            // 아이템 체크
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

            PoolManager.GetItem<Sound_Move>();

            // 이동이 반복되지 않다면
            if (!isRepeat)
            {
                GameManager.Instance.moveLimit--;

                if (!isCleared && GameManager.Instance.moveLimit <= 0)
                {
                    GameManager.Instance.onFailed?.Invoke();
                }

                GameManager.Instance.onUpdateUI.Invoke();
            }

            // 물(빙판)이라면
            switch (tileType)
            {
                case TileType.WATER:

                    // 이동 반복
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
            // 이동 불가
            CantMove(isRepeat);
        }
    }

    private void CantMove(bool isRepeat)
    {
        if (!isRepeat)
        {
            Camera.main.DOShakePosition(0.2f, 0.02f, 22, 90, false);
        }

        PoolManager.GetItem<Sound_Disable>();

        GameManager.Instance.timeDayhandler.CanChangeTime = true;
        canMove = true;
    }
}
