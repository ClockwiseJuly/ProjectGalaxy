using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIPanelAnim : MonoBehaviour
{

    
    [Header("Animation Settings")]
    [SerializeField] private float openDuration = 0.3f;
    [SerializeField] private float closeDuration = 0.2f;
    [SerializeField] private Ease openEase = Ease.OutBack;
    [SerializeField] private Ease closeEase = Ease.InBack;
    
    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Tween currentTween;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        
        // 初始状态设置为最小
        rectTransform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        Open();
    }

    private void OnDisable()
    {
        Close();
    }
    
    public void Open()
    {
        // 先激活对象再播放动画
        gameObject.SetActive(true);
        
        // 停止当前正在运行的动画
        currentTween?.Kill();
        
        // 从0缩放到原始大小
        currentTween = rectTransform.DOScale(originalScale, openDuration)
            .SetEase(openEase)
            .SetUpdate(true);
    }
    
    public void Close()
    {
        // 停止当前正在运行的动画
        currentTween?.Kill();
        
        // 从当前大小缩放到0
        currentTween = rectTransform.DOScale(Vector3.zero, closeDuration)
            .SetEase(closeEase)
            .OnComplete(() => gameObject.SetActive(false))
            .SetUpdate(true);
    }
    
    // 通过active状态控制开关
    public void SetActive(bool active)
    {
        if (active) Open();
        else Close();
    }
}
