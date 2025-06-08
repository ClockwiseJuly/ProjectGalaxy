using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WareHouseItem : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public GameData gameData;
    public Sprite icon;
    public string name;
    public string description;
    public bool hasItem = false;
    public bool canUse= false;

    private void OnEnable()
    {
        int myIndex = transform.GetSiblingIndex();
        if (gameData.warehouse.Count > myIndex)
        {
            hasItem = true;
            name = gameData.warehouse[myIndex].name;
            description = gameData.warehouse[myIndex].description;
            icon = gameData.warehouse[myIndex].icon;
            canUse = gameData.warehouse[myIndex].canUse;
            
        }
        
    }

    private void OnDisable()
    {
        if (hasItem)
        {
            hasItem = false;
            name = null;
            description = null;
            icon = null;
            canUse = false;

        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click Item");
        
        if (eventData.pointerClick.GetComponentInChildren<WareHouseItem>() != null)
        {
            Debug.Log("Detected Item");
            WareHouseItem thisItem = eventData.pointerClick.GetComponentInChildren<WareHouseItem>();
            if (thisItem.hasItem && thisItem.canUse)
            {
                Debug.Log("can use");
                InventoryManager.Instance.popupConfirmPanel.description.text =  "确认使用"+ "<color=green>" +thisItem.name +"</color>吗?" + thisItem.description;
                    
                InventoryManager.Instance.popupConfirmPanel.gameObject.SetActive(true);
            }
                
        }
    }
}
