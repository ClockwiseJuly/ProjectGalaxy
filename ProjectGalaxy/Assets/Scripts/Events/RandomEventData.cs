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
    
    [Header("事件选项")]
    public EventOption[] options;           // 事件选项数组
    
    [Header("调试信息")]
    public bool hasBeenTriggered = false;   // 是否已被触发（运行时使用）
    
    /// <summary>
    /// 验证事件数据的完整性
    /// </summary>
    public bool IsValid()
    {
        if (string.IsNullOrEmpty(eventTitle) || string.IsNullOrEmpty(eventDescription))
            return false;
            
        if (options == null || options.Length == 0)
            return false;
            
        // 普通事件必须有1个选项，特殊事件必须有2个选项
        if (eventType == EventType.Normal && options.Length != 1)
            return false;
            
        if (eventType == EventType.Special && options.Length != 2)
            return false;
            
        // 检查所有选项是否有效
        foreach (var option in options)
        {
            if (option == null || string.IsNullOrEmpty(option.optionText) || option.eventEffect == null)
                return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// 重置事件状态（用于测试）
    /// </summary>
    public void ResetTriggerState()
    {
        hasBeenTriggered = false;
    }
}