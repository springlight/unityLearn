
Shader "Study/03-Fresne" {
	Properties{
		_Color("Color Tint", Color) = (1,1,1,1)
		_FresnelScale("Fresnel Scale",Range(0,1)) = 0.5
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

			fixed4 _Color;
			float _FresnelScale;
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
				float3 worldReflect : TEXCOORD3;
				SHADOW_COORDS(4)
			};

			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityWorldToClipPos(v.vertex);
				o.worldnormal = UnityObjectToWorldNormal(v.normal);
				o.worldpos = mul(unity_ObjectToWorld,v.vertex).xyz;
				o.worldViewDir = UnityWorldSpaceViewDir(o.worldpos);
				o.worldReflect = reflect(-o.worldViewDir,o.worldnormal);
				TRANSFER_SHADOW(o);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed3 worldnormal = normalize(i.worldnormal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldpos));
				fixed3 worldViewDir = normalize(i.worldViewDir);

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				fixed3 diffuse = _LightColor0.rgb * _Color.rgb * max(0,dot(worldnormal,worldLightDir));

				fixed3 reflection = texCUBE(_Cubemap,i.worldReflect).rgb;

				fixed3 fresnel = _FresnelScale + (1 - _FresnelScale) * pow(1 - dot(worldViewDir,worldnormal),5);

				UNITY_LIGHT_ATTENUATION(atten,i,i.worldpos);

				fixed3 color = ambient + lerp(diffuse ,reflection,saturate(fresnel)) * atten;

				return fixed4(color,1.0);

			}
			ENDCG
		}


	}
		FallBack "Reflective/VertexLit"
}
