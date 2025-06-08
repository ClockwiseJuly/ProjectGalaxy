using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event Option Pool", menuName = "Random Events/Option Pool")]
public class EventOptionPool : ScriptableObject
{
    [Header("普通事件选项池")]
    public List<EventOption> normalOptions = new List<EventOption>();
    
    [Header("特殊事件选项池 - 左侧按钮")]
    public List<EventOption> specialOptionsLeft = new List<EventOption>();
    
    [Header("特殊事件选项池 - 右侧按钮")]
    public List<EventOption> specialOptionsRight = new List<EventOption>();
    
    /// <summary>
    /// 随机获取普通事件选项
    /// </summary>
    public EventOption GetRandomNormalOption()
    {
        if (normalOptions.Count == 0) return null;
        int randomIndex = Random.Range(0, normalOptions.Count);
        return normalOptions[randomIndex];
    }
    
    /// <summary>
    /// 随机获取特殊事件左侧选项
    /// </summary>
    public EventOption GetRandomSpecialOptionLeft()
    {
        if (specialOptionsLeft.Count == 0) return null;
        int randomIndex = Random.Range(0, specialOptionsLeft.Count);
        return specialOptionsLeft[randomIndex];
    }
    
    /// <summary>
    /// 随机获取特殊事件右侧选项
    /// </summary>
    public EventOption GetRandomSpecialOptionRight()
    {
        if (specialOptionsRight.Count == 0) return null;
        int randomIndex = Random.Range(0, specialOptionsRight.Count);
        return specialOptionsRight[randomIndex];
    }
}