﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Study/FirstLight"{
		Properties{
			_Tint("Tint",Color)=(1,1,1,1)
			_MainTex("Albedo",2D)="white"{}
			_Smoothness("Smoothness",Range(0,1)) = 0.5
			[Gamma]_Metallic("Metallic",Range(0,1)) = 0
		  // _SpecularTint("Specular",Color) = (0.5,0.5,0.5)
		}

		SubShader{
			Pass{
			Tags{"LightMode" = "ForwardBase"}
			CGPROGRAM
#pragma target 3.0
	#pragma vertex vert
	#pragma fragment frag
		/*	#include "UnityCG.cginc"*/
			#include "UnityStandardBRDF.cginc"
#include "UnityStandardUtils.cginc"
#include "UnityPBSLighting.cginc"
			sampler2D _MainTex;
	float4 _MainTex_ST;
	float4 _Tint;
	float _Smoothness;
	//float4 _SpecularTint;
	float _Metallic;
		struct a2v {
			float4 pos:POSITION;
			float3 normal:NORMAL;
			float2 uv:TEXCOORD0;

		};
	struct v2f {
		float4 pos:SV_POSITION;
		float2 uv:TEXCOORD0;
		float3 normal:TEXCOORD1;
		float3 worldPos:TEXCOORD2;
		};

		v2f vert(a2v v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.pos);

			o.uv = v.uv*_MainTex_ST.xy + _MainTex_ST.zw;
			o.worldPos = mul(unity_ObjectToWorld, v.pos);
			//o.normal = mul(unity_ObjectToWorld,float4(v.normal,0));
			o.normal = mul(transpose((float3x3)unity_WorldToObject), v.normal);
			o.normal = normalize(o.normal);
			return o;
		}
		float4 frag(v2f i) :SV_Target{
			i.normal = normalize(i.normal);

		float3 lightDir = _WorldSpaceLightPos0.xyz;
		float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
		float3 lightColor = _LightColor0.rgb;
		float3 albedo = tex2D(_MainTex, i.uv).rgb*_Tint.rgb;

		/*albedo *= 1 -
			max(_SpecularTint.r, max(_SpecularTint.g, _SpecularTint.b));*/
		float3 specularTint;// = albedo * _Metallic;
		float oneMinusReflectivity;// = 1 - _Metallic;
		albedo = DiffuseAndSpecularFromMetallic(albedo, _Metallic, specularTint, oneMinusReflectivity);
		//albedo *= oneMinusReflectivity;
		//albedo = EnergyConservationBetweenDiffuseAndSpecular(albedo, _SpecularTint.rgb, oneMinusReflectivity);
		/*float3 diffuse =albedo * lightColor * DotClamped(lightDir, i.normal);

		float3 halfVector = normalize(lightDir + viewDir);
		float3 specular = specularTint*lightColor * pow(DotClamped(halfVector, i.normal), _Smoothness * 100);
		return  float4(specular+diffuse, 1);*/
		UnityLight light;
		light.color = lightColor;
		light.dir = lightDir;
		light.ndotl = DotClamped(i.normal, lightDir);
		UnityIndirect indirectLight;
		indirectLight.diffuse = 0;
		indirectLight.specular = 0;

		return UNITY_BRDF_PBS(
		albedo,specularTint, oneMinusReflectivity, _Smoothness,i.normal,viewDir,light, indirectLight);

		}
		ENDCG
		}
	}

}