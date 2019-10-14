Shader "Study/GlossyTexture"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
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
				float4 posWorld:TEXCOORD0;
				float3 normalDir:TEXCOORD1;
				float4 tex:TEXCOORD2;

			};



			vetexOutput vert(vertexInput v)
			{
				vetexOutput o;
				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;


				o.posWorld = mul(modelMatrix, v.vertex);
				o.normalDir = normalize(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
				o.tex = v.texcoord;
				o.pos = UnityObjectToClipPos(v.vertex);



				
		
				return o;
			}

			fixed4 frag(vetexOutput i) : SV_Target
			{

				float3 normalDir = normalize(i.normalDir);

				float3 viewDir = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);
				float lightDir;
				float attenuation;
				float4 textureColor = tex2D(_MainTex, i.tex.xy);

				if (0.0 == _WorldSpaceLightPos0.w) //方向光
				{
					attenuation = 1.0;
					lightDir = normalize(_WorldSpaceLightPos0);
				}
				else
				{
					float3 vertexToLightSrc = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
					float distance = length(vertexToLightSrc);
					attenuation = 1.0 / distance;
					lightDir = normalize(vertexToLightSrc);
				}
				float3 ambient = textureColor * UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
				float3 diffuseReflection = textureColor * attenuation * _LightColor0.rgb *_Color.rgb*max(0.0, dot(normalDir, lightDir));
				float3 specularReflection;
				if (dot(normalDir, lightDir) < 0.0) {
					specularReflection = float3(0.0, 0.0, 0.0);
				}
				else {
					specularReflection = attenuation * _LightColor0.rgb *_SpecColor.grb*(1.0- textureColor.a)*pow(max(0.0, dot(reflect(-lightDir, normalDir), viewDir)), _Shininess);


				}

				return float4(ambient + diffuseReflection + specularReflection, 1.0);
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
				float4 posWorld:TEXCOORD0;
				float3 normalDir:TEXCOORD1;
				float4 tex:TEXCOORD2;

			};



			vetexOutput vert(vertexInput v)
			{
				vetexOutput o;
				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;


				o.posWorld = mul(modelMatrix, v.vertex);
				o.normalDir = normalize(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
				o.tex = v.texcoord;
				o.pos = UnityObjectToClipPos(v.vertex);





				return o;
			}

			fixed4 frag(vetexOutput i) : SV_Target
			{

				float3 normalDir = normalize(i.normalDir);

				float3 viewDir = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);
				float lightDir;
				float attenuation;
				float4 textureColor = tex2D(_MainTex, i.tex.xy);

				if (0.0 == _WorldSpaceLightPos0.w) //方向光
				{
					attenuation = 1.0;
					lightDir = normalize(_WorldSpaceLightPos0);
				}
				else
				{
					float3 vertexToLightSrc = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
					float distance = length(vertexToLightSrc);
					attenuation = 1.0 / distance;
					lightDir = normalize(vertexToLightSrc);
				}
				float3 ambient = textureColor * UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
				float3 diffuseReflection = textureColor * attenuation * _LightColor0.rgb *_Color.rgb*max(0.0, dot(normalDir, lightDir));
				float3 specularReflection;
				if (dot(normalDir, lightDir) < 0.0) {
					specularReflection = float3(0.0, 0.0, 0.0);
				}
				else {
					specularReflection = attenuation * _LightColor0.rgb *_SpecColor.rgb*(1.0 - textureColor.a)*pow(max(0.0, dot(reflect(-lightDir, normalDir), viewDir)), _Shininess);


				}

				return float4(ambient + diffuseReflection + specularReflection, 1.0);
			}
			ENDCG
		}

		}
}
