using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ImageShake : MonoBehaviour
{
    public float amount = 10f;
    public float duration = 0.5f;
    
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 pos = rt.anchoredPosition;
        
        rt.DOAnchorPosX(pos.x - amount, duration/2)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
    
    void OnDestroy() => GetComponent<RectTransform>().DOKill();
}
