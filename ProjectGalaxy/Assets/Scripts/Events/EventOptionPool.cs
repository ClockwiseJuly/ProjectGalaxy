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
    
    /// <summary>
    /// 根据索引获取普通事件选项
    /// </summary>
    public EventOption GetNormalOptionByIndex(int index)
    {
        foreach (var option in normalOptions)
        {
            if (option != null)
            {
                string optionName = option.name;
                if (ExtractNumberFromName(optionName) == index)
                {
                    return option;
                }
            }
        }
        return null;
    }
    
    /// <summary>
    /// 根据索引获取特殊事件左侧选项
    /// </summary>
    public EventOption GetSpecialOptionLeftByIndex(int index)
    {
        foreach (var option in specialOptionsLeft)
        {
            if (option != null)
            {
                string optionName = option.name;
                if (ExtractNumberFromName(optionName) == index)
                {
                    return option;
                }
            }
        }
        return null;
    }
    
    /// <summary>
    /// 根据索引获取特殊事件右侧选项
    /// </summary>
    public EventOption GetSpecialOptionRightByIndex(int index)
    {
        foreach (var option in specialOptionsRight)
        {
            if (option != null)
            {
                string optionName = option.name;
                if (ExtractNumberFromName(optionName) == index)
                {
                    return option;
                }
            }
        }
        return null;
    }
    
    /// <summary>
    /// 从名字中提取数字索引
    /// </summary>
    private int ExtractNumberFromName(string name)
    {
        if (string.IsNullOrEmpty(name)) return -1;
        
        // 使用正则表达式提取数字
        var match = System.Text.RegularExpressions.Regex.Match(name, @"\d+");
        if (match.Success)
        {
            return int.Parse(match.Value);
        }
        return -1;
    }
}