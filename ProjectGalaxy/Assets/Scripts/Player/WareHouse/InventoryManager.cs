using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public GameData gameData;
    public Inventory inventory;
    public ItemUseConfirm popupConfirmPanel;
    public GameData.Item thisItem;
    
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
    
    public void GainMagnifyResource()
    {
        int randomValue = UnityEngine.Random.Range(0, 100);
        
        if (randomValue < 50)
        {
            int randomNum = UnityEngine.Random.Range(0, 3);
            
            InventoryManager.Instance.AddItem(items[8], randomNum); //植物
        }
        else if (randomValue <= 75 && randomValue > 50)
        {
            int randomNum = UnityEngine.Random.Range(0, 3);
            
            InventoryManager.Instance.AddItem(items[7], randomNum);//有机矿物
        }
        else
        {
            int randomNum = UnityEngine.Random.Range(0, 3);
            
            InventoryManager.Instance.AddItem(items[6], randomNum);//无机矿物
        }
    }
    
    public void GainCloudResource()
    {
        int randomValue = UnityEngine.Random.Range(0, 100);
        
        if (randomValue < 60)
        {
            int randomNum = UnityEngine.Random.Range(1, 5);
            
            InventoryManager.Instance.AddItem(items[8], randomNum); //植物
        }
        else if (randomValue <= 80 && randomValue > 60)
        {
            int randomNum = UnityEngine.Random.Range(0, 4);
            
            InventoryManager.Instance.AddItem(items[5], randomNum);//可燃冰
        }
        else
        {
            int randomNum = UnityEngine.Random.Range(0, 3);
            
            InventoryManager.Instance.AddItem(items[7], randomNum);//有机矿物
        }
    }
    
    public void GainHouseResource()
    {
        int randomValue = UnityEngine.Random.Range(0, 100);
        
        if (randomValue < 35)
        {
            int randomNum = UnityEngine.Random.Range(1, 5);
            
            InventoryManager.Instance.AddItem(items[6], randomNum);//无机矿物
        }
        else if (randomValue <= 55 && randomValue > 35)
        {
            int randomNum = UnityEngine.Random.Range(1, 3);
            
            InventoryManager.Instance.AddItem(items[3], randomNum);//实验性药剂
        }
        else if (randomValue <= 85 && randomValue > 55)
        {
            int randomNum = UnityEngine.Random.Range(1, 3);
            
            InventoryManager.Instance.AddItem(items[7], randomNum);//有机矿物
        }
        else 
        {
            int randomNum = UnityEngine.Random.Range(1, 2);
            
            InventoryManager.Instance.AddItem(items[2], randomNum);//医疗包
        }
    }
    
    public void GainLightResource()
    {
        int randomValue = UnityEngine.Random.Range(0, 100);
        
        if (randomValue < 20)
        {
            int randomNum = UnityEngine.Random.Range(1, 3);
            
            InventoryManager.Instance.AddItem(items[10], randomNum);//纳米胶
        }
        else if (randomValue <= 55 && randomValue > 20)
        {
            int randomNum = UnityEngine.Random.Range(1, 4);
            
            InventoryManager.Instance.AddItem(items[7], randomNum);//有机矿物
        }
        else if (randomValue <= 85 && randomValue > 55)
        {
            int randomNum = UnityEngine.Random.Range(1, 4);
            
            InventoryManager.Instance.AddItem(items[6], randomNum);//无机矿物
        }
        else 
        {
            int randomNum = UnityEngine.Random.Range(1, 4);
            
            InventoryManager.Instance.AddItem(items[9], randomNum);//稀土
        }
    }
    
    public void GainEyeResource()
    {
        int randomValue = UnityEngine.Random.Range(0, 100);
        
        if (randomValue < 45)
        {
            int randomNum = UnityEngine.Random.Range(0, 3);
            
            InventoryManager.Instance.AddItem(items[10], randomNum);//纳米胶
        }
        else if (randomValue <= 80 && randomValue > 45)
        {
            int randomNum = UnityEngine.Random.Range(0, 1);
            
            InventoryManager.Instance.AddItem(items[4], randomNum);//核能燃料
        }
        
        else 
        {
            int randomNum = UnityEngine.Random.Range(0, 4);
            
            InventoryManager.Instance.AddItem(items[1], randomNum);//镇定剂
        }
    }
    
    public void GainSkullResource()
    {
        int randomValue = UnityEngine.Random.Range(0, 100);
        
        if (randomValue < 35)
        {
            int randomNum = UnityEngine.Random.Range(1, 5);
            
            InventoryManager.Instance.AddItem(items[3], randomNum);//实验性药剂
        }
        else if (randomValue <= 50 && randomValue > 35)
        {
            int randomNum = UnityEngine.Random.Range(0, 2);
            
            InventoryManager.Instance.AddItem(items[4], randomNum);//医疗包
        }
        else 
        {
            int randomNum = UnityEngine.Random.Range(0, 3);
            
            InventoryManager.Instance.AddItem(items[9], randomNum);//稀土
        }
    }

    public void ConfirmUseThisItem()
    {
        
        if (thisItem != null)
        {
            thisItem.amount -= 1;
            Debug.Log($"已使用{thisItem.name}");
            inventory.DisplayItems();
        }
    }
}
