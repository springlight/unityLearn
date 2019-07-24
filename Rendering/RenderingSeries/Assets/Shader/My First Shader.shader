// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/My Fisrt Shader"{
	Properties{
		_Tint("Tint",Color) = (1,1,1,1)
		_MainTex("Texture",2D) = "white"{}
		//_Int("MyInt",Int) = 2
	 //  _Float("MyFloat", Float) = 1.5
		//_Range("MyRange", Range(0.0, 5.0)) = 3.0
		//_Vector("MyVector", Vector) = (2, 3, 6, 1)
		//_2D("My2D",2D) = ""{}
		
	}
		SubShader
	{
		Pass
		{
			CGPROGRAM
#pragma vertex MyVertexProgram

#pragma fragment MyFragmentProgram
#include "UnityCG.cginc"
		float4 _Tint;
	sampler2D _MainTex;
	float4 _MainTex_ST;
	struct Interpolators {
		float4 position:SV_POSITION;
		float2 uv:TEXCOORD0;
		//float3 localPosition:TEXCOORD0;
		};
	struct VertexData {
		float4 position:POSITION;
		float2 uv:TEXCOORD0;
	};
			//SV_POSITION SV:System Value
	Interpolators MyVertexProgram(VertexData v) {
		Interpolators i;
		//i.localPosition = v.position.xyz;

		i.position = UnityObjectToClipPos(v.position);
		i.uv = v.uv*_MainTex_ST.xy + _MainTex_ST.zw;;
		return i;
	}
	//Interpolators MyVertexProgram(float4 position:POSITION){
	//	    Interpolators i ;
	//			i.localPosition = position.xyz;
	//			i.position = UnityObjectToClipPos(position);
	//			return i;
	//		}
		//TARGET ；default shader target ,this is frame buffer
		//顶点着色器的输出是片段着色器的输入
		float4 MyFragmentProgram(Interpolators i) :SV_TARGET{
				//return float4(i.uv,1,1);
			
			return tex2D(_MainTex, i.uv)*_Tint;
			}
			ENDCG
		}
	}

}