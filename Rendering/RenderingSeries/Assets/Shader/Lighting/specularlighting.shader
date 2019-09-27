Shader "Study/specularlighting"
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
		float4 col:COLOR;
	};

	vertexOutput vert(vertexInput input) {
		vertexOutput outPut;
		float4x4 otw = unity_ObjectToWorld;//物体坐标系转世界坐标系矩阵
		float4x4 wto = unity_WorldToObject;//世界坐标系转物体坐标系矩阵
		//float3 normalDir = normalize(mul(float4(input.normal, 0.0), wto).xyz);
			float3 normalDir = UnityObjectToWorldNormal(input.normal);
			float3 viewDir = normalize(_WorldSpaceCameraPos - mul(otw, input.vertex).xyz);
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
			float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb *_Color.rgb;

			float3 diffuseReflection = attenuation * _LightColor0.rgb *_Color.rgb*max(0.0, dot(normalDir, lightDir));

			float3 specularReflection;
			if (dot(normalDir, lightDir) < 0.0) {
				specularReflection = float3(0.0, 0.0, 0.0);
			}
			else {
				specularReflection = attenuation * _LightColor0.rgb *_SpecColor * pow(max(0.0, dot(reflect(-lightDir, normalDir), viewDir)), _Shininess);
			}
			outPut.col = float4(ambient + diffuseReflection + specularReflection, 1.0);
			outPut.pos = UnityObjectToClipPos(input.vertex);
			return outPut;
		}
		float4 frag(vertexOutput input) :COLOR{
			return input.col;
		}
			ENDCG
			}

			 Pass {
		 Tags { "LightMode" = "ForwardAdd" }
		 // pass for additional light sources
	  Blend One One // additive blending 

	  CGPROGRAM

	  #pragma vertex vert  
	  #pragma fragment frag 

	  #include "UnityCG.cginc"
	  uniform float4 _LightColor0;
		// color of light source (from "Lighting.cginc")

	 // User-specified properties
	 uniform float4 _Color;
	 uniform float4 _SpecColor;
	 uniform float _Shininess;

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
		float3x3 modelMatrixInverse = unity_WorldToObject;
		float3 normalDirection = normalize(
		   mul(input.normal, modelMatrixInverse));
		float3 viewDirection = normalize(_WorldSpaceCameraPos
		   - mul(modelMatrix, input.vertex).xyz);
		float3 lightDirection;
		float attenuation;

		if (0.0 == _WorldSpaceLightPos0.w) // directional light?
		{
		   attenuation = 1.0; // no attenuation
		   lightDirection = normalize(_WorldSpaceLightPos0.xyz);
		}
		else // point or spot light
		{
		   float3 vertexToLightSource = _WorldSpaceLightPos0.xyz
			  - mul(modelMatrix, input.vertex).xyz;
		   float distance = length(vertexToLightSource);
		   attenuation = 1.0 / distance; // linear attenuation 
		   lightDirection = normalize(vertexToLightSource);
		}

		float3 diffuseReflection =
		   attenuation * _LightColor0.rgb * _Color.rgb
		   * max(0.0, dot(normalDirection, lightDirection));

		float3 specularReflection;
		if (dot(normalDirection, lightDirection) < 0.0)
			// light source on the wrong side?
		 {
			specularReflection = float3(0.0, 0.0, 0.0);
			// no specular reflection
	  }
	  else // light source on the right side
	  {
		 specularReflection = attenuation * _LightColor0.rgb
			* _SpecColor.rgb * pow(max(0.0, dot(
			reflect(-lightDirection, normalDirection),
			viewDirection)), _Shininess);
	  }

	  output.col = float4(diffuseReflection
		 + specularReflection, 1.0);
	  // no ambient contribution in this pass
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
