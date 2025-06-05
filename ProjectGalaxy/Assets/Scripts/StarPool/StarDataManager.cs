using UnityEngine;

// 星球数据管理器 - 用于在场景间传递选中的星球信息
public static class StarDataManager
{
    // 当前选中的星球贴图
    public static Sprite SelectedStarSprite { get; private set; }
    
    // 当前选中的星球索引（用于调试和追踪）
    public static int SelectedStarIndex { get; private set; }
    
    // 当前选中的星球文件夹名称（用于加载对应的Animator Controller）
    public static string SelectedStarFolderName { get; private set; }
    
    // 设置选中的星球数据
    /// <param name="sprite">星球贴图</param>
    /// <param name="index">星球索引</param>
    public static void SetSelectedStar(Sprite sprite, int index)
    {
        SelectedStarSprite = sprite;
        SelectedStarIndex = index;
        
        // 从贴图路径中提取星球文件夹名称
        SelectedStarFolderName = ExtractStarFolderName(sprite);
        
        Debug.Log($"设置选中星球：索引 {index}，贴图名称：{sprite?.name}，文件夹名称：{SelectedStarFolderName}");
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
    }
    
    // 检查是否有有效的选中星球数据
    public static bool HasValidSelectedStar()
    {
        return SelectedStarSprite != null && !string.IsNullOrEmpty(SelectedStarFolderName);
    }
}