Shader "Effect/UI/BloodPool"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Percent("BloodPercent", Range(0.0, 1.0)) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Lighting Off
		ZWrite Off

/*		Pass
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
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Percent;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				return half4(i.uv.x < _Percent ? 0.8f : 0.0f, 0.0f, 0.0f, 1.0f);
			}
			ENDCG
		}
*/
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _Percent;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb = lerp(fixed3(i.uv.x < _Percent ? 0.8f : 0.0f, 0.0, 0.0), col.rgb*1.5, col.a);
				col.a = 1.0f;
				//col.rgb *= col.a;
                return clamp(col, 0.0, 1.0);
            }
            ENDCG
        }
    }
}
