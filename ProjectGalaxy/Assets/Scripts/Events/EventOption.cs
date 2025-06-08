using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Event Option", menuName = "Random Events/Event Option")]
public class EventOption : ScriptableObject
{
    [Header("选项信息")]
    public string optionText = "";          // 选项显示文本
    
    [Header("选项效果")]
    public EventEffect eventEffect;         // 选择此选项的效果
    
    [Header("结果反馈")]
    [TextArea(2, 4)]
    public string resultText = "";          // 选择后的结果描述
}