using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class Test : ScriptableRendererFeature
{
    // [SerializeField]
    // private Material m;
    // private Vector2 distorCenter;
    // [Range(0,1)][SerializeField]
    // private float distorValue;
    //
    // private void Start()
    // {
    //     distorCenter = new Vector2(0.5f , 0.5f);
    //     if(m==null)
    //         m = new Material(Shader.Find("Autumnind/TraverseShader"));
    // }
    //
    // private void Update()
    // {
    //     
    // }
    //
    // private void OnRenderImage(RenderTexture a , RenderTexture b)
    // {
    //     if (m != null)
    //     {
    //         m.SetFloat("_DistorValue", distorValue);
    //         m.SetVector("_DistorCenter", distorCenter);
    //         
    //         Graphics.Blit(a,b,m);
    //     }
    //     else
    //     {
    //         Graphics.Blit(a,b);
    //     }
    // }
    
    [System.Serializable]
    public class Settings
    {
        public Material material;
        [Range(0, 0.5f)] public float distortionValue = 0.1f;
        public Vector2 distortionCenter = new Vector2(0.5f, 0.5f);
        public RenderPassEvent renderEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public Settings settings = new Settings();
    private TraverseRenderPass renderPass;

    public override void Create()
    {
        if (settings.material == null)
        {
            Debug.LogWarning("Traverse效果缺少材质");
            return;
        }
        
        renderPass = new TraverseRenderPass(settings.material)
        {
            renderPassEvent = settings.renderEvent
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.material == null) return;
        
        renderPass.SetDistortionParams(settings.distortionValue, settings.distortionCenter);
        renderPass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(renderPass);
    }

    class TraverseRenderPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;
        private float distortionValue;
        private Vector2 distortionCenter;

        public TraverseRenderPass(Material material)
        {
            this.material = material;
            tempTexture.Init("_TempTraverseTexture");
        }

        public void SetDistortionParams(float value, Vector2 center)
        {
            distortionValue = value;
            distortionCenter = center;
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("TraverseEffect");
            
            material.SetFloat("_DistorValue", distortionValue);
            material.SetVector("_DistorCenter", new Vector4(distortionCenter.x, distortionCenter.y, 0, 0));
            
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;
            
            cmd.GetTemporaryRT(tempTexture.id, descriptor, FilterMode.Bilinear);
            Blit(cmd, source, tempTexture.Identifier(), material);
            Blit(cmd, tempTexture.Identifier(), source);
            cmd.ReleaseTemporaryRT(tempTexture.id);
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
    
}
