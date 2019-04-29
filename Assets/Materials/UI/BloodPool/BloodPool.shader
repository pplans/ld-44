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

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			//float _Percent;
			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float, _Percent)
			UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o); // necessary only if you want to access instanced properties in the fragment Shader.
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				UNITY_SETUP_INSTANCE_ID(i); // necessary only if any instanced properties are going to be accessed in the fragment Shader.
				
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				// blood ratio colors : float3(0.44f, 0.12f, 0.12)
				col.rgb = lerp((i.uv.x < UNITY_ACCESS_INSTANCED_PROP(Props, _Percent) ? float3(0.88, 0.24, 0.24)*2.0:float3(0.0, 0.0, 0.0)), col.rgb*1.0, col.a);
				col.rgb *= col.a;
				col.a = 1.0;
                return clamp(col, 0.0, 1.0);
            }
            ENDCG
        }
    }
}
