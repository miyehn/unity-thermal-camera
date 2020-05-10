Shader "Hidden/ThermalVisionWarm"
{
	// Replacement shader for non-heat-emissive objects
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf ThermalVisionWarm 
		#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		half4 LightingThermalVisionWarm(SurfaceOutput o, half3 lightDir, half atten)
		{
			return half4(0.4, 0.3, 0, 1);
		}

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o)
		{
		}
		ENDCG
	}
	FallBack "Diffuse"
}
