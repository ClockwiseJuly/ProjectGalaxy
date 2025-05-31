using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    public bool onTraverse = false; //玩家是否在虫洞
    public GameObject planetBackground;
    public GameObject wormholeBackground;
    public GameObject planet;
    
    protected override void Start()
    {
        base.Start();

        GameEvent.OnTraverse += HandleOnTraverse; //订阅事件
    }


    private void HandleOnTraverse()
    {
        Debug.Log("GameManager : 处理跃迁");
        HandleOnWormhole(true);


    }

    private void HandleOnWormhole(bool isOnWormhole)
    {
        onTraverse = isOnWormhole;
        
        if (onTraverse)
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
    
    
    
    
}
