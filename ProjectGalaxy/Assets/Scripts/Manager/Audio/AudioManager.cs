using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    [FormerlySerializedAs("sfx1")]
    [Header("===== 音效 =====")]
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] sfxExtra;
    [Header("===== 背景音乐 =====")]
    [SerializeField] private AudioSource[] bgm;

    public bool playBGM;
    public int bgmIndex;

    protected override void Awake()
    {
        base.Awake();
        
        
    }

    private void OnEnable()
    {
        //check scene index and play bgm
        
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (!playBGM)
            StopAllBGM();


        if (Input.GetKeyDown(KeyCode.J))
            PlaySFX(0);
            
    }

    public void PlaySFX(int _sfxIndex) //播放音效调用这个函数
    {
        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(0.85f, 1.1f); //随机音高
            sfx[_sfxIndex].Play();
        }
    }

    public void PlayExtraSFX(int _sfxIndex)
    {
        if (_sfxIndex < sfxExtra.Length)
        {
            sfxExtra[_sfxIndex].pitch = Random.Range(0.85f, 1.1f); //随机音高
            sfxExtra[_sfxIndex].Play();
        }
    }
    
    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop();

    public void PlayBGM(int _bgmIndex) //播放背景音乐调用这个函数
    {
        StopAllBGM();
        bgm[_bgmIndex].Play();
        
    }
    
    public void PlayRandomBGM() //随机背景音乐
    {
        bgmIndex = Random.Range(0, bgm.Length);
        
    }
    
    public void StopAllSFX() 
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            sfx[i].Stop();
        }
    }
    
    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    public void ExplosionSound()
    {
        PlaySFX(5);
        CameraFxManager.Instance.StartContinuousShake();
    }
}
