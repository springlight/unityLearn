// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Study/Norm Map In Tangent Space2"{

	Properties{
		_Color("Color Tint",Color) = (1,1,1,1)
		_MainTex("MainTex",2D)= "white"{}
		_BumpMap("Normal Map",2D)= "bump"{}//法线纹理
		_BumpScale("Bump Scale",Float) = 1.0//控制凸凹程度
		_Specular("Specular",Color) = (1,1,1,1)
		_Gloss("Gloss",Range(8.0,256)) = 20
	}

		SubShader{
			Pass{
				Tags{"LightMode" = "ForwardBase"}
				CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "Lighting.cginc"
			fixed4 _Color;
		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _BumpMap;
		float4 _BumpMap_ST;
		float _BumpScale;
		fixed4 _Specular;
		float _Gloss;
		struct a2v {
			float4 vertex:POSITION;
			float3 normal:NORMAL;
			float4 tangent:TANGENT;//顶点切线
			float4 texcoord:TEXCOORD0;
		};
		struct v2f {
			float4 pos:SV_POSITION;
			float4 uv:TEXCOORD0;
			float3 lightDir:TEXCOORD1;
			float3 viewDir:TEXCOORD2;
		};

		v2f vert(a2v v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			//因为有两张纹理，所以uv改成了float4
			o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
			o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
			float3 binormal = cross(normalize(v.normal), normalize(v.tangent.xyz))*v.tangent.w;
			float3x3 rotation = float3x3(v.tangent.xyz, binormal, v.normal);
			//模型空间下的光线和视觉转移到切线空间
			o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex)).xyz;
			o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex)).xyz;
			return o;
		}
		float4 frag(v2f i) :SV_Target{
			fixed3 tangentLightDir = normalize(i.lightDir);
			fixed3 tangentViewDir = normalize(i.viewDir);
			//法线纹理，转化成法线坐标
			fixed4 packedNormal = tex2D(_BumpMap, i.uv.zw);
			fixed3 tangentNormal = UnpackNormal(packedNormal);

			fixed3 albedo = tex2D(_MainTex, i.uv).rgb*_Color.rgb;
			fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz*albedo;
			fixed3 diffuse = _LightColor0.rgb *albedo*max(0, dot(tangentNormal, tangentLightDir));
			fixed3 halfDir = normalize(tangentLightDir + tangentViewDir);
			fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0, dot(tangentNormal, halfDir)), _Gloss);
			return fixed4(ambient + diffuse + specular, 1.0);
			

		}
				ENDCG
			}
		}
}