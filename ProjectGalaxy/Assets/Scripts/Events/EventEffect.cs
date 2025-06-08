using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Event Effect", menuName = "Random Events/Event Effect")]
public class EventEffect : ScriptableObject
{
    [Header("状态影响")]
    public int hpChange = 0;        // HP变化值
    public int sanChange = 0;       // SAN变化值
    public int shipHPChange = 0;    // 飞船HP变化值
    public int shipFuelChange = 0;  // 飞船燃料变化值
    
    [Header("效果描述")]
    [TextArea(2, 4)]
    public string effectDescription = ""; // 效果描述文本
    
    /// <summary>
    /// 应用事件效果到GameDataManager
    /// </summary>
    public void ApplyEffect()
    {
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.ChangeHP(hpChange);
            GameDataManager.Instance.ChangeSAN(sanChange);
            GameDataManager.Instance.ChangeShipHP(shipHPChange);
            GameDataManager.Instance.ChangeShipFuel(shipFuelChange);
            
            // 更新飞船状态UI
            GameDataManager.Instance.UpdateShipHPAndFuel();
            
            Debug.Log($"应用事件效果: HP{hpChange:+0;-0;0}, SAN{sanChange:+0;-0;0}, 飞船HP{shipHPChange:+0;-0;0}, 燃料{shipFuelChange:+0;-0;0}");
        }
    }
}
