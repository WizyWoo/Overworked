using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPFXRendererFeature : ScriptableRendererFeature
{

    class ScreenPass : ScriptableRenderPass
    {
        private Material _material;
        private Mesh _mesh;

        public ScreenPass(Material material, Mesh mesh)
        {
            _material = material;
            _mesh = mesh;
        }

        public override void Execute(ScriptableRenderContext context,
            ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(name: "ScreenPass");
            cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, _material);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    private ScreenPass screenPass;
    public Material ScreenMat;
    public Mesh mesh;

    public override void Create()
    {
        screenPass = new ScreenPass(ScreenMat, mesh);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer,
        ref RenderingData renderingData)
    {
        if (ScreenMat != null && mesh != null)
        {
            renderer.EnqueuePass(screenPass);
        }
    }

}