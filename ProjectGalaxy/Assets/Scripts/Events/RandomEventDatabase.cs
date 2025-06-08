using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Random Event Database", menuName = "Random Events/Event Database")]
public class RandomEventDatabase : ScriptableObject
{
    [Header("普通事件池（单选项）")]
    public List<RandomEventData> normalEvents = new List<RandomEventData>();
    
    [Header("特殊事件池（双选项）")]
    public List<RandomEventData> specialEvents = new List<RandomEventData>();
    
    /// <summary>
    /// 获取可用的普通事件
    /// </summary>
    public List<RandomEventData> GetAvailableNormalEvents()
    {
        List<RandomEventData> availableEvents = new List<RandomEventData>();
        
        foreach (var eventData in normalEvents)
        {
            if (eventData != null && !eventData.hasBeenTriggered && eventData.IsValid())
            {
                availableEvents.Add(eventData);
            }
        }
        
        return availableEvents;
    }
    
    /// <summary>
    /// 获取可用的特殊事件
    /// </summary>
    public List<RandomEventData> GetAvailableSpecialEvents()
    {
        List<RandomEventData> availableEvents = new List<RandomEventData>();
        
        foreach (var eventData in specialEvents)
        {
            if (eventData != null && !eventData.hasBeenTriggered && eventData.IsValid())
            {
                availableEvents.Add(eventData);
            }
        }
        
        return availableEvents;
    }
    
    /// <summary>
    /// 随机获取一个普通事件
    /// </summary>
    public RandomEventData GetRandomNormalEvent()
    {
        var availableEvents = GetAvailableNormalEvents();
        
        if (availableEvents.Count == 0)
        {
            Debug.LogWarning("没有可用的普通事件！");
            return null;
        }
        
        int randomIndex = Random.Range(0, availableEvents.Count);
        var selectedEvent = availableEvents[randomIndex];
        selectedEvent.hasBeenTriggered = true;
        
        return selectedEvent;
    }
    
    /// <summary>
    /// 随机获取一个特殊事件
    /// </summary>
    public RandomEventData GetRandomSpecialEvent()
    {
        var availableEvents = GetAvailableSpecialEvents();
        
        if (availableEvents.Count == 0)
        {
            Debug.LogWarning("没有可用的特殊事件！");
            return null;
        }
        
        int randomIndex = Random.Range(0, availableEvents.Count);
        var selectedEvent = availableEvents[randomIndex];
        selectedEvent.hasBeenTriggered = true;
        
        return selectedEvent;
    }
    
    /// <summary>
    /// 重置所有事件状态（用于测试）
    /// </summary>
    public void ResetAllEvents()
    {
        foreach (var eventData in normalEvents)
        {
            if (eventData != null)
                eventData.ResetTriggerState();
        }
        
        foreach (var eventData in specialEvents)
        {
            if (eventData != null)
                eventData.ResetTriggerState();
        }
        
        Debug.Log("所有事件状态已重置");
    }
}