// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Study/FirstLight"{
		Properties{
			_Tint("Tint",Color)=(1,1,1,1)
			_MainTex("Albedo",2D)="white"{}
			_Smoothness("Smoothness",Range(0,1)) = 0.5
		   _SpecularTint("Specular",Color) = (0.5,0.5,0.5)
		}

		SubShader{
			Pass{
			Tags{"LightMode" = "ForwardBase"}
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
		/*	#include "UnityCG.cginc"*/
			#include "UnityStandardBRDF.cginc"
			sampler2D _MainTex;
	float4 _MainTex_ST;
	float4 _Tint;
	float _Smoothness;
	float4 _SpecularTint;
		struct a2v {
			float4 pos:POSITION;
			float3 normal:NORMAL;
			float2 uv:TEXCOORD0;

		};
	struct v2f {
		float4 pos:SV_POSITION;
		float2 uv:TEXCOORD0;
		float3 normal:TEXCOORD1;
		float3 worldPos:TEXCOORD2;
		};

		v2f vert(a2v v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.pos);

			o.uv = v.uv*_MainTex_ST.xy + _MainTex_ST.zw;
			o.worldPos = mul(unity_ObjectToWorld, v.pos);
			//o.normal = mul(unity_ObjectToWorld,float4(v.normal,0));
			o.normal = mul(transpose((float3x3)unity_WorldToObject), v.normal);
			o.normal = normalize(o.normal);
			return o;
		}
		float4 frag(v2f i) :SV_Target{
			i.normal = normalize(i.normal);

		float3 lightDir = _WorldSpaceLightPos0.xyz;
		float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
		float3 lightColor = _LightColor0.rgb;
		float3 albedo = tex2D(_MainTex, i.uv).rgb*_Tint.rgb;
		float3 diffuse =albedo * lightColor * DotClamped(lightDir, i.normal);
	//	float3 reflectionDir = reflect(-lightDir, i.normal);
		float3 halfVector = normalize(lightDir + viewDir);
		float3 specular = _SpecularTint.rgb*lightColor * pow(DotClamped(halfVector, i.normal), _Smoothness * 100);
		return  float4(specular, 1);
	//	return  pow(DotClamped(viewDir,reflectionDir), _Smoothness*100);
		//return max(0,dot(float3(0, 1, 0), i.normal));
		}
		ENDCG
		}
	}

}