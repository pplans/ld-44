Shader "Effect/DeathMark"
{
    Properties
    {
		[HDR]_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Alpha (A)", 2D) = "white" {}
        _Power ("Power", Range(0,100)) = 1.0
		_Speed("Speed", Range(0,20)) = 1.0
		_Scale("Scale", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
		Blend One OneMinusSrcAlpha
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Power;
		float _Speed;
		float _Scale;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 uv = IN.uv_MainTex;
			float c = cos(_Time.x*_Speed), s = sin(_Time.x*_Speed);
			uv = mul(float2x2(c*_Scale, -s, s, c*_Scale), uv-0.5)+0.5;
			float alpha = tex2D(_MainTex, uv).a;
            // Albedo comes from a texture tinted by color
            o.Albedo = o.Emission = _Color * _Power;
            // Metallic and smoothness come from slider variables
            o.Alpha = alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
