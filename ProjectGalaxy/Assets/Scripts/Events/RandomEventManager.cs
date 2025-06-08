using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RandomEventManager : Singleton<RandomEventManager>
{
    [Header("事件数据库")]
    public RandomEventDatabase eventDatabase;
    
    [Header("选项池")]
    public EventOptionPool optionPool;
    
    [Header("事件触发时间点")]
    public float normalEventTriggerTime = 28f;  // 普通事件触发时间点
    public float specialEventTriggerTime = 14f; // 特殊事件触发时间点
    
    [Header("UI引用")]
    public RandomEvent randomEventUI;           // RandomEvent脚本引用
    
    [Header("当前状态")]
    public bool normalEventTriggered = false;   // 普通事件是否已触发
    public bool specialEventTriggered = false;  // 特殊事件是否已触发
    
    [Header("当前事件")]
    public RandomEventData currentEvent;        // 当前显示的事件
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject); // 持久化本对象
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
        
        if (optionPool == null)
        {
            Debug.LogError("RandomEventManager: 未设置选项池！");
        }
        
        if (randomEventUI == null)
        {
            Debug.LogError("RandomEventManager: 未找到RandomEvent组件！");
        }
        
        // 重置事件触发状态
        ResetEventTriggers();
        
        // 订阅跃迁完成事件，在每轮开始时重置事件触发状态
        GameEvent.OnTraverseCompleted += OnNewRoundStarted;
    }
    
    private void OnDestroy()
    {
        // 取消订阅事件
        GameEvent.OnTraverseCompleted -= OnNewRoundStarted;
    }
    
    /// <summary>
    /// 新轮次开始时调用，重置事件触发状态
    /// </summary>
    private void OnNewRoundStarted()
    {
        ResetEventTriggers();
        Debug.Log("RandomEventManager: 新轮次开始，事件触发状态已重置");
    }
    
    private void Update()
    {
        // 检查UIManager的倒计时状态
        if (UIManager.Instance != null && !UIManager.Instance.isPaused)
        {
            CheckEventTriggers();
        }
    }
    
    /// <summary>
    /// 检查事件触发条件
    /// </summary>
    private void CheckEventTriggers()
    {
        if (UIManager.Instance == null) return;
        
        // 获取UIManager的当前倒计时时间
        float currentTime = GetCurrentCountdownTime();
        
        // 检查普通事件触发条件
        if (!normalEventTriggered && currentTime <= normalEventTriggerTime && currentTime > specialEventTriggerTime)
        {
            TriggerNormalEvent();
            normalEventTriggered = true;
        }
        
        // 检查特殊事件触发条件
        if (!specialEventTriggered && currentTime <= specialEventTriggerTime && currentTime > 0)
        {
            TriggerSpecialEvent();
            specialEventTriggered = true;
        }
    }
    
    /// <summary>
    /// 获取UIManager的当前倒计时时间
    /// </summary>
    public float GetCurrentCountdownTime()
    {
        if (UIManager.Instance != null)
        {
            return UIManager.Instance.GetCurrentTime();
        }
        return 0f;
    }
    
    /// <summary>
    /// 重置事件触发状态（当一轮开始时调用）
    /// </summary>
    public void ResetEventTriggers()
    {
        normalEventTriggered = false;
        specialEventTriggered = false;
        Debug.Log("RandomEventManager: 事件触发状态已重置");
    }
    
    /// <summary>
    /// 触发普通事件
    /// </summary>
    private void TriggerNormalEvent()
    {
        if (eventDatabase == null || optionPool == null)
        {
            Debug.LogError("无法触发普通事件：事件数据库或选项池为空！");
            return;
        }
        
        RandomEventData normalEvent = eventDatabase.GetRandomNormalEvent();
        
        if (normalEvent != null)
        {
            // 从选项池随机获取普通事件选项
            EventOption randomOption = optionPool.GetRandomNormalOption();
            
            if (randomOption != null)
            {
                // 设置运行时选项
                normalEvent.SetRuntimeOptions(new EventOption[] { randomOption });
                
                Debug.Log($"触发普通事件: {normalEvent.eventTitle}, 选项: {randomOption.optionText}");
                ShowEvent(normalEvent);
            }
            else
            {
                Debug.LogWarning("没有可用的普通事件选项！");
            }
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
        if (eventDatabase == null || optionPool == null)
        {
            Debug.LogError("无法触发特殊事件：事件数据库或选项池为空！");
            return;
        }
        
        RandomEventData specialEvent = eventDatabase.GetRandomSpecialEvent();
        
        if (specialEvent != null)
        {
            // 从选项池随机获取特殊事件选项
            EventOption leftOption = optionPool.GetRandomSpecialOptionLeft();
            EventOption rightOption = optionPool.GetRandomSpecialOptionRight();
            
            if (leftOption != null && rightOption != null)
            {
                // 设置运行时选项
                specialEvent.SetRuntimeOptions(new EventOption[] { leftOption, rightOption });
                
                Debug.Log($"触发特殊事件: {specialEvent.eventTitle}, 左选项: {leftOption.optionText}, 右选项: {rightOption.optionText}");
                ShowEvent(specialEvent);
            }
            else
            {
                Debug.LogWarning("没有可用的特殊事件选项！");
            }
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

        // 根据事件类型显示不同的UI和动画
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
        
        EventOption[] options = currentEvent.GetOptions();
        if (options == null || optionIndex < 0 || optionIndex >= options.Length)
        {
            Debug.LogError($"选项索引超出范围: {optionIndex}");
            return;
        }
        
        EventOption selectedOption = options[optionIndex];
        
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
    /// 检查倒计时是否激活
    /// </summary>
    public bool IsCountdownActive()
    {
        return UIManager.Instance != null && !UIManager.Instance.isPaused;
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