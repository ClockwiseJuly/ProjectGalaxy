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
            AudioManager.Instance.PlayBGM(1);
            return;
        }
        else if (GameDataManager.Instance.nowSAN <= 0)
        {
            sanDownEnd.SetActive(true);
            UIManager.Instance.Pause();
            GameDataManager.Instance.endTriggered = true;
            AudioManager.Instance.PlayBGM(1);
            return;
        }
        else if (GameDataManager.Instance.nowShipHP <= 0)
        {
            shipHPDownEnd.SetActive(true);
            UIManager.Instance.Pause();
            GameDataManager.Instance.endTriggered = true;
            AudioManager.Instance.PlayBGM(1);
            return;
        }
        else if(GameDataManager.Instance.nowShipFuel <= 0)
        {
            shipFuelDownEnd.SetActive(true);
            UIManager.Instance.Pause();
            GameDataManager.Instance.endTriggered = true;
            AudioManager.Instance.PlayBGM(1);
            return;
        }
        
        
        #endregion

        //真结局
        if (GameDataManager.Instance.hasEngine)
        {
            UIManager.Instance.Pause();
            trueEnd.SetActive(true);
            GameDataManager.Instance.endTriggered = true;
            AudioManager.Instance.PlayBGM(1);
            return;
        }
        
        
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    
}
