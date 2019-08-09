// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Study/FirstLight"{
		Properties{
			_Tint("Tint",Color)=(1,1,1,1)
			_MainTex("Albedo",2D)="white"{}
			_Smoothness("Smoothness",Range(0,1)) = 0.5
			[Gamma]_Metallic("Metallic",Range(0,1)) = 0
		  // _SpecularTint("Specular",Color) = (0.5,0.5,0.5)
		}

		SubShader{
			Pass{
			Tags{"LightMode" = "ForwardBase"}
			CGPROGRAM
#pragma target 3.0
	#pragma vertex vert
	#pragma fragment frag
		/*	#include "UnityCG.cginc"*/
			#include "UnityStandardBRDF.cginc"
#include "UnityStandardUtils.cginc"
#include "MyLighting.cginc"
		ENDCG
		}
	}

}