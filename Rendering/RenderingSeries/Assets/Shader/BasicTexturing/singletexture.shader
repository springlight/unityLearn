Shader "Study/singletexture"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

	}
	SubShader
	{


		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct vertexInput {
				float4 vertex:POSITION;
				float2 texcoord:TEXCOORD0;
			};
			struct vertexOutput {
				float4 pos:SV_POSITION;
				float2 tex:TEXCOORD0;
			};


			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			vertexOutput vert (vertexInput v)
			{
				vertexOutput o;
				o.tex = v.texcoord;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (vertexOutput i) : SV_Target
			{
				return tex2D(_MainTex,_MainTex_ST.xy * i.tex.xy + _MainTex_ST.zw);
			//return tex2D(_MainTex, TRANSFORM_TEX(i.tex, _MainTex));
			}
			ENDCG
		}
	}
}
