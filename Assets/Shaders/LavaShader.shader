Shader "Poggers/LavaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        AlphaMapTex ("Alpha map texture", 2D) = "white" {}
        Multiplier ("Multiplier for lava pos", Float) = 0
        TimeMult ("Time Reposition Multiplier", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderPipeline" = "UniversalPipeline"}
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D AlphaMapTex;
            float4 _MainTex_ST;
            Float TimeMult;
            float2 pos;
            Float Multiplier;

            v2f vert (appdata _inn)
            {
                v2f o;
                pos = float2(abs(sin(v.uv.x + (Multiplier * _Time[1]))), abs(sin(v.uv.y + (Multiplier * _Time[1]))));
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, pos);
                col.r = abs(sin(_Time[1] + col.r));
                col.g = abs(sin(_Time[1] + 1 + col.g));
                col.b = abs(sin(_Time[1] + 2 + col.b));
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDHLSL
        }
    }
}
