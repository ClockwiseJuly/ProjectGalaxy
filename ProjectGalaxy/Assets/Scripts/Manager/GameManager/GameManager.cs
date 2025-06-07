using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;



public class GameManager : Singleton<GameManager>
{
    [FormerlySerializedAs("onTraverse")] public bool playerOnWormHole = false; //玩家是否在虫洞
    public GameObject planetBackground;
    public GameObject wormholeBackground;
    public GameObject planet;
    public Image nowPlanetImage;
    
    [Header("===== 选择星球 =====")]
    public GameObject selectedPlanetPanel;
    
    [Header("===== Fungus =====")]
    public Flowchart flowchart;
    
    protected override void Start()
    {
        base.Start();

        
        //订阅事件
        GameEvent.OnTraverse += HandleOnTraverse; 
        GameEvent.OnTraverseCompleted += HandleOnTraverseCompleted; 
        GameEvent.OnFinishSelectingPlanet += HandleOnFinishSelectingPlanet; 
    }

    private void Update()
    {
        if (wormholeBackground.activeInHierarchy)
        {
            playerOnWormHole = true;
        }
        else
        {
            playerOnWormHole = false;
        }
        
        
    }


    //跃迁中调用
    private void HandleOnTraverse()
    {
        Debug.Log("GameManager : 跃迁");
        HandleOnWormhole(true);

        UIManager.Instance.TogglePause(true);
    }

    //跃迁完成调用
    private void HandleOnTraverseCompleted()
    {
        Debug.Log("GameManager : 跃迁完成");
        HandleOnWormhole(false);
        
        
        //UIManager.Instance.TogglePause(false);
        
        
    }

    private void HandleOnWormhole(bool _isOnWormhole)
    {
        playerOnWormHole = _isOnWormhole;
        
        if (playerOnWormHole)
        {
            planetBackground.SetActive(false);
            planet.SetActive(false);
            wormholeBackground.SetActive(true);
        }
        else
        {
            planetBackground.SetActive(true);
            planet.SetActive(true);
            wormholeBackground.SetActive(false);
        }
        

    }

    private void HandleOnFinishSelectingPlanet(Image img)
    {
        Debug.Log("GameManager: 已选择星球");
        nowPlanetImage.sprite = img.sprite;

    }

    public void SeletcPlanet()
    {
        
    }

    
    

    public void DialogueOSPanelTutorial()
    {
        flowchart.ExecuteBlock("T2");
    }
    
    public void DialogueTraverseTutorial()
    {
        flowchart.ExecuteBlock("T3");
    }
    
}
