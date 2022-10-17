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
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            Float Multiplier;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float2 pos = float2(abs(sin(i.uv.x + (Multiplier * _Time[1]))), abs(sin(i.uv.y + (Multiplier * _Time[1]))));
                fixed4 col = tex2D(_MainTex, pos);
                col.r = abs(sin(_Time[1] + col.r));
                col.g = abs(sin(_Time[1] + 1 + col.g));
                col.b = abs(sin(_Time[1] + 2 + col.b));
                col.a = tex2D(AlphaMapTex, pos).a;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
