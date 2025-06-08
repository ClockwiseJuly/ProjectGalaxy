using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public GameObject hpDownEnd;
    public GameObject sanDownEnd;
    public GameObject shipHPDownEnd;
    public GameObject shipFuelDownEnd;
    public GameObject trueEnd;
    

    public void EndCheck()
    {
        #region BadEndCheck

        if (GameDataManager.Instance.nowHP <= 0)
        {
            hpDownEnd.SetActive(true);
            UIManager.Instance.Pause();
            GameDataManager.Instance.endTriggered = true;
            return;
        }
        else if (GameDataManager.Instance.nowSAN <= 0)
        {
            sanDownEnd.SetActive(true);
            UIManager.Instance.Pause();
            GameDataManager.Instance.endTriggered = true;
            return;
        }
        else if (GameDataManager.Instance.nowShipHP <= 0)
        {
            shipHPDownEnd.SetActive(true);
            UIManager.Instance.Pause();
            GameDataManager.Instance.endTriggered = true;
            return;
        }
        else if(GameDataManager.Instance.nowShipFuel <= 0)
        {
            shipFuelDownEnd.SetActive(true);
            UIManager.Instance.Pause();
            GameDataManager.Instance.endTriggered = true;
            return;
        }
        
        
        #endregion

        //真结局
        if (GameDataManager.Instance.hasEngine)
        {
            UIManager.Instance.Pause();
            trueEnd.SetActive(true);
            GameDataManager.Instance.endTriggered = true;
            return;
        }
        
        
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    
}
