Shader "Effect/BackgroundUI"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_FlowMapTex("FlowMap", 2D) = "white" {}
		_FlowCycle("Cycle", Float) = 4.0
		_FlowCycleSpeed("Cycle Speed", Float) = 1.0
		_FlowSpeed("Speed", Float) = 1.5
		_FlowScale("Scale (flow x,y)", Vector) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
			sampler2D _FlowMapTex;
			float4 _MainTex_ST;

			float _FlowCycle;
			float _FlowCycleSpeed;
			float _FlowSpeed;
			float4 _FlowScale;

			// from https://www.shadertoy.com/view/tdfSW4
			// author : pierre.plans@gmail.com
			void flowmap(
				float blend_cycle, float cycle_speed,
				float offset,
				float2 flow, float flow_speed, float2 flow_scale,
				float2 base_uv,
				out float blend_factor,
				out float2 oLayer1, out float2 oLayer2)
			{
				float half_cycle = blend_cycle * 0.5;

				float phase1 = fmod(offset + _Time.x * cycle_speed, blend_cycle);
				float phase2 = fmod(offset + _Time.x * cycle_speed + half_cycle, blend_cycle);

				blend_factor = abs(half_cycle - phase1) / half_cycle;

				phase1 -= half_cycle;
				phase2 -= half_cycle;

				flow *= flow_speed * flow_scale;

				oLayer1 = flow * phase1 + base_uv;
				oLayer2 = flow * phase2 + base_uv;
			}

			float4 applyFlowMap(sampler2D _origin, sampler2D _flowmap, float2 baseUV)
			{
				float offset = 0.5;
				float2 flow = -1.0 + 2.0*tex2D(_flowmap, baseUV).rg;
				float oBlend_factor = 0.0;
				float2 oLayer1, oLayer2;

				flowmap(_FlowCycle, _FlowCycleSpeed,
					offset,
					flow, _FlowSpeed, _FlowScale.xy,
					baseUV.xy,
					oBlend_factor,
					oLayer1, oLayer2);

				return lerp(
					tex2D(_origin, oLayer1),
					tex2D(_origin, oLayer2),
					oBlend_factor);
			}

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
                fixed4 col = applyFlowMap(_MainTex, _FlowMapTex, i.uv);
				col.gb = float2(0.0, 0.0);
                return col;
            }
            ENDCG
        }
    }
}
