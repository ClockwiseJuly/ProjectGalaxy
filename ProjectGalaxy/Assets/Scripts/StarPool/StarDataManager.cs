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
        // 0-6 TerranWet 类地湿润行星
        { 0, "新伊甸" },
        { 1, "蔚蓝" },
        { 2, "涟漪之环" },
        { 3, "潮汐之眼" },
        { 4, "翠影" },
        { 5, "水镜座α" },
        { 6, "蓝梦泽" },
        // 7-13 TerranDry 类地干燥行星
        { 7, "赤沙"},
        { 8, "风蚀谷" },
        { 9, "荒原之心"},
        { 10, "遗迹之地"},
        { 11, "流火" },
        { 12, "银沙荒漠" },
        { 13, "沙暴环" },
        // 14-20 Islands 群岛行星
        { 14, "浮岛星" },
        { 15, "珊瑚环礁" },
        { 16, "幻影" },
        { 17, "潮汐迷宫" },
        { 18, "浮光" },
        { 19, "银翼湾"},
        { 20, "迷雾晨星" },
        // 21-27 NoAtmosphere 无大气行星
        { 21,  "寂静之岩" },
        { 22, "林语星" },
        { 23, "幽影岩" },
        { 24, "极夜之境" },
        { 25, "沼泽荒原" },
        { 26, "碎环"  },
        { 27, "奶酪" },
        // 28-31 GasGiant 气态巨行星（无星环）
        { 28, "蜂巢" },
        { 29, "雷霆之眼" },
        { 30, "湍流" },
        { 31, "提拉米苏" },
        // 32-34 GasGiant 气态巨行星（有星环）
        { 32, "苍穹" },
        { 33, "风暴灵" },
        { 34, "流金" },
        // 35-37 冰冻行星
        { 35, "冰语" },
        { 36, "冻土座β" },
        { 37, "雪境" },
        // 38-41 熔岩行星
        { 38, "焰核"},
        { 39, "赤霓" },
        { 40, "熔光" },
        { 41, "废核" },
        // 42-44 恒星
        { 42, "曙光" },
        { 43, "银心之耀" },
        { 44, "北宸" },
        // 45-46 爆炸的恒星
        { 45, "超新星爆" },
        { 46, "湮灭余烬" },
        // 47-48 恒星
        { 47, "紫晶" },
        { 48, "鑫" }
    };
    private static Dictionary<int, string> starDescriptions = new Dictionary<int, string>()
    {
        // 0-6 TerranWet 类地湿润行星
        { 0, "温暖湿润的气候，森林与河流纵横交错，适宜居住。" },
        { 1, "这是一颗神秘的蓝色星球，表面覆盖着广阔的海洋，蕴含着生命的起源。" },
        { 2, "星球表面被无数湖泊点缀，水汽蒸腾，孕育着奇异生态。" },
        { 3, "巨大的潮汐能量场影响着星球生态，海洋生物繁盛。"},
        { 4, "星球被翠绿色植被覆盖，雨林深处隐藏未知生物。"},
        { 5, "如镜的湖泊反射星空，夜晚常现极光奇观。" },
        { 6, "星球大气湿润，常年云雾缭绕，水资源极为丰富。"  },
        // 7-13 TerranDry 类地干燥行星
        { 7, "星球表面覆盖着红色沙漠，昼夜温差极大。" },
        { 8, "干燥的风长期侵蚀地表，形成奇特的岩石地貌。"},
        { 9, "广袤荒原上偶有绿洲点缀，生命顽强生存。"},
        { 10, "古老文明遗迹散布在干涸河床之间，等待探索。" },
        { 11, "地表岩浆流动遗迹清晰可见，气候干热。" },
        { 12, "黄昏时分，整个星球被银灰色沙尘笼罩。"  },
        { 13, "频繁的沙尘暴席卷星球，能见度极低。"  },
        // 14-20 Islands 群岛行星
        { 14, "无数岛屿漂浮在蔚蓝海洋之上，生态多样。" },
        { 15, "星球海平面下潜藏着巨大的珊瑚礁，海洋生物丰富。"},
        { 16, "岛屿位置经常变化，传说中藏有宝藏。"  },
        { 17, "潮汐涨落形成天然迷宫，考验探险者智慧。" },
        { 18, "浮光掠影的列岛星球，岛屿星罗棋布，海洋资源极为丰富。"},
        { 19, "岛屿形状如银翼，海风吹拂，景色宜人。" },
        { 20, "夜晚岛屿上空繁星点点，宛如银河落入海中。" },
        // 21-27 NoAtmosphere 无大气行星
        { 21, "无大气层保护，星球表面布满蓝灰色陨石坑。" },
        { 22, "被遗弃的卫星，表面刻满神秘符号。" },
        { 23, "星球表面反射极少光线，难以观测。" },
        { 24, "一半以上永远处于黑暗中的星球，温度极低。"  },
        { 25,"褐绿色灰色岩石反射着冷冽星光，环境极端恶劣。" },
        { 26, "星球被自身碎片环绕，宛如天然护盾。"},
        { 27, "从太空看去，这颗星球宛如一块巨大的球形奶酪" },
        // 28-31 GasGiant 气态巨行星（无星环）
        { 28, "橙色和褐色气体纵横交错,仿佛千万只蜂类生物同时振翅刮起的风暴" },
        { 29, "极端气候下，星球表面常现闪电风暴。" },
        { 30, "气流湍急，云层色彩斑斓，极具观赏性。" },
        { 31, "颜色如一款糕点般的行星，气流裹挟着尘土片刻也不停歇" },
        // 32-34 GasGiant 气态巨行星（有星环）
        { 32, "庞大的气体层包裹着神秘核心，体积巨大。" },
        { 33, "星球大气层中常年有巨型风暴肆虐，风暴中似乎有什么巨型生物的阴影。"},
        { 34, "星球表面闪烁着金属般的光泽，绚丽的光泽下似乎隐藏着危险" },
        // 35-37 冰冻行星
        { 35, "星球表面覆盖厚厚冰层，温度极低。"},
        { 36, "冻土广阔，偶有地热喷口带来生机。" },
        { 37, "终年飘雪，极光频现，景色如梦似幻。" },
        // 38-41 熔岩行星
        { 38, "星球内部炽热，地表岩浆流动不息。" },
        { 39, "火山频发，星球表面如同燃烧的地狱。" },
        { 40, "熔岩河流纵横交错，夜晚如同灯火通明。" },
        { 41, "整个星球如同火焰彻底焚烧后的灰烬，毫无生机。" },
        // 42-44 恒星
        { 42, "恒星发出温暖光芒，孕育周围行星生命。" },
        { 43, "位于某个星系中心，能量极为强大。" },
        { 44, "恒星光芒照亮数光年，成为导航标志。" },
        // 45-46 爆炸的恒星
        { 45, "恒星爆发产生强烈能量波，影响整个星域。" },
        { 46, "剧烈爆炸后只剩下神秘残骸，充满未知。" },
        // 47-48 恒星
        { 47, "爆裂恒星，能量极度不稳定，随时可能发生剧烈爆发。" },
        { 48, "光芒耀眼璀璨，充满未知与神秘。" }
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