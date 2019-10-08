Shader "Study/perpixellighting"
{
	Properties{
		_Color("Diffuse Material Color",Color) = (1,1,1,1)
		_SpecColor("specular Material color",Color) = (1,1,1,1)
		_Shininess("Shininess",Float) = 10
	}

		SubShader
	{
	Pass{
		Tags{"LightMode" = "ForwardBase"}
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
		uniform float4 _LightColor0;
	uniform float4 _Color;
	uniform float4 _SpecColor;
	uniform float _Shininess;


	struct vertexInput {
		float4 vertex:POSITION;
		float3 normal:NORMAL;
	};
	struct vertexOutput {
		float4 pos:SV_POSITION;
		float4 posWorld:TEXCOORD0;
		float3 normalDir:TEXCOORD1;

	};

	vertexOutput vert(vertexInput input) {
		vertexOutput outPut;
		float4x4 otw = unity_ObjectToWorld;//物体坐标系转世界坐标系矩阵
		float4x4 wto = unity_WorldToObject;//世界坐标系转物体坐标系矩阵

		outPut.posWorld = mul(otw, input.vertex);
		//float3 normalDir = normalize(mul(float4(input.normal, 0.0), wto).xyz);
			outPut.normalDir = UnityObjectToWorldNormal(input.normal);
			



		
			outPut.pos = UnityObjectToClipPos(input.vertex);
			return outPut;
		}
		float4 frag(vertexOutput input) :COLOR{
			float3 normalDir = normalize(input.normalDir);
			 float3 viewDir = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);


			 float attenuation;

			 float3 lightDir;
			 if (0.0 == _WorldSpaceLightPos0.w) {//方向光
			 	attenuation = 1.0;
			 	lightDir = normalize(_WorldSpaceLightPos0.xyz);

			 }
			 else {
			 	float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
			 	float distance = length(vertexToLightSource);
			 	attenuation = 1.0 / distance;
			 	lightDir = normalize(vertexToLightSource);
			 }
			 float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb *_Color.rgb;

			 float3 diffuseReflection = attenuation * _LightColor0.rgb *_Color.rgb*max(0.0, dot(normalDir, lightDir));

			 float3 specularReflection;
			 if (dot(normalDir, lightDir) < 0.0) {
			 	specularReflection = float3(0.0, 0.0, 0.0);
			 }
			 else {
			 	specularReflection = attenuation * _LightColor0.rgb *_SpecColor * pow(max(0.0, dot(reflect(-lightDir, normalDir), viewDir)), _Shininess);
			 }
			 return float4(ambient + diffuseReflection
				 + specularReflection, 1.0);
			//return input.col;
		}
			ENDCG
	}

			Pass{
		Tags{"LightMode" = "ForwardAdd"}
		Blend One One
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
		uniform float4 _LightColor0;
	uniform float4 _Color;
	uniform float4 _SpecColor;
	uniform float _Shininess;


	struct vertexInput {
		float4 vertex:POSITION;
		float3 normal:NORMAL;
	};
	struct vertexOutput {
		float4 pos:SV_POSITION;
		float4 posWorld:TEXCOORD0;
		float3 normalDir:TEXCOORD1;

	};

	vertexOutput vert(vertexInput input) {
		vertexOutput outPut;
		float4x4 otw = unity_ObjectToWorld;//物体坐标系转世界坐标系矩阵
		float4x4 wto = unity_WorldToObject;//世界坐标系转物体坐标系矩阵

		outPut.posWorld = mul(otw, input.vertex);
		outPut.normalDir = UnityObjectToWorldNormal(input.normal);





			outPut.pos = UnityObjectToClipPos(input.vertex);
			return outPut;
		}
		float4 frag(vertexOutput input) :COLOR{
			float3 normalDir = normalize(input.normalDir);
			 float3 viewDir = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);


			 float attenuation;

			 float3 lightDir;
			 if (0.0 == _WorldSpaceLightPos0.w) {//方向光
				attenuation = 1.0;
				lightDir = normalize(_WorldSpaceLightPos0.xyz);

			 }
			 else {
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
				float distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDir = normalize(vertexToLightSource);
			 }
			 float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb *_Color.rgb;

			 float3 diffuseReflection = attenuation * _LightColor0.rgb *_Color.rgb*max(0.0, dot(normalDir, lightDir));

			 float3 specularReflection;
			 if (dot(normalDir, lightDir) < 0.0) {
				specularReflection = float3(0.0, 0.0, 0.0);
			 }
			 else {
				specularReflection = attenuation * _LightColor0.rgb *_SpecColor * pow(max(0.0, dot(reflect(-lightDir, normalDir), viewDir)), _Shininess);
			 }

			 return float4(ambient + diffuseReflection
				 + specularReflection, 1.0);
		}
			ENDCG
			}
	}
}
