using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomEvent : MonoBehaviour
{
    [System.Serializable]
    public class ImageData
    {
        public RectTransform image;          // 要移动的图片
        public Transform originalPosition;   // 原始位置物体
        public Transform targetPosition;     // 目标位置物体
    }

    [Header("Image Settings")]
    public ImageData leftImage;
    public ImageData centerImage;
    public ImageData rightImage;

    [Header("Animation Settings")]
    public float moveDuration = 1f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Event UI Components")]
    [SerializeField] private TextMeshProUGUI eventTitleText;        // 事件标题
    [SerializeField] private TextMeshProUGUI eventDescriptionText;  // 事件描述
    [SerializeField] private Image eventIconImage;                  // 事件图标
    
    [Header("Option Buttons")]
    [SerializeField] private Button leftOptionButton;               // 左侧选项按钮
    [SerializeField] private Button rightOptionButton;              // 右侧选项按钮
    [SerializeField] private TextMeshProUGUI leftOptionText;        // 左侧选项文本
    [SerializeField] private TextMeshProUGUI rightOptionText;       // 右侧选项文本
    
    [Header("Single Option Mode")]
    [SerializeField] private Button singleOptionButton;             // 单选项按钮
    [SerializeField] private TextMeshProUGUI singleOptionText;      // 单选项文本

    private RandomEventData currentEventData;
    private System.Action onEventCompleted;

    void Start()
    {
        // 初始化按钮事件
        if (leftOptionButton != null)
            leftOptionButton.onClick.AddListener(() => OnOptionSelected(0));
        if (rightOptionButton != null)
            rightOptionButton.onClick.AddListener(() => OnOptionSelected(1));
        if (singleOptionButton != null)
            singleOptionButton.onClick.AddListener(() => OnOptionSelected(0));
    }

    private void Update()
    {
        // 保留原有的Update逻辑
    }

    // 显示普通事件（单选项）
    public void ShowNormalEvent(RandomEventData eventData)
    {
        currentEventData = eventData;
        
        // 设置事件内容
        SetEventContent(eventData);
        
        // 设置为单选项模式
        SetUIMode(false);
        
        // 播放弹出动画
        MoveToTarget();
        
        Debug.Log($"显示普通事件: {eventData.eventTitle}");
    }

    // 显示特殊事件（双选项）
    public void ShowSpecialEvent(RandomEventData eventData)
    {
        currentEventData = eventData;
        
        // 设置事件内容
        SetEventContent(eventData);
        
        // 设置为双选项模式
        SetUIMode(true);
        
        // 播放弹出动画
        MoveToTarget();
        
        Debug.Log($"显示特殊事件: {eventData.eventTitle}");
    }

    // 通用显示事件方法（保留兼容性）
    public void ShowEvent(RandomEventData eventData, System.Action onCompleted = null)
    {
        currentEventData = eventData;
        onEventCompleted = onCompleted;
        
        // 设置事件内容
        SetEventContent(eventData);
        
        // 根据事件类型设置UI模式
        bool isSpecialEvent = (eventData.eventType == EventType.Special);
        SetUIMode(isSpecialEvent);
        
        // 播放弹出动画
        MoveToTarget();
    }

    // 设置事件内容
    private void SetEventContent(RandomEventData eventData)
    {
        if (eventTitleText != null)
            eventTitleText.text = eventData.eventTitle;
            
        if (eventDescriptionText != null)
            eventDescriptionText.text = eventData.eventDescription;
            
        if (eventIconImage != null && eventData.eventImage != null)
            eventIconImage.sprite = eventData.eventImage;

        // 设置选项文本
        bool isSpecialEvent = (eventData.eventType == EventType.Special);
        if (isSpecialEvent && eventData.options.Length >= 2)
        {
            if (leftOptionText != null)
                leftOptionText.text = eventData.options[0].optionText;
            if (rightOptionText != null)
                rightOptionText.text = eventData.options[1].optionText;
        }
        else if (eventData.options.Length >= 1)
        {
            if (singleOptionText != null)
                singleOptionText.text = eventData.options[0].optionText;
        }
    }

    // 设置UI模式（单选项或双选项）
    private void SetUIMode(bool isSpecialEvent)
    {
        if (isSpecialEvent)
        {
            // 特殊事件：显示双选项
            if (leftOptionButton != null) leftOptionButton.gameObject.SetActive(true);
            if (rightOptionButton != null) rightOptionButton.gameObject.SetActive(true);
            if (singleOptionButton != null) singleOptionButton.gameObject.SetActive(false);
        }
        else
        {
            // 普通事件：显示单选项
            if (leftOptionButton != null) leftOptionButton.gameObject.SetActive(false);
            if (rightOptionButton != null) rightOptionButton.gameObject.SetActive(false);
            if (singleOptionButton != null) singleOptionButton.gameObject.SetActive(true);
        }
    }

    // 处理选项选择
    private void OnOptionSelected(int optionIndex)
    {
        if (currentEventData != null && optionIndex < currentEventData.options.Length)
        {
            // 通知RandomEventManager处理选项选择
            if (RandomEventManager.Instance != null)
            {
                RandomEventManager.Instance.OnOptionSelected(optionIndex);
            }
            else
            {
                // 如果没有RandomEventManager，直接应用效果
                ApplyEventEffect(currentEventData.options[optionIndex].eventEffect);
                ReturnToOriginal();
            }
            
            // 调用完成回调
            onEventCompleted?.Invoke();
        }
    }

    // 应用事件效果（备用方法）
    private void ApplyEventEffect(EventEffect effect)
    {
        if (effect != null)
        {
            // 直接调用EventEffect的ApplyEffect方法
            effect.ApplyEffect();
        }
    }

    // 移动到目标位置（弹出面板）
    public void MoveToTarget()
    {
        StartCoroutine(AnimateMove(leftImage.image, leftImage.targetPosition.position));
        StartCoroutine(AnimateMove(centerImage.image, centerImage.targetPosition.position));
        StartCoroutine(AnimateMove(rightImage.image, rightImage.targetPosition.position));
    }

    // 返回原始位置（收走面板）
    public void ReturnToOriginal()
    {
        StartCoroutine(AnimateMove(leftImage.image, leftImage.originalPosition.position));
        StartCoroutine(AnimateMove(centerImage.image, centerImage.originalPosition.position));
        StartCoroutine(AnimateMove(rightImage.image, rightImage.originalPosition.position));
    }

    private IEnumerator AnimateMove(RectTransform image, Vector3 targetPos)
    {
        Vector3 startPos = image.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            float curveValue = moveCurve.Evaluate(t);
            
            image.position = Vector3.Lerp(startPos, targetPos, curveValue);
            yield return null;
        }

        image.position = targetPos;
    }
}
