using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlanetState : MonoBehaviour
{
    public enum PlanetType
    {
        None,
        TerranWet, //类地湿润行星
        TerranDry, //类地干燥行星
        Islands, //群岛行星
        NoAtmosphere, //无大气行星
        GasGiant1, //气态巨行星
        GasGiant2, //气态巨行星（带光环）
        Ice, //冰冻行星
        Lava, //熔岩行星
        Asteroid, //小行星
        BlackHole, //黑洞
        Galaxy, //星系
        Star, //恒星
    }
    
    public enum PlanetCamp
    {
        None,
        Humanoid,  //类人
        AI,        //智械
        Zerg,      //虫族
        OuterGod   //古神
    }
    
    public PlanetType planetType;
    public PlanetCamp planetCamp;
}
