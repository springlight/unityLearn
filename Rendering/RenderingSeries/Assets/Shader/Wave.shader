// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Study/Wave"
{
	Properties
	{
		_MainTex("MainTex",2D) = "white"{}
		_Frequncy("Frenquncy",Float) = 1
		_Speed("Speed",Float) = 1
		_Hight("Hight",Float) = 1
	}
		SubShader
		{

			Pass
			{
				Cull off
				
			CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members uv)
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
				
		sampler2D _MainTex;
		float4 _MainTex_ST;
		float _Frequncy;
		float _Speed;
		float _Hight;
		struct a2v 
		{
			float4 pos:POSITION;
			float2 uv:TEXCOORD0;

		};
		struct v2f {
			float4 pos:SV_POSITION;
			float2 uv:TEXCOORD0;
		};
		v2f vert(a2v i) {
			v2f o;

			float distance = _Time.y * _Speed;//移动距离
			float offset = _Hight * sin(_Frequncy*i.pos.x + distance);
			i.pos.y += offset;
			o.pos = UnityObjectToClipPos(i.pos);
			
			 
			o.uv = i.uv;// *_MainTex_ST.xy + _MainTex_ST.zw;
			return o;
		}
		float4 frag(v2f i):SV_Target {

			float4 color = tex2D(_MainTex, i.uv);
			return color;
		}
		ENDCG
		}
	}
}