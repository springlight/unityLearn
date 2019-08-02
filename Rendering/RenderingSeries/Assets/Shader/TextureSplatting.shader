Shader "Study/Texture Splatting"{
	Properties{
		_MainTex("Splat Map",2D) = "white"{}
		[NoScaleOffset]_Texture1("Texture1",2D)="white"{}
		[NoScaleOffset]_Texture2("Texture2",2D)="white"{}
		[NoScaleOffset]_Textrue3("Texture3",2D)="white"{}
		[NoScaleOffset]_Texture4("Texture4",2D)="white"{}
	}
		SubShader{
			Pass{
				CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"
		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _Texture1, _Texture2,_Texture3,_Texture4;
		struct a2v {
			float4 pos:POSITION;
			float2 uv:TEXCOORD0;
		 };
		struct v2f {
			float4 pos:SV_POSITION;
			float2 uv:TEXCOORD0;
			float2 uvSplat:TEXCOORD1;

		};

		v2f vert(a2v v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.pos);
			o.uv = v.uv *_MainTex_ST.xy + _MainTex_ST.zw;
			o.uvSplat = v.uv;

			return o;
		}

		float4 frag(v2f i) :SV_Target {
			float4 splat = tex2D(_MainTex,i.uvSplat);
			//值1代表第一个纹理。由于我们的splat映射是单色的，我们可以使用任何RGB通道来检索这个值。我们用R通道把它和纹理相乘
		return tex2D(_Texture1,i.uv)*splat.r + 
			tex2D(_Texture2,i.uv)*splat.g +
			tex2D(_Texture3,i.uv)*splat.b +
			tex2D(_Texture4,i.uv)*(1-splat.r-splat.g-splat.b);
		}
		ENDCG
			}
	}

	
}