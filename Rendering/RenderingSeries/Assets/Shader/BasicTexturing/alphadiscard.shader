Shader "Study/AlphaDiscard"
{
	Properties
	{
		_MainTex("Image",2D) = "white"{}
		_Cutoff("Alpha Cutoff",Float) = 0.5
	}
	SubShader{

		
		Pass{
			Cull Off
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
			struct vertexInput {
			float4 vertex:POSITION;
			float4 texcoord:TEXCOORD0;

		};
				sampler2D _MainTex;
				float _Cutoff;
		struct vertexOutput
		{
			float4 pos:SV_POSITION;
			float4 tex:TEXCOORD0;
		};
		vertexOutput vert(vertexInput input) {
			vertexOutput output;
			output.tex = input.texcoord;
		//	output.tex = input
			output.pos = UnityObjectToClipPos(input.vertex);
			return output;
		}
		float4 frag(vertexOutput input) :COLOR
		{	
			float4 texColor = tex2D(_MainTex,input.tex.xy);
			if (texColor.a < _Cutoff) {
				discard;
			}
			return texColor;
		}
		ENDCG
		}
	}
}
