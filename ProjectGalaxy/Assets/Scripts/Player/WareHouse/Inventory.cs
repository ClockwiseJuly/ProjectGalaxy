using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public GameData gameData;
    public ItemUseConfirm popupConfirmPanel;


    public GameObject[] slots;
    

    private void Start()
    {
        //DisplayItems();
    }

    private void OnEnable()
    {
        DisplayItems();
    }

    private void OnDisable()
    {
        //ClearItems();
        popupConfirmPanel.gameObject.SetActive(false);
        
    }



    public void DisplayItems()
    {
        int j = 0;
        for (int i = 0; i < gameData.warehouse.Count; i++)
        {
            if (gameData.warehouse[i].amount > 0)
            {
                slots[j].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                slots[j].transform.GetChild(0).GetComponent<Image>().sprite = gameData.warehouse[i].icon;
                slots[j].transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
                slots[j].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gameData.warehouse[i].amount.ToString();
                
                slots[i].GetComponent<WareHouseItem>().hasItem = true;
                j++;
            }
        }
        
        Debug.Log($"仓库已打开，有{j}种物品");
    }

    public void ClearItems()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].GetComponent<WareHouseItem>().hasItem = false;
            slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = gameData.warehouse[i].icon;
            slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0f);
            slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 0.ToString();
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
        
    }
}

