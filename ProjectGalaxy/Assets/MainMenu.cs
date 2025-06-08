using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameData gameData;
    public GameData initialGameData;
    
    public Button startBtn;
    public Button exitBtn;
    public Button tutorialBtn;
    
    public CanvasGroup mainMenuCanvas;
    public GameObject mainMenuPanel;
    public GameObject loadPanel;
    public Animator fadePanelAnim;
    public Slider loadSlider;
    public TextMeshProUGUI loadingPercentageText;
    public GameObject loadpanel;
    private float currentValue = 0f;
    [SerializeField] private float currentValueSpeed = 0.5f;

    public GameObject toturialPanel;

    private void OnEnable()
    {
        AudioManager.Instance.PlayBGM(0);
    }

    private void Start()
    {
        
        Screen.SetResolution(1920,1080,true);
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(1);
        gameData.playedIntro =false;
        gameData.osPaneltutorialtaught  = false;
        StartCoroutine(LoadLevel());
        
    }

    public void OpenTutorial()
    {
        
    }

    public void ExitGame()
    {
        
    }

    private IEnumerator LoadLevel()
    {
        mainMenuCanvas.alpha = 0;
        mainMenuCanvas.interactable = false;
        
        loadPanel.SetActive(true);
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        
        operation.allowSceneActivation = false;

        //fadePanelAnim.speed = 0;

        while (!operation.isDone)
        {
            currentValue = Mathf.Clamp(currentValue + currentValueSpeed * Time.deltaTime, 0f, 1f);
            
            loadSlider.value = currentValue;
            
            //loadSlider.value = operation.progress;
            //loadingPercentageText.text = operation.progress *100 + "%";
            
            

            if (operation.progress >= 0.9f && currentValue >= 1f)
            {
                loadSlider.value = 1;
                //fadePanelAnim.speed = 1;
                
                loadingPercentageText.text = "按下任意键以继续";

                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }
            
            yield return null;
        }
        
    }

    public void OpenURL()
    {
        Application.OpenURL("https://www.bilibili.com/video/BV1d9TXzrEF1/?spm_id_from=333.1387.homepage.video_card.click&vd_source=fd03c6736f0154bafdb47a9facf6e4d9");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
