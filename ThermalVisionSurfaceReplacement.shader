Shader "Hidden/ThermalVisionSurfaceReplacement"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
	}

	CGINCLUDE
	#pragma target 3.0

	// global input
	float _EnvironmentTemperature;

	sampler2D _MainTex;
	float4 _Color;

	// fragment
	struct Input
	{
		float2 uv_MainTex;
	};

	ENDCG

	// Replacement shader for non-heat-emissive objects
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf ThermalVisionReplacement 

		half4 LightingThermalVisionReplacement(SurfaceOutput o, half3 lightDir, half atten)
		{
			half NdotL = dot(o.Normal, lightDir);
			half3 res = o.Albedo * _LightColor0.rgb * saturate(NdotL);
			return half4(res, 1);
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			o.Albedo = _EnvironmentTemperature;
		}
		ENDCG
	}

	// for explicitly controlled
	SubShader
	{
		Tags { "RenderType"="Temperature" }
		LOD 200

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = mul(UNITY_MATRIX_MV, v.normal);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				half cosTheta = i.normal.z;
				half edgeCool = pow(1 - cosTheta, 1.5);

				half3 Temp = _EnvironmentTemperature + _Color.r - edgeCool * 0.5f;
				return half4(Temp, 1);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
