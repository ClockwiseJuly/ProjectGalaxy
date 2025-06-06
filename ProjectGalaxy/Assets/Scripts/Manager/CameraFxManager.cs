using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class CameraFxManager : Singleton<CameraFxManager>
{
    
    [Header("UI References")]
    public RectTransform targetCanvas; // 拖入需要震动的Canvas
    public RectTransform introCanvas; 
    
    [Header("Default Settings")]
    public float defaultDuration = 0.5f;
    public float defaultStrength = 50f; 
    public int defaultVibrato = 20;
    public bool defaultFadeOut = true;
    
    [Header("Continuous Shake")]
    public float continuousShakeInterval = 0.1f;
    public float continuousShakeStrength = 30f;
    
    private Vector2 originalAnchoredPos;
    private Tween continuousShakeTween;
    private bool isShakingContinuously;

    protected override void Awake()
    {
        base.Awake(); 
        if (targetCanvas != null)
        {
            originalAnchoredPos = targetCanvas.anchoredPosition;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartContinuousShake();
        }
    }

    private void OnDisable() => StopShake();
    private void OnDestroy() => StopShake();

    public void Shake(
        float duration = -1f,
        float strength = -1f,
        int vibrato = -1,
        bool fadeOut = true)
    {
        if (targetCanvas == null)
        {
            Debug.LogError("Target Canvas未分配！", this);
            return;
        }
        
        StopContinuousShake(); // 停止持续抖动
    
        duration = duration < 0 ? defaultDuration : duration;
        strength = strength < 0 ? defaultStrength : strength;
        vibrato = vibrato < 0 ? defaultVibrato : vibrato;
    
        targetCanvas.DOShakeAnchorPos(
            duration: duration,
            strength: strength,
            vibrato: vibrato,
            randomness: 90,
            fadeOut: fadeOut
        ).OnComplete(ResetPosition);
    }

    public void IntroShake(float duration = -1f,
        float strength = -1f,
        int vibrato = -1,
        bool fadeOut = true)
    {
        if (introCanvas == null)
        {
            Debug.LogError("Target Canvas未分配！", this);
            return;
        }
        
        StopContinuousShake(); // 停止持续抖动
    
        duration = duration < 0 ? defaultDuration : duration;
        strength = strength < 0 ? defaultStrength : strength;
        vibrato = vibrato < 0 ? defaultVibrato : vibrato;
    
        introCanvas.DOShakeAnchorPos(
            duration: duration,
            strength: strength,
            vibrato: vibrato,
            randomness: 90,
            fadeOut: fadeOut
        ).OnComplete(ResetPosition);
    }

    //持续抖动
    public void StartContinuousShake(float strength = -1f)
    {
        if (targetCanvas == null || isShakingContinuously) return;
        
        isShakingContinuously = true;
        strength = strength < 0 ? continuousShakeStrength : strength;
        
        continuousShakeTween = DOTween.Sequence()
            .Append(targetCanvas.DOShakeAnchorPos(
                duration: continuousShakeInterval,
                strength: strength,
                vibrato: defaultVibrato,
                randomness: 90,
                fadeOut: true
            ))
            .AppendCallback(ResetPosition)
            .SetLoops(-1) // 无限循环
            .SetLink(gameObject) // 绑定对象生命周期
            .Play();
    }

    //停止所有抖动
    public void StopShake()
    {
        StopContinuousShake();
        targetCanvas.DOKill(); // 停止所有动画
        ResetPosition();
    }

    public void StopContinuousShake()
    {
        if (!isShakingContinuously) return;
        
        continuousShakeTween?.Kill();
        isShakingContinuously = false;
    }


    private void ResetPosition()
    {
        if (targetCanvas != null)
            targetCanvas.anchoredPosition = originalAnchoredPos;
    }
    
    public void LightShake() => Shake(0.3f, 30f);
    public void StrongShake() => Shake(1f, 100f);
    public void LongShake() => Shake(10000f, 40f);
}
