Shader "Study/ForwardRendering"
{
	Properties
	{
		_Diffuse("Diffuse",Color) = (1,1,1,1)
		_Specular("Specular",Color) = (1,1,1,1)
		_Gloss("Gloss",Range(8.0,256)) = 20
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }


			Pass
			{
				Tags{"LightMode" = "ForwardBase"}
					CGPROGRAM
		#pragma multi_compile_fwdbase
					#pragma vertex vert
					#pragma fragment frag
		#include "Lighting.cginc"

			float4 _Diffuse;
			float4 _Specular;
			float _Gloss;
				struct a2v
				{
					float4 vertex : POSITION;
					float3 normal:NORMAL;
				};

				struct v2f
				{
					float4 pos:SV_POSITION;
					float3 worldNormal:TEXCOORD0;
					float3 worldPos:TEXCOORD1;
				};
			
				v2f vert (a2v v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				
					return o;
				}
			
				fixed4 frag (v2f i) : SV_Target
				{
					float3 worldNormal = normalize(i.worldNormal);
					float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
					float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
					float3 diffuse = _LightColor0.rgb *_Diffuse.rgb*max(0, dot(worldNormal, worldLightDir));
					float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
					float3 halfDir = normalize(worldLightDir + viewDir);
					float3 specular = _LightColor0.rgb *_Specular.rgb*pow(max(0, dot(worldNormal, halfDir)), _Gloss);
					float atten = 1.0;
					return float4(ambient + (diffuse + specular) * atten, 1.0);
				}
				ENDCG
			}

			Pass
			{
				Tags{"LightMode" = "ForwardAdd"}
				Blend One One
				CGPROGRAM
				#pragma multi_compile_fwdadd
				#pragma vertex vert
				#pragma fragment frag
				#include "Lighting.cginc"
				#include "AutoLight.cginc"

				float4 _Diffuse;
				float4 _Specular;
				float _Gloss;
					struct a2v
					{
						float4 vertex : POSITION;
						float3 normal:NORMAL;
					};

					struct v2f
					{
						float4 pos:SV_POSITION;
						float3 worldNormal:TEXCOORD0;
						float3 worldPos:TEXCOORD1;
					};

					v2f vert(a2v v)
					{
						v2f o;
						o.pos = UnityObjectToClipPos(v.vertex);
						o.worldNormal = UnityObjectToWorldNormal(v.normal);
						o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

						return o;
					}

					fixed4 frag(v2f i) : SV_Target
					{
						float3 worldNormal = normalize(i.worldNormal);
						float3 worldLightDir;
		#ifdef USING_DIRECTIONAL_LIGHT
						worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
		#else
						worldLightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos.xyz);
		#endif
						float3 diffuse = _LightColor0.rgb *_Diffuse.rgb*max(0, dot(worldNormal, worldLightDir));
						float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
						float3 halfDir = normalize(worldLightDir + viewDir);
						float3 specular = _LightColor0.rgb *_Specular.rgb*pow(max(0, dot(worldNormal, halfDir)), _Gloss);
						float atten;

		#ifdef USING_DIRECTIONAL_LIGHT

						float atten = 1.0;
		#else	
			#if defined(POINT)
						//得到光源空间上的坐标
						float3 lightCoord = mul(unity_WorldToLight, float4(i.worldPos, 1)).xyz;
						atten = tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).UNITY_ATTEN_CHANNEL;
			#elif defined(SPOT)
						float4 lightCoord = mul(unity_WorldToLight, float4(i.worldPos, 1));
						atten = (lightCoord.z > 0)*tex2D(_LightTexture0, lightCoord.xy / lightCoord.w + 0.5).w * tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).UNITY_ATTEN_CHANNEL;
			#else
						atten = 1.0;
			#endif
		#endif
						return float4((diffuse + specular) * atten, 1.0);

					}
				ENDCG
			}
		}
	
}
