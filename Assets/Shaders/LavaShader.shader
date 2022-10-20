Shader "Poggers/LavaShader"
{
    Properties
    {
        
        [MainTexture] _Texture("Texture", 2D) = "white"
        _FlowDirX("Flow direction", float) = 0
        _FlowDirY("Flow direction", float) = 0

    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {

                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                
            };

            struct Varyings
            {

                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;

            };

            TEXTURE2D(_Texture);
            SAMPLER(sampler_Texture);

            CBUFFER_START(UnityPerMaterial)

                float4 _Texture_ST;

            CBUFFER_END

            Varyings vert(Attributes IN)
            {

                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _Texture);
                return OUT;

            }
            
            float _FlowDirX;
            float _FlowDirY;

            half4 frag(Varyings IN) : SV_Target
            {

                half4 color = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, float2(IN.uv.x + (_Time[1] * _FlowDirX), IN.uv.y + (_Time[1] * _FlowDirY)));
                return color;

            }

            ENDHLSL

        }

    }

}