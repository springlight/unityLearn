Shader "Study/Cg semitransparent colors based on alpha"
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
					if (texColor.a > 0.5) {
						texColor = float4(0.0, 0.0, 1.2, 1.0);
					}
					else {
						texColor = float4(0.0, 0.0, 1.0, 0.5);
					}

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
				if (texColor.a > 0.5) {
					texColor = float4(0.0, 1.0, 0.0, 1.0);
				}
				else {
					texColor = float4(0.0, 0.0, 1.0, 0.3);
				}

				return texColor;
			}
				ENDCG
			}
	}
}
