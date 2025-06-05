using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EffectManager : MonoBehaviour
{
    [Header("(shock wave)")]
    [SerializeField] private float shockWaveTime = 0.75f;
    [SerializeField] private float traverseTime = 1f;
    private Coroutine shockWaveCoroutine;
    private Coroutine traverseCoroutine;
    private Coroutine traverseCompletedMaterial;
    [SerializeField] private Material shockWaveMaterial;
    [SerializeField] private Material traverseMaterial;
    
    
    private static int waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFormCenter");
    private static int traverseLambda = Shader.PropertyToID("_traverseFx");
    [SerializeField]private int shockWaveIndex = 1;
    [SerializeField]private int traverseIndex = 3;
    
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

        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     CallSpaceJump();
        // }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TraverseParticle();
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CallTraverseCompleted();
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

    public void CallTraverseCompleted()
    {
        traverseCompletedMaterial = StartCoroutine(TraverseCompletedAction(1f, 6f));
    }

    // public void CallSpaceJump()
    // {
    //     spaceJumpCoroutine = StartCoroutine(ShockWaveThenTraverseAction(-0.1f,1f,1f,6f));
    // }

    private IEnumerator ShockWaveAction(float startPos , float endPos)
    {
        Debug.Log("调用冲击波协程");
        
        CameraFxManager.Instance.Shake();
        
        AudioManager.Instance.PlaySFX(shockWaveIndex);
        CameraFxManager.Instance.Shake();
        
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
        
        
        
        shockWaveMaterial.SetFloat(waveDistanceFromCenter, -0.1f);

    }

    private IEnumerator TraverseAction(float startPos, float endPos)
    {
        Debug.Log("调用穿越协程");
        //AudioManager.Instance.PlaySFX(traverseIndex);
        
        CameraFxManager.Instance.Shake();
        
        CallShockWave();

        yield return new WaitForSeconds(shockWaveTime);
    
        traverseMaterial.SetFloat(traverseLambda, startPos);
    
        float elapsedTime = 0f;
        float recoverTime = 0.3f; // 默认回退时间 = 前进时间
    
        // 从 startPos 到 endPos
        while (elapsedTime < traverseTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime / traverseTime);
            traverseMaterial.SetFloat(traverseLambda, lerpedAmount);
            yield return null;
        }
    
        // 确保最终到达 endPos
        traverseMaterial.SetFloat(traverseLambda, endPos);
        
        yield return new WaitForSeconds(1f);
        TraverseParticle();
        GameEvent.OnTraverse?.Invoke(); //调用事件
        AudioManager.Instance.PlaySFX(1);
        CameraFxManager.Instance.Shake();
        
        elapsedTime = 0f; // 重置计时器
    
        // 从 endPos 回到 startPos
        while (elapsedTime < recoverTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedAmount = Mathf.Lerp(endPos, startPos, elapsedTime / recoverTime);
            traverseMaterial.SetFloat(traverseLambda, lerpedAmount);
            yield return null;
        }
    
        // 确保最终回到 startPos
        traverseMaterial.SetFloat(traverseLambda, startPos);
    }

    private IEnumerator TraverseCompletedAction(float startPos, float endPos)
    {
        Debug.Log("调用穿越完成协程");
        
        CallShockWave();

        yield return new WaitForSeconds(shockWaveTime);
    
        traverseMaterial.SetFloat(traverseLambda, startPos);
    
        float elapsedTime = 0f;
        float recoverTime = 0.3f; // 默认回退时间 = 前进时间
    
        // 从 startPos 到 endPos
        while (elapsedTime < traverseTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime / traverseTime);
            traverseMaterial.SetFloat(traverseLambda, lerpedAmount);
            yield return null;
        }
    
        // 确保最终到达 endPos
        traverseMaterial.SetFloat(traverseLambda, endPos);
        
        yield return new WaitForSeconds(1f);
        TraverseParticle();
        GameEvent.OnTraverseCompleted?.Invoke(); //调用事件
        AudioManager.Instance.PlaySFX(1);
        CameraFxManager.Instance.Shake();
        
        elapsedTime = 0f; // 重置计时器
    
        // 从 endPos 回到 startPos
        while (elapsedTime < recoverTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedAmount = Mathf.Lerp(endPos, startPos, elapsedTime / recoverTime);
            traverseMaterial.SetFloat(traverseLambda, lerpedAmount);
            yield return null;
        }
    
        // 确保最终回到 startPos
        traverseMaterial.SetFloat(traverseLambda, startPos);
    }
    

    
    // private IEnumerator ShockWaveThenTraverseAction(float shockStart, float shockEnd, float traverseStart, float traverseEnd)
    // {
    //     Debug.Log("调用跃迁协程");
    //     //AudioManager.Instance.PlayExtraSFX(0);
    //     
    //     yield return new WaitForSeconds(4f);
    //     
    //     // 先执行ShockWaveAction
    //     yield return ShockWaveAction(shockStart, shockEnd);
    //     //AudioManager.Instance.PlaySFX(shockWaveIndex);
    //     
    //     // ShockWaveAction完成后执行TraverseAction
    //     yield return TraverseAction(traverseStart, traverseEnd);
    //     //AudioManager.Instance.PlaySFX(traverseIndex);
    // }

    public GameObject particlePrefab;
    public Canvas canvas;
    
    public void TraverseParticle()
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

        //粒子播放完成后自动销毁
        if (particleSystem != null)
            Destroy(particleInstance, particleSystem.main.duration);
    }


}
