Shader "Study/LayersofTextures"
{
	Properties
	{
		_DecalTex("Daytime",2D) = "white"{}
		_MainTex("Image",2D) = "white"{}
		_Color("Color",Color) = (1,1,1,1)

	}
		SubShader{
			Tags{"LightMode" = "ForwardBase"}

			Pass{
		
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
			 #include "UnityCG.cginc"
				struct vertexInput {
					float4 vertex:POSITION;
					float3 normal:NORMAL;
					float4 texcoord:TEXCOORD0;

				};
					sampler2D _MainTex;
					sampler2D _DecalTex;
					float4 _Color;
					float4 _LightColor0;
			struct vertexOutput
			{
				float4 pos:SV_POSITION;
			
				float4 tex:TEXCOORD0;
				float levelOfLighting : TEXCOORD1;
			};
			vertexOutput vert(vertexInput input) {
				vertexOutput output;

				float3 normalDir = normalize(mul(float4(input.normal, 0.0), unity_WorldToObject).xyz);
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				output.levelOfLighting = max(0.0, dot(normalDir, lightDir));

					output.tex = input.texcoord;
			
					output.pos = UnityObjectToClipPos(input.vertex);
					return output;
				}
				float4 frag(vertexOutput input) :COLOR
				{
					float4 nightColor = tex2D(_MainTex,input.tex.xy)*_Color;
					float4 dayTimeColor = tex2D(_DecalTex, input.tex.xy) * _LightColor0;

					return lerp(nightColor, dayTimeColor, input.levelOfLighting);

				
				}
				ENDCG
			}

	
	}
}
