

Shader "Study/Flow"
{
	Properties{
		_MainTex("MainTex",2D) = "white"{}
		}

	SubShader{
		Pass
		{
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
			struct a2v
			{
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
			};
			struct v2f
			{
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
			};
			sampler2D _MainTex;
			float4 _MainTex_ST;
			v2f vert(a2v i) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(i.pos);
				o.uv = i.uv*_MainTex_ST + _MainTex_ST.zw;
				return o;
			}

			float4 frag(v2f i) :SV_Target{

				float2 uv = i.uv;
				uv.x += _Time.x;
				uv.y += _Time.y;
				return tex2D(_MainTex, uv);
			}
			ENDCG
		}
	}
}