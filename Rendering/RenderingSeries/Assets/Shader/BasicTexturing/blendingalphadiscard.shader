Shader "Study/blendingAlphaDiscard"
{
	Properties
	{
		_MainTex("Image",2D) = "white"{}
		
	}
		SubShader{
			Tags{"Queue" = "Transparent"}

			Pass{
				Cull Front
				ZWrite Off
				Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
				struct vertexInput {
					float4 vertex:POSITION;
					float4 texcoord:TEXCOORD0;

				};
					sampler2D _MainTex;
		
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
					
					return texColor;
				}
				ENDCG
			}

		Pass{
				Cull Back
				ZWrite Off
				Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
				struct vertexInput {
					float4 vertex:POSITION;
					float4 texcoord:TEXCOORD0;

				};
					sampler2D _MainTex;

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

					return texColor;
				}
				ENDCG
			}
		}
}
