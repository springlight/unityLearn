Shader "Study/Diffuselighting"
{
	Properties{
		_Color ("Diffuse Material Color",Color)= (1,1,1,1)
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
	struct vertexInput {
		float4 vertex:POSITION;
		float3 normal:NORMAL;
	};
	struct vertexOutput {
		float4 pos:SV_POSITION;
		float4 col:COLOR;
	};

	vertexOutput vert(vertexInput input) {
		vertexOutput outPut;
		float4x4 otw = unity_ObjectToWorld;//物体坐标系转世界坐标系矩阵
		float4x4 wto = unity_WorldToObject;//世界坐标系转物体坐标系矩阵
		float3 normalDir = normalize(mul(float4(input.normal, 0.0), wto).xyz);
	//	float3 normalDir = UnityObjectToWorldNormal(input.normal);
		//float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

		float attenuation;

		float3 lightDir;
		if (0.0 == _WorldSpaceLightPos0.w) {//方向光
			attenuation = 1.0;
			lightDir = normalize(_WorldSpaceLightPos0.xyz);

		}
		else {
			float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - mul(otw, input.vertex).xyz;
			float distance = length(vertexToLightSource);
			attenuation = 1.0 / distance;
			lightDir = normalize(vertexToLightSource);
		}

		float3 diffuseReflection = attenuation*_LightColor0.rgb *_Color.rgb*max(0.0, dot(normalDir, lightDir));
		outPut.col = float4(diffuseReflection, 1.0);
		outPut.pos = UnityObjectToClipPos(input.vertex);
		return outPut;
	}
	float4 frag(vertexOutput input) :COLOR{
		return input.col;
	}
		ENDCG
		}
	
		Pass{
		Tags {"LightMode" = "ForwardAdd"}

		Blend One One
		 CGPROGRAM

		 #pragma vertex vert  
		 #pragma fragment frag 

		 #include "UnityCG.cginc"

		 uniform float4 _LightColor0;
	// color of light source (from "Lighting.cginc")

	uniform float4 _Color; // define shader property for shaders

 struct vertexInput {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
 };
 struct vertexOutput {
	float4 pos : SV_POSITION;
	float4 col : COLOR;
 };

 vertexOutput vert(vertexInput input)
 {
	vertexOutput output;

	float4x4 modelMatrix = unity_ObjectToWorld;
	float4x4 modelMatrixInverse = unity_WorldToObject;

	float3 normalDirection = normalize(
	   mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
	//float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
	float attenuation;

	float3 lightDir;
	if (0.0 == _WorldSpaceLightPos0.w) {//方向光
		attenuation = 1.0;
		lightDir = normalize(_WorldSpaceLightPos0.xyz);

	}
	else {
		float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - mul(modelMatrix, input.vertex).xyz;
		float distance = length(vertexToLightSource);
		attenuation = 1.0 / distance;
		lightDir = normalize(vertexToLightSource);
	}

	float3 diffuseReflection = attenuation * _LightColor0.rgb *_Color.rgb*max(0.0, dot(normalDirection, lightDir));

	output.col = float4(diffuseReflection, 1.0);
	output.pos = UnityObjectToClipPos(input.vertex);
	return output;
 }

 float4 frag(vertexOutput input) : COLOR
 {
	return input.col;
 }

 ENDCG
		}
	
	}
}
