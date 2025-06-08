using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataManager : Singleton<GameDataManager>
{
    public GameData gameData;
    
    [Header("===== 状态 =====")]
    public int maxHP = 100;
    public int nowHP = 100;
    public int maxSAN =100;
    public int nowSAN = 100;
    public int maxShipHP = 100;
    public int nowShipHP = 100;
    public int maxShipFuel = 100;
    public int nowShipFuel = 100;
    public Image HPSlider;
    public Image SANSlider;
    public Image shipHPImg;
    public Image shipFuelImg;

    public bool hasEngine = false;
    public bool endTriggered = false;
    

    protected override void Awake()
    {
        base.Awake();
        
    }

    protected override void Start()
    {
        base.Start();
        
    }

    private void Update()
    {
        HPSlider.fillAmount = nowHP/(float)maxHP;
        SANSlider.fillAmount = nowSAN/(float)maxSAN;
    }

    public int ChangeHP(int value)
    {
        
        return nowHP += value;
    }

    public int ChangeSAN(int value)
    {
        return nowSAN += value;
    }

    public int ChangeShipHP(int value)
    {
        return nowShipHP += value;
    }

    public int ChangeShipFuel(int value)
    {
        return nowShipFuel += value;
    }

    public void UpdateShipHPAndFuel()
    {
        shipHPImg.fillAmount = nowShipHP / (float)maxShipHP;
        shipFuelImg.fillAmount = nowShipFuel / (float)maxShipFuel;
    }


    public void HasEngine()
    {
        hasEngine = true;
    }
}
