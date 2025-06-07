using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    public enum MoveDirection
    {
        UpDown,
        LeftRight
    }

    [Header("移动设置")]
    [SerializeField] private MoveDirection direction = MoveDirection.UpDown;
    [SerializeField] private float moveDistance = 50f;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private Ease easeType = Ease.InOutSine;
    [SerializeField] private bool playOnEnable = true;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Sequence moveSequence;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        DOTween.Init();
    }

    void OnEnable()
    {
        if (playOnEnable)
        {
            StartAnimation();
        }
    }

    void OnDisable()
    {
        StopAnimation();
    }

    public void StartAnimation()
    {
        StopAnimation();

        rectTransform.anchoredPosition = originalPosition;
        moveSequence = DOTween.Sequence();

        // 根据方向创建动画
        if (direction == MoveDirection.UpDown)
        {
            Tweener moveUp = rectTransform.DOAnchorPosY(originalPosition.y + moveDistance, moveDuration).SetEase(easeType);
            Tweener moveDown = rectTransform.DOAnchorPosY(originalPosition.y, moveDuration).SetEase(easeType);
            
            moveSequence.Append(moveUp)
                       .Append(moveDown);
        }
        else // LeftRight
        {
            Tweener moveRight = rectTransform.DOAnchorPosX(originalPosition.x + moveDistance, moveDuration).SetEase(easeType);
            Tweener moveLeft = rectTransform.DOAnchorPosX(originalPosition.x, moveDuration).SetEase(easeType);
            
            moveSequence.Append(moveRight)
                       .Append(moveLeft);
        }

        moveSequence.SetLoops(-1, LoopType.Restart)
                    .SetUpdate(true);
    }

    public void StopAnimation()
    {
        if (moveSequence != null)
        {
            moveSequence.Kill();
            moveSequence = null;
        }
        rectTransform.anchoredPosition = originalPosition;
    }

    public void SetDirection(MoveDirection newDirection, bool restartAnimation = true)
    {
        if (direction != newDirection)
        {
            direction = newDirection;
            if (restartAnimation && gameObject.activeInHierarchy)
            {
                StartAnimation();
            }
        }
    }

    void OnValidate()
    {
        if (moveDuration <= 0)
        {
            moveDuration = 0.1f;
            Debug.LogWarning("移动持续时间必须大于0");
        }
        if (moveDistance < 0)
        {
            moveDistance = Mathf.Abs(moveDistance);
        }
    }
}
