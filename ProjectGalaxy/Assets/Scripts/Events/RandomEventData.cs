using UnityEngine;

public enum EventType
{
    Normal,     // 普通事件（单选项）
    Special     // 特殊事件（双选项）
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Random Event", menuName = "Random Events/Random Event Data")]
public class RandomEventData : ScriptableObject
{
    [Header("事件基本信息")]
    public string eventTitle = "";          // 事件标题
    
    [TextArea(3, 6)]
    public string eventDescription = "";    // 事件描述
    
    public Sprite eventImage;               // 事件图片/图标
    
    public EventType eventType = EventType.Normal; // 事件类型
    
    [Header("运行时选项（由系统自动分配）")]
    [System.NonSerialized]
    public EventOption[] runtimeOptions;    // 运行时分配的选项
    
    [Header("调试信息")]
    public bool hasBeenTriggered = false;   // 是否已被触发（运行时使用）
    
    /// <summary>
    /// 设置运行时选项
    /// </summary>
    public void SetRuntimeOptions(EventOption[] options)
    {
        runtimeOptions = options;
    }
    
    /// <summary>
    /// 获取运行时选项
    /// </summary>
    public EventOption[] GetOptions()
    {
        return runtimeOptions;
    }
    
    /// <summary>
    /// 验证事件数据的完整性
    /// </summary>
    public bool IsValid()
    {
        if (string.IsNullOrEmpty(eventTitle) || string.IsNullOrEmpty(eventDescription))
            return false;
            
        return true;
    }
    
    /// <summary>
    /// 重置触发状态
    /// </summary>
    public void ResetTriggerState()
    {
        hasBeenTriggered = false;
        runtimeOptions = null;
    }
}