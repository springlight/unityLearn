Shader "Study/lightingwithtexture"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color",COLOR) = (1,1,1,1)
		_SpecColor("SpecColor",COLOR) = (1,1,1,1)
		_Shininess("Shininess",Float) = 10
	}
		SubShader
		{


			Pass
			{
				Tags{"LightMode" = "ForwardBase"}
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag


				#include "UnityCG.cginc"

			 float4 _LightColor0;
			sampler2D _MainTex;
			float4 _Color;
			float4 _SpecColor;
			float _Shininess;
			float4 _MainTex_ST;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal:NORMAL;
				float4 texcoord:TEXCOORD0;
			};

			struct vetexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex:TEXCOORD0;
				float3 diffuseColor:TEXCOORD1;
				float3 specularColor:TEXCOORD2;
			};

		
			
			vetexOutput vert (vertexInput v)
			{
				vetexOutput o;
				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				float3 normalDir = normalize(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
				float3 viewDir = normalize(_WorldSpaceCameraPos - mul(modelMatrix, v.vertex).xyz);
				float lightDir;
				float attenuation;

				if (0.0 == _WorldSpaceLightPos0.w) //方向光
				{
					attenuation = 1.0;
					lightDir = normalize(_WorldSpaceLightPos0);
				}
				else
				{
					float3 vertexToLightSrc = _WorldSpaceLightPos0.xyz - mul(modelMatrix, v.vertex).xyz;
					float distance = length(vertexToLightSrc);
					attenuation = 1.0 / distance;
					lightDir = normalize(vertexToLightSrc);
				}
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
				float3 diffuseReflection = attenuation * _LightColor0.rgb *_Color.rgb*max(0.0, dot(normalDir, lightDir));
				float3 specularReflection;
				if (dot(normalDir, lightDir) < 0.0) {
					specularReflection = float3(0.0, 0.0, 0.0);
				}
				else {
					specularReflection = attenuation * _LightColor0.rgb *_SpecColor.rgb*pow(max(0.0, dot(reflect(-lightDir, normalDir), viewDir)), _Shininess);


				}
				o.diffuseColor = ambient + diffuseReflection;
				o.specularColor = specularReflection;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.tex = v.texcoord;
				return o;
			}
			
			fixed4 frag (vetexOutput i) : SV_Target
			{
				// sample the texture
				fixed4 col = float4(i.specularColor + i.diffuseColor*tex2D(_MainTex,i.tex.xy),1.0);

				return col;
			}
			ENDCG
		}


			Pass
			{
				Tags{"LightMode" = "ForwardAdd"}
				Blend One One
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag


				#include "UnityCG.cginc"

			 float4 _LightColor0;
			sampler2D _MainTex;
			float4 _Color;
			float4 _SpecColor;
			float _Shininess;
			float4 _MainTex_ST;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal:NORMAL;
				float4 texcoord:TEXCOORD0;
			};

			struct vetexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex:TEXCOORD0;
				float3 diffuseColor:TEXCOORD1;
				float3 specularColor:TEXCOORD2;
			};



			vetexOutput vert(vertexInput v)
			{
				vetexOutput o;
				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				float3 normalDir = normalize(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
				float3 viewDir = normalize(_WorldSpaceCameraPos - mul(modelMatrix, v.vertex).xyz);
				float lightDir;
				float attenuation;

				if (0.0 == _WorldSpaceLightPos0.w) //方向光
				{
					attenuation = 1.0;
					lightDir = normalize(_WorldSpaceLightPos0);
				}
				else
				{
					float3 vertexToLightSrc = _WorldSpaceLightPos0.xyz - mul(modelMatrix, v.vertex).xyz;
					float distance = length(vertexToLightSrc);
					attenuation = 1.0 / distance;
					lightDir = normalize(vertexToLightSrc);
				}
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
				float3 diffuseReflection = attenuation * _LightColor0.rgb *_Color.rgb*max(0.0, dot(normalDir, lightDir));
				float3 specularReflection;
				if (dot(normalDir, lightDir) < 0.0) {
					specularReflection = float3(0.0, 0.0, 0.0);
				}
				else {
					specularReflection = attenuation * _LightColor0.rgb *_SpecColor.rgb*pow(max(0.0, dot(reflect(-lightDir, normalDir), viewDir)), _Shininess);


				}
				o.diffuseColor = ambient + diffuseReflection;
				o.specularColor = specularReflection;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.tex = v.texcoord;
				return o;
			}

			fixed4 frag(vetexOutput i) : SV_Target
			{
				// sample the texture
				fixed4 col = float4(i.specularColor + i.diffuseColor*tex2D(_MainTex,i.tex.xy),1.0);

				return col;
			}
			ENDCG
		}
	}
}
