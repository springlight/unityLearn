
Shader "Study/02-Refraction" {
	Properties{
		_Color("Color Tint", Color) = (1,1,1,1)
		_RefractColor("Refraction Color",Color) = (1,1,1,1)
		_RefractAmount("Refract Amount",Range(0,1)) = 1
		_RefractRatio("Refract Ratio",Range(0,1)) = 1
		_Cubemap("Refraction Cubemap",Cube) = "_Skybox"{}
	}
		SubShader
	{

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma multi_compile_fwdbase	
			#pragma vertex vert	
			#pragma fragment frag
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			float4 _Color;
			float4 _RefractColor;
			float _RefractAmount;
			float _RefractRatio;
			samplerCUBE _Cubemap;

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldnormal : TEXCOORD0;
				float3 worldpos : TEXCOORD1;
				float3 worldViewDir : TEXCOORD2;
				float3 worldRefract : TEXCOORD3;
				SHADOW_COORDS(4)
			};

			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityWorldToClipPos(v.vertex);
				o.worldnormal = UnityObjectToWorldNormal(v.normal);
				o.worldpos = mul(unity_ObjectToWorld,v.vertex).xyz;
				o.worldViewDir = UnityWorldSpaceViewDir(o.worldpos);
				o.worldRefract = refract(normalize(o.worldViewDir),normalize(o.worldnormal),_RefractRatio);
				TRANSFER_SHADOW(o);
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float3 worldnormal = normalize(i.worldnormal);
				float3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldpos));
				float3 worldViewDir = normalize(i.worldViewDir);

				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				float3 diffuse = _LightColor0.rgb * _Color.rgb * max(0,dot(worldnormal,worldLightDir));

				float3 Refraction = texCUBE(_Cubemap,normalize(i.worldRefract)).rgb * _RefractColor.rgb;
				UNITY_LIGHT_ATTENUATION(atten,i,i.worldpos);
				float3 color = ambient + lerp(diffuse ,Refraction,_RefractAmount) * atten;
				return float4(color,1.0);

			}
			ENDCG
		}


	}
		FallBack "Reflective/VertexLit"

}