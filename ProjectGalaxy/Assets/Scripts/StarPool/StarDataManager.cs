using UnityEngine;
using System.Collections.Generic;

// 星球数据管理器 - 用于在场景间传递选中的星球信息
public static class StarDataManager
{
    // 当前选中的星球贴图
    public static Sprite SelectedStarSprite { get; private set; }
    
    // 当前选中的星球索引（用于调试和追踪）
    public static int SelectedStarIndex { get; private set; }
    
    // 当前选中的星球文件夹名称（用于加载对应的Animator Controller）
    public static string SelectedStarFolderName { get; private set; }
    
    // 新增：当前选中的星球名称
    public static string SelectedStarName { get; private set; }
    
    // 新增：当前选中的星球介绍文本
    public static string SelectedStarDescription { get; private set; }
    
    // 新增：星球描述数据的引用
    public static StarDescriptionData starDescriptionData;
    
    // 新增：49个星球的名称数据（备用方案）
    // 新增：49个星球的名称数据（备用方案）
    private static Dictionary<int, string> starNames = new Dictionary<int, string>()
    {
        { 0, "蔚蓝海洋星" },
        { 1, "炽热火山星" },
        { 2, "翠绿森林星" },
        { 3, "冰雪极地星" },
        { 4, "沙漠风暴星" },
        { 5, "蔚蓝海洋星" },
        { 6, "炽热火山星" },
        { 7, "翠绿森林星" },
        { 8, "冰雪极地星" },
        { 9, "沙漠风暴星" },
        { 10, "蔚蓝海洋星" }, 
        { 11, "炽热火山星" },
        { 12, "翠绿森林星" },
        { 13, "冰雪极地星" },
        { 14, "沙漠风暴星" },
        { 15, "蔚蓝海洋星" },
        { 16, "炽热火山星" },
        { 17, "翠绿森林星" },
        { 18, "冰雪极地星" },
        { 19, "沙漠风暴星" },
        { 20, "蔚蓝海洋星" },
        { 21, "炽热火山星" },
        { 22, "翠绿森林星" },
        { 23, "冰雪极地星" },
        { 24, "沙漠风暴星" },
        { 25, "蔚蓝海洋星" },
        { 26, "炽热火山星" },
        { 27, "翠绿森林星" },
        { 28, "冰雪极地星" },
        { 29, "沙漠风暴星" },
        { 30, "蔚蓝海洋星" },
        { 31, "炽热火山星" },
        { 32, "翠绿森林星" },
        { 33, "冰雪极地星" },
        { 34, "沙漠风暴星" },
        { 35, "蔚蓝海洋星" },
        { 36, "炽热火山星" },
        { 37, "翠绿森林星" },
        { 38, "冰雪极地星" },
        { 39, "沙漠风暴星" },
        { 40, "蔚蓝海洋星" },
        { 41, "炽热火山星" },
        { 42, "翠绿森林星" },
        { 43, "冰雪极地星" },
        { 44, "沙漠风暴星" },
        { 45, "蔚蓝海洋星" },
        { 46, "蔚蓝海洋星" },
        { 47, "炽热火山星" },
        { 48, "翠绿森林星" },
        { 49, "冰雪极地星" }
    };
    
    // 新增：49个星球的介绍文本数据（备用方案）
    private static Dictionary<int, string> starDescriptions = new Dictionary<int, string>()
    {
        { 0, "这是一颗神秘的蓝色星球，表面覆盖着广阔的海洋，蕴含着生命的起源..." },
        { 1, "炽热的红色星球，拥有活跃的火山活动，岩浆四溅，充满原始的力量..." },
        { 2, "翠绿的森林星球，生机勃勃，古老的树木见证着时间的流逝..." },
        { 3, "冰雪覆盖的极地星球，永恒的寒冷中隐藏着古老的秘密..." },
        { 4, "沙漠风暴肆虐的星球，金色的沙丘中埋藏着失落的文明..." },
        { 5, "这是一颗神秘的蓝色星球，表面覆盖着广阔的海洋，蕴含着生命的起源..." },
        { 6, "炽热的红色星球，拥有活跃的火山活动，岩浆四溅，充满原始的力量..." },
        { 7, "翠绿的森林星球，生机勃勃，古老的树木见证着时间的流逝..." },
        { 8, "冰雪覆盖的极地星球，永恒的寒冷中隐藏着古老的秘密..." },
        { 9, "沙漠风暴肆虐的星球，金色的沙丘中埋藏着失落的文明..." },
        { 10, "这是一颗神秘的蓝色星球，表面覆盖着广阔的海洋，蕴含着生命的起源..." },
        { 11, "炽热的红色星球，拥有活跃的火山活动，岩浆四溅，充满原始的力量..." },
        { 12, "翠绿的森林星球，生机勃勃，古老的树木见证着时间的流逝..." },
        { 13, "冰雪覆盖的极地星球，永恒的寒冷中隐藏着古老的秘密..." },
        { 14, "沙漠风暴肆虐的星球，金色的沙丘中埋藏着失落的文明..." },
        { 15, "这是一颗神秘的蓝色星球，表面覆盖着广阔的海洋，蕴含着生命的起源..." },
        { 16, "炽热的红色星球，拥有活跃的火山活动，岩浆四溅，充满原始的力量..." },
        { 17, "翠绿的森林星球，生机勃勃，古老的树木见证着时间的流逝..." },
        { 18, "冰雪覆盖的极地星球，永恒的寒冷中隐藏着古老的秘密..." },
        { 19, "沙漠风暴肆虐的星球，金色的沙丘中埋藏着失落的文明..." },
        { 20, "这是一颗神秘的蓝色星球，表面覆盖着广阔的海洋，蕴含着生命的起源..." },
        { 21, "炽热的红色星球，拥有活跃的火山活动，岩浆四溅，充满原始的力量..." },
        { 22, "翠绿的森林星球，生机勃勃，古老的树木见证着时间的流逝..." },
        { 23, "冰雪覆盖的极地星球，永恒的寒冷中隐藏着古老的秘密..." },
        { 24, "沙漠风暴肆虐的星球，金色的沙丘中埋藏着失落的文明..." },
        { 25, "这是一颗神秘的蓝色星球，表面覆盖着广阔的海洋，蕴含着生命的起源..." },
        { 26, "炽热的红色星球，拥有活跃的火山活动，岩浆四溅，充满原始的力量..." },
        { 27, "翠绿的森林星球，生机勃勃，古老的树木见证着时间的流逝..." },
        { 28, "冰雪覆盖的极地星球，永恒的寒冷中隐藏着古老的秘密..." },
        { 29, "沙漠风暴肆虐的星球，金色的沙丘中埋藏着失落的文明..." },
        { 30, "这是一颗神秘的蓝色星球，表面覆盖着广阔的海洋，蕴含着生命的起源..." },
        { 31, "炽热的红色星球，拥有活跃的火山活动，岩浆四溅，充满原始的力量..." },
        { 32, "翠绿的森林星球，生机勃勃，古老的树木见证着时间的流逝..." },
        { 33, "冰雪覆盖的极地星球，永恒的寒冷中隐藏着古老的秘密..." },
        { 34, "沙漠风暴肆虐的星球，金色的沙丘中埋藏着失落的文明..." },
        { 35, "这是一颗神秘的蓝色星球，表面覆盖着广阔的海洋，蕴含着生命的起源..." },
        { 36, "炽热的红色星球，拥有活跃的火山活动，岩浆四溅，充满原始的力量..." },
        { 37, "翠绿的森林星球，生机勃勃，古老的树木见证着时间的流逝..." },
        { 38, "冰雪覆盖的极地星球，永恒的寒冷中隐藏着古老的秘密..." },
        { 39, "沙漠风暴肆虐的星球，金色的沙丘中埋藏着失落的文明..." },
        { 40, "这是一颗神秘的蓝色星球，表面覆盖着广阔的海洋，蕴含着生命的起源..." },
        { 41, "炽热的红色星球，拥有活跃的火山活动，岩浆四溅，充满原始的力量..." },
        { 42, "翠绿的森林星球，生机勃勃，古老的树木见证着时间的流逝..." },
        { 43, "冰雪覆盖的极地星球，永恒的寒冷中隐藏着古老的秘密..." },
        { 44, "沙漠风暴肆虐的星球，金色的沙丘中埋藏着失落的文明..." },
        { 45, "这是一颗神秘的蓝色星球，表面覆盖着广阔的海洋，蕴含着生命的起源..." },
        { 46, "炽热的红色星球，拥有活跃的火山活动，岩浆四溅，充满原始的力量..." },
        { 47, "翠绿的森林星球，生机勃勃，古老的树木见证着时间的流逝..." },
        { 48, "冰雪覆盖的极地星球，永恒的寒冷中隐藏着古老的秘密..." },
        { 49, "沙漠风暴肆虐的星球，金色的沙丘中埋藏着失落的文明..." }
    };
    
    // 设置选中的星球数据
    /// <param name="sprite">星球贴图</param>
    /// <param name="index">星球索引</param>
    // 在SetSelectedStar方法中添加调试
    public static void SetSelectedStar(Sprite sprite, int index)
    {
        SelectedStarSprite = sprite;
        SelectedStarIndex = index;
        
        // 从贴图路径中提取星球文件夹名称
        SelectedStarFolderName = ExtractStarFolderName(sprite);
        
        // 新增：设置对应的星球名称和介绍文本
        SelectedStarName = GetStarName(index);
        SelectedStarDescription = GetStarDescription(index);
        
        Debug.Log($"设置选中星球：索引 {index}，名称：{SelectedStarName}，贴图名称：{sprite?.name}，文件夹名称：{SelectedStarFolderName}");
    }
    
    // 新增：获取指定索引的星球名称
    public static string GetStarName(int index)
    {
        // 优先使用StarDescriptionData
        if (starDescriptionData != null)
        {
            return starDescriptionData.GetStarName(index);
        }
        
        // 备用方案：使用内置字典
        if (starNames.ContainsKey(index))
        {
            return starNames[index];
        }
        return $"星球 {index + 1}";
    }
    
    // 新增：获取指定索引的星球介绍文本
    public static string GetStarDescription(int index)
    {
        // 优先使用StarDescriptionData
        if (starDescriptionData != null)
        {
            return starDescriptionData.GetDescription(index);
        }
        
        // 备用方案：使用内置字典
        if (starDescriptions.ContainsKey(index))
        {
            return starDescriptions[index];
        }
        return $"星球 {index + 1}：暂无介绍信息";
    }
    
    // 从Sprite中提取星球文件夹名称
    private static string ExtractStarFolderName(Sprite sprite)
    {
        if (sprite == null) return null;
        
#if UNITY_EDITOR
        // 在编辑器中，我们可以使用AssetDatabase获取完整路径
        string assetPath = UnityEditor.AssetDatabase.GetAssetPath(sprite.texture);
        if (!string.IsNullOrEmpty(assetPath))
        {
            // 路径格式：Assets/Resources/Stars/teststar1/frame_0001.png
            string[] pathParts = assetPath.Split('/');
            for (int i = 0; i < pathParts.Length; i++)
            {
                if (pathParts[i].StartsWith("teststar"))
                {
                    Debug.Log($"从路径 {assetPath} 中提取到文件夹名称：{pathParts[i]}");
                    return pathParts[i];
                }
            }
        }
#endif
        
        // 运行时的备用方案：通过贴图的名称和Resources路径推断
        // 由于所有贴图都叫frame_0001，我们需要通过其他方式
        // 这里我们尝试通过sprite.texture.name来获取信息
        if (sprite.texture != null)
        {
            string textureName = sprite.texture.name;
            Debug.Log($"贴图名称：{textureName}");
            
            // 如果贴图名称包含路径信息，尝试提取
            if (textureName.Contains("teststar"))
            {
                // 查找teststar开头的部分
                int startIndex = textureName.IndexOf("teststar");
                if (startIndex >= 0)
                {
                    string remaining = textureName.Substring(startIndex);
                    // 查找下一个分隔符或结束
                    int endIndex = remaining.IndexOfAny(new char[] { '/', '\\', '_', '.' });
                    if (endIndex > 0)
                    {
                        string folderName = remaining.Substring(0, endIndex);
                        Debug.Log($"从贴图名称中提取到文件夹名称：{folderName}");
                        return folderName;
                    }
                    else
                    {
                        Debug.Log($"从贴图名称中提取到文件夹名称：{remaining}");
                        return remaining;
                    }
                }
            }
        }
        
        Debug.LogWarning($"无法从贴图 {sprite.name} 中提取文件夹名称");
        return null;
    }
    
    // 清除选中的星球数据
    public static void ClearSelectedStar()
    {
        SelectedStarSprite = null;
        SelectedStarIndex = -1;
        SelectedStarFolderName = null;
        SelectedStarName = null; // 新增
        SelectedStarDescription = null; // 新增
    }
    
    // 检查是否有有效的选中星球数据
    public static bool HasValidSelectedStar()
    {
        return SelectedStarSprite != null && !string.IsNullOrEmpty(SelectedStarFolderName);
    }
}