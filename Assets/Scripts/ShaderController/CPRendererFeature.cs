using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

internal class CPRendererFeature : ScriptableRendererFeature
{
    public Shader m_Shader;
    public float m_Intensity;

    public Material m_Material;

    CBPass m_RenderPass = null;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            //Calling ConfigureInput with the ScriptableRenderPassInput.Color argument ensures that the opaque texture is available to the Render Pass
            m_RenderPass.ConfigureInput(ScriptableRenderPassInput.Color);
            m_RenderPass.SetTarget(renderer.cameraColorTarget, m_Intensity);
            renderer.EnqueuePass(m_RenderPass);
        }
    }

    public override void Create()
    {
        /*if (m_Shader != null)
            m_Material = new Material(m_Shader);*/

        m_RenderPass = new CBPass(m_Material);
    }

    protected override void Dispose(bool disposing)
    {
        //CoreUtils.Destroy(m_Material);
    }
}