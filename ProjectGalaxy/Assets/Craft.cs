using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Craft : MonoBehaviour
{
    public GameData gameData;
    public GameData.Item[] items;
    public ItemUseConfirm popupCraftPanel;
    
    public GameObject textPrefab;
    public Canvas parentCanvas;
    public CraftInfo craftInfo;

    
    
    private void OnEnable()
    {
        
    }

    public void PopupCraftPanel(CraftInfo _craftInfo)
    {
        
        craftInfo = _craftInfo;
        string craftName = _craftInfo.craftName;
        popupCraftPanel.description.text = "确认合成<color=green>" + craftName + "</color>?这将消耗"+ _craftInfo.craftInfo + "。";
        popupCraftPanel.gameObject.SetActive(true);
    }

    public void ConfirmCraft()
    {
        foreach (var requiredItem in craftInfo.requiredItems)
        {
            foreach (var itemYouHave in gameData.warehouse)
            {
                if (itemYouHave.name == requiredItem.itemName && itemYouHave.amount >=requiredItem.itemAmount)
                {
                    requiredItem.required = true;
                }
            }
            
        }

        foreach (var requiredItem in craftInfo.requiredItems)
        {
            if (!requiredItem.required)
            {
                PopupText("材料不足",craftInfo);     
                return;
            }
            
        }

        foreach (var requiredItem in craftInfo.requiredItems)
        {
            foreach (var itemYouHave in gameData.warehouse)
            {
                if (itemYouHave.name == requiredItem.itemName)
                {
                    itemYouHave.amount -= requiredItem.itemAmount;
                }
            }
            
        }
        
        
        
        InventoryManager.Instance.AddItem(InventoryManager.Instance.items[craftInfo.craftID],1);
        
        InventoryManager.Instance.inventory.DisplayItems();
        popupCraftPanel.gameObject.SetActive(false);
    }
    
    private void PopupText(string _text,CraftInfo craftInfo)
    {
        Debug.Log("上升文字");
        
        // 实例化文本
        GameObject textObj = null;
        
        textObj  = Instantiate(textPrefab, craftInfo.transform.position, Quaternion.identity,parentCanvas.transform);
            
        
        
        TextMeshProUGUI tmpText = textObj.GetComponent<TextMeshProUGUI>();
        tmpText.text = _text;

        //启动上升协程
        StartCoroutine(FloatAndDestroyText(tmpText.rectTransform));

    }
    
    IEnumerator FloatAndDestroyText(RectTransform rectTransform)
    {
        float duration = 1.5f;    // 持续时间
        float speed = 50f;        // 上升速度

        float elapsed = 0f;
        while (elapsed < duration)
        {
            // 每帧向上移动
            rectTransform.anchoredPosition += Vector2.up * speed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 销毁对象
        Destroy(rectTransform.gameObject);
    }
}
