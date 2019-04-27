Shader "Effect/Obfuscate"
{
	Properties
	{
		_FlowMap("FlowMap ", 2D) = "" {}
		_FlowCycle("Cycle", Float) = 4.0
		_FlowCycleSpeed("Cycle Speed", Float) = 1.0
		_FlowSpeed("Speed", Float) = 1.5
		_FlowScale("Scale (flow x,y)", Vector) = (1, 1, 1, 1)
	}
	SubShader
	{
		// Draw ourselves after all opaque geometry
		Tags { "Queue" = "Transparent" }

		// Grab the screen behind the object into _BackgroundTexture
		GrabPass
		{
			"_BackgroundTexture"
		}

		// Render the object with the texture generated above, and invert the colors
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

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

			float4 applyFlowMap(sampler2D _origin, sampler2D _flowmap, float4 baseUV)
			{
				float offset = 0.5;
				float2 flow = -1.0+2.0*tex2Dproj(_flowmap, baseUV).rg;
				float oBlend_factor = 0.0;
				float2 oLayer1, oLayer2;

				flowmap(_FlowCycle, _FlowCycleSpeed,
					offset,
					flow, _FlowSpeed, _FlowScale.xy,
					baseUV.xy,
					oBlend_factor,
					oLayer1, oLayer2);
				//return _FlowScale;
				//return float4(_FlowScale.x, _FlowScale.y, 0.0, 1.0);
				//Debug return half4(oLayer1.x, oLayer1.y, oBlend_factor, oBlend_factor);
				return lerp(
					tex2Dproj(_origin, float4(oLayer1, baseUV.z, baseUV.w)),
					tex2Dproj(_origin, float4(oLayer2, baseUV.z, baseUV.w)),
					oBlend_factor);
			}

			struct v2f
			{
				float4 grabPos : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_base v) {
				v2f o;
				// use UnityObjectToClipPos from UnityCG.cginc to calculate 
				// the clip-space of the vertex
				o.pos = UnityObjectToClipPos(v.vertex);
				// use ComputeGrabScreenPos function from UnityCG.cginc
				// to get the correct texture coordinate
				o.grabPos = ComputeGrabScreenPos(o.pos);
				return o;
			}

			sampler2D _FlowMap;
			float4 _FlowMap_ST;
			sampler2D _BackgroundTexture;

			half4 frag(v2f i) : SV_Target
			{
				return applyFlowMap(_BackgroundTexture, _FlowMap, i.grabPos);
			}
			ENDCG
		}

	}
}