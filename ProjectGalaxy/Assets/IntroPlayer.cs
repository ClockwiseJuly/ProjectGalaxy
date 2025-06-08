using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Fungus;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class IntroPlayer : MonoBehaviour
{
    [Header("基本设置")] 
    public GameObject introPlayer;
    public Flowchart flowchart;
    public float fadeDuration = 0.5f; // 淡入持续时间
    public float defaultDelay = 0.3f; // 默认图片间延迟
    public float groupDelay = 1f; // 组间延迟
    public bool loop = false; // 是否循环
    public bool canClick = false; // 是否可以点击
    
    
    
    [Header("图片组")]
    public List<ImageGroup> imageGroups = new List<ImageGroup>();

    private Sequence _sequence;

    [System.Serializable]
    public class ImageSetting
    {
        public Image image;
        [Tooltip("负数将使用默认延迟")]
        public float customDelay = -1f;
        public int sfxIndex;
    }

    [System.Serializable]
    public class ImageGroup
    {
        public List<ImageSetting> images;
    }

    private void Start()
    {
        if (!GameDataManager.Instance.gameData.playedIntro)
        {
            introPlayer.SetActive(true);
            Initialize();
            PlaySequence();
        }
        

    }


    private void OnEnable()
    {

        AudioManager.Instance.PlayBGM(1);


    }


    private void Initialize()
    {
        // 初始化所有图片为透明
        foreach (var group in imageGroups)
        {
            if (group == null || group.images == null) continue;
            
            foreach (var setting in group.images)
            {
                if (setting != null && setting.image != null)
                {
                    Color c = setting.image.color;
                    c.a = 0;
                    setting.image.color = c;
                }
            }
        }
    }

    public void PlaySequence()
    {
        
        
        // 清理现有序列
        if (_sequence != null && _sequence.IsActive())
        {
            _sequence.Kill();
        }
        
        AudioManager.Instance.PlayBGM(1);

        _sequence = DOTween.Sequence();
        
        // 遍历所有图片组
        for (int i = 0; i < imageGroups.Count; i++)
        {
            var group = imageGroups[i];
            if (group == null || group.images == null) continue;
            
            // 添加组间延迟（第一组除外）
            if (i > 0)
            {
                _sequence.AppendInterval(groupDelay);
            }
            
            // 遍历组内图片
            for (int j = 0; j < group.images.Count; j++)
            {
                var setting = group.images[j];
                if (setting == null || setting.image == null) continue;
                
                // 添加图片间延迟（第一张图片除外）
                if (j > 0)
                {
                    float delay = setting.customDelay >= 0 ? setting.customDelay : defaultDelay;
                    _sequence.AppendInterval(delay);
                }
                
                // 添加淡入动画
                _sequence.Append(setting.image.DOFade(1f, fadeDuration));
                
                if (setting.sfxIndex > 0)
                {
                    
                    _sequence.AppendCallback(() => {
                        AudioManager.Instance.PlaySFX(setting.sfxIndex);
                        if (setting.sfxIndex == 5)
                        {
                            CameraFxManager.Instance.IntroShake();
                        }
                        
                    });
                }
            }
        }
        
        // 设置循环
        if (loop)
        {
            _sequence.SetLoops(-1, LoopType.Restart);
        }
        
        // 循环时重置状态
        _sequence.OnComplete(() => {
            if (loop) Initialize();
            
            canClick = true;
        });
    }

    public void Pause()
    {
        if (_sequence != null) _sequence.Pause();
    }

    public void Resume()
    {
        if (_sequence != null) _sequence.Play();
    }

    public void Stop()
    {
        if (_sequence != null) _sequence.Kill();
        Initialize();
    }

    private void OnDestroy()
    {
        if (_sequence != null && _sequence.IsActive())
        {
            _sequence.Kill();
        }
    }

    public void OnClick()
    {
        if(!canClick)
            return;
        
        GameDataManager.Instance.gameData.playedIntro = true;
        introPlayer.gameObject.SetActive(false);
        flowchart.ExecuteBlock("T1");
        AudioManager.Instance.PlayBGM(2);
        
    }
}
