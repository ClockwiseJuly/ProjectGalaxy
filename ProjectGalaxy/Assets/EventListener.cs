using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    [SerializeField] private bool osTaught = false;
    
    public enum EventType
    {
        none,
        OSPanelTutorial,
        closeCraftPanel,
        
    }

    private void Start()
    {
        if (GameDataManager.Instance.gameData.osPaneltutorialtaught)
        {
            osTaught = true;
        }
    }


    public EventType eventType;

    private void OnEnable()
    {
        if (eventType == EventType.OSPanelTutorial)
        {
            if (GameDataManager.Instance.gameData.playedIntro)
            {
                if (!GameDataManager.Instance.gameData.osPaneltutorialtaught && !osTaught)
                {
                    GameManager.Instance.DialogueOSPanelTutorial();
                    
                
                }
            }
        }   

    }
    
    private void OnDisable()
    {
        if (eventType == EventType.OSPanelTutorial)
        {
            GameDataManager.Instance.gameData.osPaneltutorialtaught = true;
            
            if (!osTaught)
            {
                GameManager.Instance.DialogueTraverseTutorial();
                osTaught = true;
            }
        }
        else if (eventType == EventType.closeCraftPanel)
        {
            InventoryManager.Instance.inventory.DisplayItems();
        }
        
        
        
    }
}
