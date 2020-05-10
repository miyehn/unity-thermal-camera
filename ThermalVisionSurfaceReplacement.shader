Shader "Hidden/ThermalVisionSurfaceReplacement"
{
	// Replacement shader for non-heat-emissive objects
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf ThermalVisionReplacement 
		#pragma target 3.0

		// global input
		float _EnvironmentTemperature;

		struct Input
		{
			float2 uv_MainTex;
		};

		half4 LightingThermalVisionReplacement(SurfaceOutput o, half3 lightDir, half atten)
		{
			half NdotL = dot(o.Normal, lightDir);
			half3 res = o.Albedo * _LightColor0.rgb * saturate(NdotL) * atten;
			return half4(res, 1);
		}

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o)
		{
			o.Albedo = _EnvironmentTemperature;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
