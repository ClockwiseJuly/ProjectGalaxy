using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveManager : MonoBehaviour
{
    [Header("(按下空格测试)")]
    [SerializeField] private float shockWaveTime = 0.75f;
    private Coroutine shockWaveCoroutine;
    [SerializeField] private Material shockWaveMaterial;
    
    private static int waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFormCenter");
    private int shockWaveIndex = 1;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CallShockWave();
        }
    }

    public void CallShockWave()
    {
        shockWaveCoroutine = StartCoroutine(ShockWaveAction(-0.1f, 1f));
        AudioManager.Instance.PlaySFX(shockWaveIndex);
    }

    private IEnumerator ShockWaveAction(float startPos , float endPos)
    {
        shockWaveMaterial.SetFloat(waveDistanceFromCenter, startPos);

        float lerpedAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < shockWaveTime)
        {
            elapsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime / shockWaveTime);
            shockWaveMaterial.SetFloat(waveDistanceFromCenter, lerpedAmount);

            yield return null;
        }

    }
}
