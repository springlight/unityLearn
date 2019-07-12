// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/My Fisrt Shader"{
	Properties{
		_Tint("Tint",Color) = (1,1,1,1)
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
	struct Interpolators {
		float4 position:SV_POSITION;
		float3 localPosition:TEXCOORD0;
		};
			//SV_POSITION SV:System Value
	Interpolators MyVertexProgram(float4 position:POSITION){
		    Interpolators i ;
				i.localPosition = position.xyz;
				i.position = UnityObjectToClipPos(position);
				return i;
			}
		//TARGET ；default shader target ,this is frame buffer
		//顶点着色器的输出是片段着色器的输入
		float4 MyFragmentProgram(Interpolators i) :SV_TARGET{
				return float4(i.localPosition + 0.5,1)* _Tint;
			}
			ENDCG
		}
	}

}