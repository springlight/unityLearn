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
			//SV_POSITION SV:System Value
		float4 MyVertexProgram(float4 position:POSITION) :SV_POSITION{
				return UnityObjectToClipPos(position);
			}
		//TARGET ；default shader target ,this is frame buffer
		//顶点着色器的输出是片段着色器的输入
		float4 MyFragmentProgram(float4 position:SV_POSITION) :SV_TARGET{
				return _Tint;
			}
			ENDCG
		}
	}

}