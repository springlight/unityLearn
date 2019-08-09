

Shader "Study/Laoding"
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
					//把uv坐标移动到原点
					uv -= float2(0.5, 0.5);
					if (length(uv) > 0.5) {
						return float4(0, 0, 0, 0);
					}
					float2 finalUV = 0;
					//旋转角度
					float angle = _Time.x * 20;
					//旋转uv坐标
					finalUV.x = uv.x * cos(angle) - uv.y*sin(angle);
					finalUV.y = uv.x*sin(angle) + uv.y*cos(angle);
					//再次平移uv坐标，还原第一步操作
					finalUV += float2(0.5, 0.5);
					return tex2D(_MainTex, finalUV);
				}
				ENDCG
			}
	}
}