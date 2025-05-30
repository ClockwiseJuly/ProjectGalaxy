//Shader "Autumnind/TraverseShader"
//{
//    Properties
//    {
//        _MainTex ("MainTex" , 2D) = "white" {}
//        _MainColor ("MainColor" , Color) = (1,1,1,1)
//    }
//    SubShader
//    {
//        Pass
//        {
//            CGPROGRAM
//            #include "UnityCG.cginc"
//            #pragma vertex vert
//            #pragma fragment frag
//
//            uniform sampler2D _MainTex;
//            uniform float4 _MainColor;
//            uniform float _DistorValue;
//            uniform float2 _DistorCenter;
//
//            struct a2v
//            {
//                float4 pos : POSITION;
//                float2 uv : TEXCOORD0;
//                
//                
//            };
//
//            struct v2f
//            {
//                float4 sv_pos : SV_POSITION;
//                float2 sv_uv : TEXCOORD0;
//            };
//
//            v2f vert (a2v i)
//            {
//                v2f o;
//                o.sv_pos = UnityObjectToClipPos(i.pos);
//                o.sv_uv = i.uv;
//
//                return o;
//                
//            }
//
//            float4 frag(v2f u): SV_TARGET
//            {
//                //计算偏移的方向
//                float2 dir = u.sv_uv - _DistorCenter.xy;
//                //偏移方向  方向*(1-长度) 越在边缘偏移越小
//                float2 offset = _DistorValue * normalize(dir) * (1 - length(dir));
//
//                float4 tex = tex2D(_MainTex , u.sv_uv + offset);
//                return tex;
//            }
//            
//            
//        
//            ENDCG 
//            
//        }
//        
//        
//    }
//    
//}
Shader "Autumnind/TraverseShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _DistorValue ("Distortion Value", Range(0, 0.5)) = 0.1
        _DistorCenter ("Distortion Center", Vector) = (0.5,0.5,0,0)
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
        }
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float4 _MainColor;
            float _DistorValue;
            float2 _DistorCenter;
        CBUFFER_END
        
        struct Attributes
        {
            float4 positionOS : POSITION;
            float2 uv : TEXCOORD0;
        };
        
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float2 uv : TEXCOORD0;
        };
        ENDHLSL
        
        Pass
        {
            Name "TraversePass"
            Tags { "LightMode" = "UniversalForward" }
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                // 计算偏移方向
                float2 dir = input.uv - _DistorCenter;
                // 偏移计算：方向 * (1-长度) 边缘偏移减小
                float2 offset = _DistorValue * normalize(dir) * (1 - length(dir));
                
                // 应用偏移采样纹理
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv + offset);
                return tex * _MainColor;
            }
            ENDHLSL
        }
    }
}