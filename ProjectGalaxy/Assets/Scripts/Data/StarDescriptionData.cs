using UnityEngine;

[CreateAssetMenu(fileName = "StarDescriptionData", menuName = "Galaxy/Star Description Data")]
public class StarDescriptionData : ScriptableObject
{
    [System.Serializable]
    public class StarInfo
    {
        [Header("星球基本信息")]
        public int index;
        
        [Header("星球名称")]
        public string starName;
        
        [Header("星球介绍")]
        [TextArea(3, 8)]
        public string description;
    }
    
    [Header("49个星球的信息数据")]
    public StarInfo[] starInfos = new StarInfo[49];
    
    // 获取星球名称
    public string GetStarName(int index)
    {
        foreach (var info in starInfos)
        {
            if (info.index == index)
            {
                return string.IsNullOrEmpty(info.starName) ? $"星球 {index + 1}" : info.starName;
            }
        }
        return $"星球 {index + 1}";
    }
    
    // 获取星球介绍
    public string GetDescription(int index)
    {
        foreach (var info in starInfos)
        {
            if (info.index == index)
            {
                return string.IsNullOrEmpty(info.description) ? $"星球 {index + 1}：暂无介绍信息" : info.description;
            }
        }
        return $"星球 {index + 1}：暂无介绍信息";
    }
    
    // 编辑器中初始化数据的辅助方法
    [ContextMenu("Initialize Star Data")]
    public void InitializeStarData()
    {
        starInfos = new StarInfo[49];
        for (int i = 0; i < 49; i++)
        {
            starInfos[i] = new StarInfo
            {
                index = i,
                starName = $"星球 {i + 1}",
                description = $"这是第 {i + 1} 颗星球的介绍信息，等待你来编辑..."
            };
        }
    }
}