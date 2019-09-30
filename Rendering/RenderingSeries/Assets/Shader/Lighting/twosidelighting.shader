Shader "Study/twosidelighting"
{
	Properties{
		_Color("Front Material Diffuse Color",Color) = (1,1,1,1)
		_SpecColor("Front Material Specular Color",Color) = (1,1,1,1)
		_Shininess("Front Material Shininess",Float) = 10
		_BackColor("Back Material Diffuse Color",Color) = (1,1,1,1)
		_BackSpeColor("Back Material Specular Color",Color) = (1,1,1,1)
		_BackShininess("Back Material Shininess",Float) = 10
	}
		SubShader
	{
		Pass{
			Tags{"LightMode" = "ForwardBase"}
			Cull Back//先渲染前面
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
		uniform float4 _LightColor0;
		uniform float4 _Color;
		uniform float4 _SpecColor;
		uniform float _Shininess;
		uniform float4 _BackColor;
		uniform float4 _BackSpeColor;
		uniform float _BackShininess;
		struct vertexInput {
			float4 vertex:POSITION;
			float3 normal:Normal;
		};
		struct vertexOutput {
			float4 pos:SV_POSITION;
			float4 col:COLOR;
		};

		vertexOutput vert(vertexInput input) {
			vertexOutput output;
			float3 normDir = normalize(mul(float4(input.normal, 0.0), unity_WorldToObject).xyz);
			float3 viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, input.vertex).xyz);
			float3 lightDir;
			float attenuation;

			if (0.0 == _WorldSpaceLightPos0.w)//方向光
			{
				attenuation = 1.0;
				lightDir = normalize(_WorldSpaceLightPos0.xyz);
			}
			else {
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - mul(unity_ObjectToWorld, input.vertex).xyz;
				float distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDir = normalize(vertexToLightSource);

			}
			float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
			float3 diffuseReflection = attenuation * _LightColor0.rgb *_Color.rgb * max(0.0, dot(normDir, lightDir));
			float3 specularReflection;
			if (dot(normDir, lightDir) < 0.0)//光源方向错误
			{
				specularReflection = float3(0.0, 0.0, 0.0);
			}
			else {
				specularReflection = attenuation * _LightColor0.rgb *_SpecColor.rgb *pow(max(0.0, dot(reflect(-lightDir, normDir), viewDir)), _Shininess);
			}
			output.col = float4(ambientLighting + diffuseReflection + specularReflection, 1.0);
			output.pos = UnityObjectToClipPos(input.vertex);
			return output;
		}
		float4 frag(vertexOutput input) :COLOR{
			return input.col;
		}

			ENDCG
		}
		Pass{
				Tags{"LightMode" = "ForwardAdd"}
				Blend One One
			Cull Back
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
		uniform float4 _LightColor0;
		uniform float4 _Color;
		uniform float4 _SpecColor;
		uniform float _Shininess;
		uniform float4 _BackColor;
		uniform float4 _BackSpeColor;
		uniform float _BackShininess;
		struct vertexInput {
			float4 vertex:POSITION;
			float3 normal:Normal;
		};
		struct vertexOutput {
			float4 pos:SV_POSITION;
			float4 col:COLOR;
		};

		vertexOutput vert(vertexInput input) {
			vertexOutput output;
			float3 normDir = normalize(mul(float4(input.normal, 0.0), unity_WorldToObject).xyz);
			float3 viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, input.vertex).xyz);
			float3 lightDir;
			float attenuation;

			if (0.0 == _WorldSpaceLightPos0.w)//方向光
			{
				attenuation = 1.0;
				lightDir = normalize(_WorldSpaceLightPos0.xyz);
			}
			else {
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - mul(unity_ObjectToWorld, input.vertex).xyz;
				float distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDir = normalize(vertexToLightSource);

			}
			float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
			float3 diffuseReflection = attenuation * _LightColor0.rgb *_Color.rgb * max(0.0, dot(normDir, lightDir));
			float3 specularReflection;
			if (dot(normDir, lightDir) < 0.0)//光源方向错误
			{
				specularReflection = float3(0.0, 0.0, 0.0);
			}
			else {
				specularReflection = attenuation * _LightColor0.rgb *_SpecColor.rgb *pow(max(0.0, dot(reflect(-lightDir, normDir), viewDir)), _Shininess);
			}
			output.col = float4(ambientLighting + diffuseReflection + specularReflection, 1.0);
			output.pos = UnityObjectToClipPos(input.vertex);
			return output;
		}
		float4 frag(vertexOutput input) :COLOR{
			return input.col;
		}
			ENDCG
		}
		Pass{
				Tags{"LightMode" = "ForwardBase"}
				
			Cull Front
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
		uniform float4 _LightColor0;
		uniform float4 _Color;
		uniform float4 _SpecColor;
		uniform float _Shininess;
		uniform float4 _BackColor;
		uniform float4 _BackSpeColor;
		uniform float _BackShininess;
		struct vertexInput {
			float4 vertex:POSITION;
			float3 normal:Normal;
		};
		struct vertexOutput {
			float4 pos:SV_POSITION;
			float4 col:COLOR;
		};

		vertexOutput vert(vertexInput input) {
			vertexOutput output;
			float3 normDir = normalize(mul(float4(-input.normal, 0.0), unity_WorldToObject).xyz);
			float3 viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, input.vertex).xyz);
			float3 lightDir;
			float attenuation;

			if (0.0 == _WorldSpaceLightPos0.w)//方向光
			{
				attenuation = 1.0;
				lightDir = normalize(_WorldSpaceLightPos0.xyz);
			}
			else {
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - mul(unity_ObjectToWorld, input.vertex).xyz;
				float distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDir = normalize(vertexToLightSource);

			}
			float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
			float3 diffuseReflection = attenuation * _LightColor0.rgb *_Color.rgb * max(0.0, dot(normDir, lightDir));
			float3 specularReflection;
			if (dot(normDir, lightDir) < 0.0)//光源方向错误
			{
				specularReflection = float3(0.0, 0.0, 0.0);
			}
			else {
				specularReflection = attenuation * _LightColor0.rgb *_SpecColor.rgb *pow(max(0.0, dot(reflect(-lightDir, normDir), viewDir)), _Shininess);
			}
			output.col = float4(ambientLighting + diffuseReflection + specularReflection, 1.0);
			output.pos = UnityObjectToClipPos(input.vertex);
			return output;
		}
		float4 frag(vertexOutput input) :COLOR{
			return input.col;
		}
			ENDCG
		}
		Pass{
				Tags{"LightMode" = "ForwardAdd"}
				Blend One One
			Cull Front
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
		uniform float4 _LightColor0;
		uniform float4 _Color;
		uniform float4 _SpecColor;
		uniform float _Shininess;
		uniform float4 _BackColor;
		uniform float4 _BackSpeColor;
		uniform float _BackShininess;
		struct vertexInput {
			float4 vertex:POSITION;
			float3 normal:Normal;
		};
		struct vertexOutput {
			float4 pos:SV_POSITION;
			float4 col:COLOR;
		};

		vertexOutput vert(vertexInput input) {
			vertexOutput output;
			float3 normDir = normalize(mul(float4(-input.normal, 0.0), unity_WorldToObject).xyz);
			float3 viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, input.vertex).xyz);
			float3 lightDir;
			float attenuation;

			if (0.0 == _WorldSpaceLightPos0.w)//方向光
			{
				attenuation = 1.0;
				lightDir = normalize(_WorldSpaceLightPos0.xyz);
			}
			else {
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - mul(unity_ObjectToWorld, input.vertex).xyz;
				float distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDir = normalize(vertexToLightSource);

			}
			float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
			float3 diffuseReflection = attenuation * _LightColor0.rgb *_Color.rgb * max(0.0, dot(normDir, lightDir));
			float3 specularReflection;
			if (dot(normDir, lightDir) < 0.0)//光源方向错误
			{
				specularReflection = float3(0.0, 0.0, 0.0);
			}
			else {
				specularReflection = attenuation * _LightColor0.rgb *_SpecColor.rgb *pow(max(0.0, dot(reflect(-lightDir, normDir), viewDir)), _Shininess);
			}
			output.col = float4(ambientLighting + diffuseReflection + specularReflection, 1.0);
			output.pos = UnityObjectToClipPos(input.vertex);
			return output;
		}
		float4 frag(vertexOutput input) :COLOR{
			return input.col;
		}
			ENDCG
		}
	}
}
