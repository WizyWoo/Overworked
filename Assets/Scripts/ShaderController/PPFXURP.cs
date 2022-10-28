using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PPFXURP : ScriptableRenderPass
{

    public Material ScreenEffect;
    CommandBuffer cmdBuffer;

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {

        Blit(cmdBuffer, ref renderingData, ScreenEffect);

    }

}
