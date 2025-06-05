using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderFXController : MonoBehaviour
{
    public RawImage screenImage;
    public float effectDuration = 2.0f;
    
    private Material material;
    private float effectStrength;
    
    void Start()
    {
        material = screenImage.material;
        material = new Material(material); // 创建实例
        screenImage.material = material;
        //StartEffect();
    }
    
    public void StartEffect()
    {
        StartCoroutine(PhaseEffect());
    }
    
    IEnumerator PhaseEffect()
    {
        float timer = 0;
        while (timer < effectDuration)
        {
            timer += Time.deltaTime;
            effectStrength = Mathf.PingPong(timer, effectDuration/2) / (effectDuration/2);
            material.SetFloat("_EffectStrength", effectStrength);
            yield return null;
        }
        material.SetFloat("_EffectStrength", 0);
    }
}
