using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 新增：星球信息数据结构
[System.Serializable]
public class StarInfo
{
    public Sprite sprite;           // 星球贴图
    public string folderName;       // 文件夹名称（如 teststar1）
    public int globalIndex;         // 全局索引（1-49）
    
    public StarInfo(Sprite sprite, string folderName, int globalIndex)
    {
        this.sprite = sprite;
        this.folderName = folderName;
        this.globalIndex = globalIndex;
    }
}

[System.Serializable]
public class StarGroup
{
    public string groupName;
    public StarInfo[] starInfos;        // 修改：使用StarInfo替代Sprite数组
    public List<int> availableIndices;  // 可用的星球索引
    
    public StarGroup(string name, int starCount)
    {
        groupName = name;
        starInfos = new StarInfo[starCount];  // 修改：初始化StarInfo数组
        availableIndices = new List<int>();
        for (int i = 0; i < starCount; i++)
        {
            availableIndices.Add(i);
        }
    }
}

[System.Serializable]
public class StarPoolSaveData
{
    public List<List<int>> groupAvailableIndices = new List<List<int>>();
}

public class StarPoolManager : MonoBehaviour
{
    public static StarPoolManager Instance;
    
    [Header("星球池配置")]
    public StarGroup[] starGroups = new StarGroup[7]; // 7个星球组
    
    // 新增：当前轮次选中的星球信息
    private StarInfo[] currentRoundStarInfos;
    
    private const string SAVE_KEY = "StarPoolData";
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeStarPool();
            LoadStarPoolState();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializeStarPool()
    {
        // 初始化7个星球组，每组7个星球
        for (int groupIndex = 0; groupIndex < 7; groupIndex++)
        {
            starGroups[groupIndex] = new StarGroup($"Group{groupIndex + 1}", 7);
            
            // 加载每组的星球贴图（第一帧）
            for (int starIndex = 0; starIndex < 7; starIndex++)
            {
                // 计算全局星球索引：组索引 * 7 + 星球索引
                int globalStarIndex = groupIndex * 7 + starIndex + 1;
                string folderName = $"teststar{globalStarIndex}";
                string spritePath = $"Stars/{folderName}/frame_0001";
                
                // 从Resources加载sprite
                Sprite starSprite = Resources.Load<Sprite>(spritePath);
                
                if (starSprite != null)
                {
                    // 创建StarInfo对象
                    starGroups[groupIndex].starInfos[starIndex] = new StarInfo(starSprite, folderName, globalStarIndex);
                }
                else
                {
                    Debug.LogWarning($"无法加载星球贴图: {spritePath}");
                }
            }
        }
    }
    
    // 修改：返回StarInfo数组而不是Sprite数组
    public StarInfo[] GetRandomStarInfosForRound()
    {
        StarInfo[] selectedStarInfos = new StarInfo[7];
        
        for (int groupIndex = 0; groupIndex < 7; groupIndex++)
        {
            if (starGroups[groupIndex].availableIndices.Count > 0)
            {
                // 从可用索引中随机选择一个
                int randomIndex = Random.Range(0, starGroups[groupIndex].availableIndices.Count);
                int selectedStarIndex = starGroups[groupIndex].availableIndices[randomIndex];
                
                // 获取选中的星球信息
                selectedStarInfos[groupIndex] = starGroups[groupIndex].starInfos[selectedStarIndex];
                
                // 从可用列表中移除已选择的星球
                starGroups[groupIndex].availableIndices.RemoveAt(randomIndex);
                
                Debug.Log($"第{groupIndex + 1}组选择了星球: {selectedStarInfos[groupIndex].folderName}");
            }
            else
            {
                Debug.LogWarning($"第{groupIndex + 1}组星球已全部使用完毕！");
                // 可以考虑重置该组或使用默认贴图
            }
        }
        
        // 保存当前轮次的星球信息
        currentRoundStarInfos = selectedStarInfos;
        
        // 保存当前状态
        SaveStarPoolState();
        
        return selectedStarInfos;
    }
    
    // 兼容性方法：返回Sprite数组（供现有代码使用）
    public Sprite[] GetRandomStarsForRound()
    {
        StarInfo[] starInfos = GetRandomStarInfosForRound();
        Sprite[] sprites = new Sprite[starInfos.Length];
        
        for (int i = 0; i < starInfos.Length; i++)
        {
            sprites[i] = starInfos[i]?.sprite;
        }
        
        return sprites;
    }
    
    // 新增：根据选项索引获取对应的星球信息
    public StarInfo GetStarInfoByOptionIndex(int optionIndex)
    {
        if (currentRoundStarInfos != null && optionIndex >= 0 && optionIndex < currentRoundStarInfos.Length)
        {
            return currentRoundStarInfos[optionIndex];
        }
        return null;
    }
    
    // 保存星球池状态到PlayerPrefs
    void SaveStarPoolState()
    {
        StarPoolSaveData saveData = new StarPoolSaveData();
        
        foreach (var group in starGroups)
        {
            saveData.groupAvailableIndices.Add(new List<int>(group.availableIndices));
        }
        
        string jsonData = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
        PlayerPrefs.Save();
        
        Debug.Log("星球池状态已保存");
    }
    
    // 从PlayerPrefs加载星球池状态
    void LoadStarPoolState()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            StarPoolSaveData saveData = JsonUtility.FromJson<StarPoolSaveData>(jsonData);
            
            for (int i = 0; i < starGroups.Length && i < saveData.groupAvailableIndices.Count; i++)
            {
                starGroups[i].availableIndices = new List<int>(saveData.groupAvailableIndices[i]);
            }
            
            Debug.Log("星球池状态已加载");
        }
    }
    
    // 重置星球池
    public void ResetStarPool()
    {
        foreach (var group in starGroups)
        {
            group.availableIndices.Clear();
            for (int i = 0; i < group.starInfos.Length; i++)
            {
                group.availableIndices.Add(i);
            }
        }
        SaveStarPoolState();
        Debug.Log("星球池已重置");
    }
    
    // 获取剩余星球数量
    public void PrintRemainingStars()
    {
        for (int i = 0; i < starGroups.Length; i++)
        {
            Debug.Log($"第{i + 1}组剩余星球数量: {starGroups[i].availableIndices.Count}");
        }
    }
}