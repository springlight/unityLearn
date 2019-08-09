﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Study/MyAlphaBlendZWriteTest"
{
	Properties
	{
		_Color("MainTint",Color) = (1,1,1,1)
		_MainTex("MainTex",2D) = "white"{}
		_AlphaScale("Alpha Cutoff",Range(0,1)) = 1//控制整体透明度
	}
		SubShader
		{
			Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			Pass
			{
				ZWrite On
				ColorMask 0//该命令表示当前Pass 不写入任何颜色通道，即不会输出任何颜色
			}
			Pass
			{
				Tags{"LightMode" = "ForwardBase"}
				ZWrite Off
				Blend SrcAlpha OneMinusSrcAlpha
				CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "Lighting.cginc"
				float4 _Color;
				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _AlphaScale;

				struct a2v
				{
					float4 vertex:POSITION;
					float3 normal:NORMAL;
					float4 texcoord:TEXCOORD0;
				};
				struct v2f
				{
					float4 pos:SV_POSITION;
					float3 worldNormal:TEXCOORD0;
					float3 worldPos:TEXCOORD1;
					float2 uv:TEXCOORD2;
				};

				v2f vert(a2v v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xzy;
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}

				float4 frag(v2f i) :SV_Target
				{
					float3 worldNormal = normalize(i.worldNormal);
					float3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
					float4 texColor = tex2D(_MainTex, i.uv);

					float3 albedo = texColor.rgb *_Color.rgb;
					float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz*albedo;
					float3 diffuse = _LightColor0.rgb*albedo*max(0, dot(worldNormal, worldLightDir));
					return float4(ambient + diffuse, texColor.a*_AlphaScale);
				}
			ENDCG
		}
		}
}