using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public GameData gameData;
    public Inventory inventory;
    
    public GameData.Item[] items;
    
    private void Update()
    {

        
    }
    
    public void AddItem(GameData.Item newItem, int amount)
    {
        //检查背包是否有该物品
        var existingItem = gameData.warehouse.Find(item => item.name == newItem.name);
        if (existingItem != null)
        {
            existingItem.amount += amount;
        }
        else
        {
            gameData.warehouse.Add(newItem);
        }
        inventory.DisplayItems();
        Debug.Log($"仓库添加物品：{newItem.name}，数量为{newItem.amount}");    
        
        
    }

    public void RemoveItem(string itemName, int amount)
    {
        // 查找背包中的物品
        var item = gameData.warehouse.Find(i => i.name == itemName);
        if (item != null)
        {
            item.amount -= amount; // 减少数量
            if (item.amount <= 0)
            {
                item.amount = 0;
                gameData.warehouse.Remove(item); // 数量为0时移除物品
            }
            inventory.DisplayItems();
            Debug.Log($"仓库移除物品: {itemName}, 数量为: {amount}");
        }
        else
        {
            Debug.LogWarning($"物品未找到: {itemName}");
        }
    }
}
