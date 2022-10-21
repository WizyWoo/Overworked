Shader "Poggers/LavaShader"
{
    //float = high precision
    //half = medium precision useful for positions / high dynamic range colors and short vectors, accurate down to 3 decimals
    //fixed = low precision, useful for colors and simple operations, 1/256
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


            struct appdata
            {

                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                
            };

            struct v2f
            {

                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;

            };

            TEXTURE2D(_Texture);
            SAMPLER(sampler_Texture);

            CBUFFER_START(UnityPerMaterial)

                float4 _Texture_ST;

            CBUFFER_END

            v2f vert(appdata i)
            {

                v2f o;
                o.positionHCS = TransformObjectToHClip(i.positionOS.xyz);
                o.uv = float2(i.uv.x + o.positionHCS.x, i.uv.y + o.positionHCS.z);
                return o;

            }
            
            float _FlowDirX;
            float _FlowDirY;

            half4 frag(v2f IN) : SV_Target
            {

                half4 color = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, float2(IN.uv.x + (_Time[1] * _FlowDirX), IN.uv.y + (_Time[1] * _FlowDirY)));
                return color;

            }

            ENDHLSL

        }

    }

}