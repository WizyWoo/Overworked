Shader "Poggers/MovingSurface"
{
    //float = high precision
    //half = medium precision useful for positions / high dynamic range colors and short vectors, accurate down to 3 decimals
    //fixed = low precision, useful for colors and simple operations, 1/256
    Properties
    {
        
        [NoScaleOffset] _Texture("Texture", 2D) = "white"
        _FlowDirX("Flow direction", float) = 0
        _FlowDirY("Flow direction", float) = 0
        _TileDirX("X tile direction", float) = 0
        _TileDirZ("Y tile direction", float) = 0
        _TexSize("Tiling scale", float) = 1

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
                float3 wPos         : TEXCOORD1;

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
                o.uv = i.uv;
                o.wPos = mul(GetObjectToWorldMatrix(), float4(i.positionOS.xyz, 1.0)).xyz;
                return o;

            }
            
            float _FlowDirX, _FlowDirY, _TexSize, _TileDirX, _TileDirZ;

            half4 frag(v2f IN) : SV_Target
            {

                half4 color = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, float2(IN.uv.x + (IN.wPos.x * _TileDirX) + (_Time[1] * _FlowDirX * _TexSize), 
                IN.uv.y + (IN.wPos.z * _TileDirZ) + (_Time[1] * _FlowDirY * _TexSize)));
                return color;

            }

            ENDHLSL

        }

    }

}