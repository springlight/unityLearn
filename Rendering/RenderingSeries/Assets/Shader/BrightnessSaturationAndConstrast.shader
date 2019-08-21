﻿Shader "Unlit/BrightnessSaturationAndConstrast"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Brightness("Brightness",Float) = 1
		_Saturation("Saturation",Float) = 1
		_Contrast("Contrast",Float) = 1
	}
		SubShader
		{


			Pass
			{
				ZTest Always Cull Off Zwrite Off
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				sampler2D _MainTex;
		half _Brightness;
		half _Saturation;
		half _Contrast;
	/*		struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};*/

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				
				
			};

			
			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
			
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 renderTex = tex2D(_MainTex, i.uv);
			fixed3 finalColor = renderTex.rgb * _Brightness;
			fixed luminance = 0.2125 *renderTex.r + 0.7154 *renderTex.g + 0.0721*renderTex.b;
			fixed3 luminanceColor = fixed3(luminance, luminance, luminance);
			finalColor = lerp(luminanceColor, finalColor, _Saturation);

			fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
			finalColor = lerp(avgColor, finalColor, _Contrast);
			
			
				return fixed4(finalColor,renderTex.a);
			}
			ENDCG
		}
	}
			Fallback Off
}
