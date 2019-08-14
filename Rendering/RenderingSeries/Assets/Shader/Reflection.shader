// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Study/Reflection"
{
	Properties
	{
		_Color("Color Tint",Color) = (1,1,1,1)
		//控制反射颜色
		_ReflectColor("Reflection Color",Color) = (1,1,1,1)
		//反射程度
		_ReflectAmount("Reflect Amount",Range(0,1)) = 1
		_Cubemap("Reflection Cubemap",Cube) ="_Skybox"{}
	}
	SubShader
	{

		Tags{"RenderType" = "Opaque" "Queue" = "Geometry"}
		Pass
		{
			Tags{"LightMode" = "ForwardBase"}
			CGPROGRAM
#pragma multi_compile_fwdbase

			#pragma vertex vert
			#pragma fragment frag

#include "Lighting.cginc"
#include "AutoLight.cginc"
		float4 _Color;
	float4 _ReflectColor;
	float _ReflectAmount;
	samplerCUBE _Cubemap;

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal:NORMAL;
			
			};

			struct v2f
			{
				float4 pos:SV_POSITION;
				float3 worldPos:TEXCOORD0;
				float3 worldNormal:TEXCOORD1;
				float worldViewDir : TEXCOORD2;
				float3 worldRefl:TEXCOORD3;
				SHADOW_COORDS(4)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.worldViewDir = UnityWorldSpaceViewDir(o.worldPos);
				o.worldRefl = reflect(-o.worldViewDir, o.worldNormal);
				TRANSFER_SHADOW(o);

				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float3 worldNormal = normalize(i.worldNormal);
				float worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				float3 wolrdViewDir = normalize(i.worldViewDir);
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				float3 diffuse = _LightColor0.rgb*_Color.rgb*max(0, dot(worldNormal, worldLightDir));
				float3 reflection = texCUBE(_Cubemap, i.worldRefl).rgb*_ReflectColor.rgb;
				UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);
				float3 color = ambient + lerp(diffuse, reflection, _ReflectAmount)*atten;
				return float4(color,1.0);
			}
			ENDCG
		}
	}
		FallBack "Reflective/VertexLit"
}
