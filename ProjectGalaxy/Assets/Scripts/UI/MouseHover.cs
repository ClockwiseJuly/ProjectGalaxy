using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public enum UIType
    {
        none,
        functional,
        item,
    }

    public UIType uiType ;
    
    [SerializeField] private float hoverScale = 1.2f; // 悬停时的放大倍数
    [SerializeField] private float scaleDuration = 0.3f; // 动画持续时间
    [SerializeField] private Ease easeType = Ease.OutBack; // 动画缓动类型

    private Vector3 _originalScale; // 记录原始大小
    private Tween _scaleTween; // 存储Tween动画以便控制
    private IPointerEnterHandler _pointerEnterHandlerImplementation;
    private IPointerExitHandler _pointerExitHandlerImplementation;

    private void Start()
    {
        _originalScale = transform.localScale; //记录原始大小
    }
    
    private void OnDisable()
    {
        transform.localScale = _originalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(4);
        
        _scaleTween?.Kill(); // 打断之前的动画

        if (uiType == UIType.item)
        {
            if (gameObject.GetComponentInParent<WareHouseItem>() != null)
            {
                if (gameObject.GetComponentInParent<WareHouseItem>().hasItem)
                {
                    _scaleTween = transform.DOScale(_originalScale * hoverScale, scaleDuration)
                        .SetEase(easeType)
                        .SetUpdate(true); // 无视Time.timeScale
                }
                else
                {
                    return;
                }
            }
            
        }
        else
        {
            _scaleTween = transform.DOScale(_originalScale * hoverScale, scaleDuration)
                .SetEase(easeType)
                .SetUpdate(true); // 无视Time.timeScale
        }
        
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(_originalScale, scaleDuration)
            .SetEase(easeType).SetUpdate(true);
    }
}
