using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameEvent : MonoBehaviour
{
    //静态委托,跃迁中触发
    public static Action OnTraverse;
    public static Action OnTraverseCompleted;
    public static Action<Image> OnFinishSelectingPlanet;
}
