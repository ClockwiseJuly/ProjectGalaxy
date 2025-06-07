using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;
using DG.Tweening;
using TMPro;


public class GameManager : Singleton<GameManager>//,IPointerClickHandler
{
    public bool playerOnWormHole = false; //玩家是否在虫洞
    public GameObject planetBackground;
    public GameObject wormholeBackground;
    public GameObject planet;
    public Image nowPlanetImage;
    public Canvas uiCanvas;
    
    [Header("===== 选择星球 =====")]
    public GameObject selectedPlanetPanel;
    
    [Header("===== Fungus =====")]
    public Flowchart flowchart;
    
    [Header("===== 收集资源 =====")]
    public CollectResources collectResources;

    


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
        EffectManager.Instance.CallTraverseCompleted();
        
        nowPlanetImage.sprite = img.sprite;
        collectResources.nowPlanet.sprite = img.sprite;

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

    public void StartCollect()
    {
        if (playerOnWormHole)
        {
            
            PopupText("在时空隧道中无法收集资源",0);
            return;
        }

        
        collectResources.StartResourceGame();
    }

    public void CallTraverseCompleted()
    {
        if (!playerOnWormHole)
        {
            PopupText("暂时无法跃迁",1);
            return;
        }
        
        UIManager.Instance.ShowStarSelectCanvas();
        
    }
    
    
    
    [Header("===== 上升文字 =====")]
    public GameObject risingTextPrefab; // 上升文字的预制体
    public float riseHeight = 100f;     
    public float animationDuration = 1f; // 动画持续时间
    public string displayText = "new";   
    public Ease easeType = Ease.OutQuad; // 动画缓动类型 

    public Button collectBtn;  
    public Button traverseBtn; 
    
    private void PopupText(string _text,int _index = 0)
    {
        Debug.Log("上升文字");
        
        // 实例化文本
        GameObject textObj = null;
        if (_index == 0)
        {
            textObj  = Instantiate(risingTextPrefab, collectBtn.transform.position, Quaternion.identity,uiCanvas.transform);
            
        }
        else if(_index == 1)
        {
            textObj = Instantiate(risingTextPrefab, traverseBtn.transform.position, Quaternion.identity,uiCanvas.transform);
        }
        
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

    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     Debug.Log("GameManager : OnPointerClick");
    //     
    //     if (eventData.pointerPress != null)
    //     {
    //         if (eventData.pointerPress.gameObject.name == "采集")
    //         {
    //             Debug.Log("点击采集按钮");
    //             
    //             if(!playerOnWormHole)
    //                 StartCollect();
    //             else
    //             {
    //                 
    //             }
    //                 
    //             
    //         }
    //         else if(eventData.pointerPress.gameObject.name == "确认跃迁")
    //         {
    //             Debug.Log("点击跃迁按钮");
    //             
    //             if (playerOnWormHole)
    //             {
    //                 
    //             }
    //                 
    //             
    //         }
    //     }
    // }
}
