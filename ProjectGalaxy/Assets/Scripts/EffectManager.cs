using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectManager : MonoBehaviour
{
    [Header("(按下空格测试)")]
    [SerializeField] private float shockWaveTime = 0.75f;
    [SerializeField] private float traverseTime = 1f;
    private Coroutine shockWaveCoroutine;
    private Coroutine traverseCoroutine;
    private Coroutine spaceJumpCoroutine;
    [SerializeField] private Material shockWaveMaterial;
    [SerializeField] private Material traverseMaterial;
    
    private static int waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFormCenter");
    private static int traverseLambda = Shader.PropertyToID("_traverseFx");
    private int shockWaveIndex = 1;
    private int traverseIndex = 3;
    
    [Header("===== 特效 =====")]
    public ParticleSystem traverseParticle;

    private void Awake()
    {
        
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //CallShockWave();
            CallTraverse();
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            CallShockWave();
            
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            CallSpaceJump();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayParticle();
        }
    }

    public void CallShockWave()
    {
        shockWaveCoroutine = StartCoroutine(ShockWaveAction(-0.1f, 1f));
        
    }

    public void CallTraverse()
    {
        traverseCoroutine = StartCoroutine(TraverseAction(1f, 6f));
    }

    public void CallSpaceJump()
    {
        spaceJumpCoroutine = StartCoroutine(ShockWaveThenTraverseAction(-0.1f,1f,1f,6f));
    }

    private IEnumerator ShockWaveAction(float startPos , float endPos)
    {
        Debug.Log("调用冲击波协程");
        
        AudioManager.Instance.PlaySFX(shockWaveIndex);
        shockWaveMaterial.SetFloat(waveDistanceFromCenter, startPos);
        //Debug.Log("waveDistanceFromCenter is " + waveDistanceFromCenter);

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

    private IEnumerator TraverseAction(float startPos, float endPos)
    {
        Debug.Log("调用穿越协程");
        //AudioManager.Instance.PlaySFX(traverseIndex);
        
        traverseMaterial.SetFloat(traverseLambda, startPos);
        //Debug.Log("traverseLambda is " + traverseLambda);
        
        float lerpedAmount = 0f;
        float elapsedTime = 0f;
        
        while (elapsedTime < traverseTime)
        {
            elapsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime / shockWaveTime);
            traverseMaterial.SetFloat(traverseLambda, lerpedAmount);
            
            yield return null;
            
        }
    }
    
    private IEnumerator ShockWaveThenTraverseAction(float shockStart, float shockEnd, float traverseStart, float traverseEnd)
    {
        Debug.Log("调用跃迁协程");
        //AudioManager.Instance.PlayExtraSFX(0);
        
        yield return new WaitForSeconds(4f);
        
        // 先执行ShockWaveAction
        yield return ShockWaveAction(shockStart, shockEnd);
        //AudioManager.Instance.PlaySFX(shockWaveIndex);
        
        // ShockWaveAction完成后执行TraverseAction
        yield return TraverseAction(traverseStart, traverseEnd);
        //AudioManager.Instance.PlaySFX(traverseIndex);
    }

    public GameObject particlePrefab;
    public Canvas canvas;
    
    public void PlayParticle()
    {


        //例化粒子预制体
        GameObject particleInstance = Instantiate(particlePrefab, canvas.transform);

        //置粒子位置为屏幕正中央（UI坐标系）
        RectTransform rect = particleInstance.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero; // 关键代码：居中

        //播放粒子系统
        ParticleSystem particleSystem = particleInstance.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
        else
        {
            Debug.LogWarning("No ParticleSystem found on prefab!");
        }

        // 5. 可选：粒子播放完成后自动销毁
        if (particleSystem != null)
            Destroy(particleInstance, particleSystem.main.duration);
    }


}
