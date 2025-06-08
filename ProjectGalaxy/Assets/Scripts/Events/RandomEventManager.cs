using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RandomEventManager : Singleton<RandomEventManager>
{
    [Header("事件数据库")]
    public RandomEventDatabase eventDatabase;
    
    [Header("倒计时设置")]
    public float totalCountdownTime = 42f;      // 总倒计时时间
    public float normalEventTriggerTime = 28f;  // 普通事件触发时间点
    public float specialEventTriggerTime = 14f; // 特殊事件触发时间点
    
    [Header("UI引用")]
    public RandomEvent randomEventUI;           // RandomEvent脚本引用
    
    [Header("当前状态")]
    public float currentCountdownTime;          // 当前倒计时时间
    public bool isCountdownActive = false;      // 倒计时是否激活
    public bool normalEventTriggered = false;   // 普通事件是否已触发
    public bool specialEventTriggered = false;  // 特殊事件是否已触发
    
    [Header("当前事件")]
    public RandomEventData currentEvent;        // 当前显示的事件
    
    protected override void Awake()
    {
        base.Awake();
        
        // 如果没有设置RandomEvent引用，尝试自动查找
        if (randomEventUI == null)
        {
            randomEventUI = FindObjectOfType<RandomEvent>();
        }
    }
    
    protected override void Start()
    {
        base.Start();
        
        // 验证必要的引用
        if (eventDatabase == null)
        {
            Debug.LogError("RandomEventManager: 未设置事件数据库！");
        }
        
        if (randomEventUI == null)
        {
            Debug.LogError("RandomEventManager: 未找到RandomEvent组件！");
        }
    }
    
    private void Update()
    {
        // 更新倒计时
        if (isCountdownActive)
        {
            UpdateCountdown();
        }
    }
    
    /// <summary>
    /// 开始42秒倒计时
    /// </summary>
    public void StartCountdown()
    {
        currentCountdownTime = totalCountdownTime;
        isCountdownActive = true;
        normalEventTriggered = false;
        specialEventTriggered = false;
        
        Debug.Log($"开始42秒倒计时！当前时间: {currentCountdownTime}秒");
    }
    
    /// <summary>
    /// 停止倒计时
    /// </summary>
    public void StopCountdown()
    {
        isCountdownActive = false;
        Debug.Log("倒计时已停止");
    }
    
    /// <summary>
    /// 更新倒计时逻辑
    /// </summary>
    private void UpdateCountdown()
    {
        currentCountdownTime -= Time.deltaTime;
        
        // 检查普通事件触发条件（42秒到28秒之间）
        if (!normalEventTriggered && currentCountdownTime <= normalEventTriggerTime)
        {
            TriggerNormalEvent();
            normalEventTriggered = true;
        }
        
        // 检查特殊事件触发条件（28秒到14秒之间）
        if (!specialEventTriggered && currentCountdownTime <= specialEventTriggerTime)
        {
            TriggerSpecialEvent();
            specialEventTriggered = true;
        }
        
        // 倒计时结束
        if (currentCountdownTime <= 0)
        {
            currentCountdownTime = 0;
            isCountdownActive = false;
            Debug.Log("倒计时结束！");
        }
    }
    
    /// <summary>
    /// 触发普通事件
    /// </summary>
    private void TriggerNormalEvent()
    {
        if (eventDatabase == null)
        {
            Debug.LogError("无法触发普通事件：事件数据库为空！");
            return;
        }
        
        RandomEventData normalEvent = eventDatabase.GetRandomNormalEvent();
        
        if (normalEvent != null)
        {
            Debug.Log($"触发普通事件: {normalEvent.eventTitle}");
            ShowEvent(normalEvent);
        }
        else
        {
            Debug.LogWarning("没有可用的普通事件！");
        }
    }
    
    /// <summary>
    /// 触发特殊事件
    /// </summary>
    private void TriggerSpecialEvent()
    {
        if (eventDatabase == null)
        {
            Debug.LogError("无法触发特殊事件：事件数据库为空！");
            return;
        }
        
        RandomEventData specialEvent = eventDatabase.GetRandomSpecialEvent();
        
        if (specialEvent != null)
        {
            Debug.Log($"触发特殊事件: {specialEvent.eventTitle}");
            ShowEvent(specialEvent);
        }
        else
        {
            Debug.LogWarning("没有可用的特殊事件！");
        }
    }
    
    /// <summary>
    /// 显示事件UI
    /// </summary>
    private void ShowEvent(RandomEventData eventData)
    {
        if (randomEventUI == null)
        {
            Debug.LogError("无法显示事件：RandomEvent UI引用为空！");
            return;
        }
        
        currentEvent = eventData;
        
        // 根据事件类型显示不同的UI
        if (eventData.eventType == EventType.Normal)
        {
            randomEventUI.ShowNormalEvent(eventData);
        }
        else if (eventData.eventType == EventType.Special)
        {
            randomEventUI.ShowSpecialEvent(eventData);
        }
    }
    
    /// <summary>
    /// 处理事件选项选择
    /// </summary>
    public void OnOptionSelected(int optionIndex)
    {
        if (currentEvent == null)
        {
            Debug.LogError("没有当前事件可处理！");
            return;
        }
        
        if (optionIndex < 0 || optionIndex >= currentEvent.options.Length)
        {
            Debug.LogError($"选项索引超出范围: {optionIndex}");
            return;
        }
        
        EventOption selectedOption = currentEvent.options[optionIndex];
        
        Debug.Log($"选择了选项: {selectedOption.optionText}");
        
        // 应用事件效果
        if (selectedOption.eventEffect != null)
        {
            selectedOption.eventEffect.ApplyEffect();
        }
        
        // 显示结果文本（如果有）
        if (!string.IsNullOrEmpty(selectedOption.resultText))
        {
            Debug.Log($"结果: {selectedOption.resultText}");
            // 这里可以添加显示结果文本的UI逻辑
        }
        
        // 隐藏事件UI
        HideEventUI();
        
        // 清除当前事件
        currentEvent = null;
    }
    
    /// <summary>
    /// 隐藏事件UI
    /// </summary>
    public void HideEventUI()
    {
        if (randomEventUI != null)
        {
            randomEventUI.ReturnToOriginal();
        }
    }
    
    /// <summary>
    /// 获取当前倒计时时间（供UI显示）
    /// </summary>
    public float GetCurrentCountdownTime()
    {
        return currentCountdownTime;
    }
    
    /// <summary>
    /// 获取倒计时是否激活
    /// </summary>
    public bool IsCountdownActive()
    {
        return isCountdownActive;
    }
    
    /// <summary>
    /// 重置事件数据库（用于测试）
    /// </summary>
    [ContextMenu("重置事件数据库")]
    public void ResetEventDatabase()
    {
        if (eventDatabase != null)
        {
            eventDatabase.ResetAllEvents();
            Debug.Log("事件数据库已重置");
        }
    }
    
    /// <summary>
    /// 手动触发普通事件（用于测试）
    /// </summary>
    [ContextMenu("测试触发普通事件")]
    public void TestTriggerNormalEvent()
    {
        TriggerNormalEvent();
    }
    
    /// <summary>
    /// 手动触发特殊事件（用于测试）
    /// </summary>
    [ContextMenu("测试触发特殊事件")]
    public void TestTriggerSpecialEvent()
    {
        TriggerSpecialEvent();
    }
}