Shader "Study/SilhousetteEnhancement"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{

		Pass
		{
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
#include "UnityCG.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos:SV_POSITION;
				float3 normal:TEXCOORD0;
				float3 viewDir:TEXCOORD1;
				float2 uv : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;
				o.normal = normalize(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
				o.viewDir = normalize(_WorldSpaceCameraPos - mul(modelMatrix, v.vertex).xyz);
				

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
			float3 normalDir = normalize(i.normal);
			float3 viewDir = normalize(i.viewDir);
			float newOpacity = min(1.0, col.a / abs(dot(viewDir, normalDir)));

				return float4(col.rgb,newOpacity);
			}
			ENDCG
		}
	}
}
