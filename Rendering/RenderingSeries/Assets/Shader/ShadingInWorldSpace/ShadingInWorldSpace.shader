Shader "Study/ShadingInWorldSpace"
{
	Properties{
			_Point("a point in world sapce",Vector) = (0.0,0.0,0.0,1.0)
			_DistanceNear("threshold distance",Float) = 5.0
			_ColorNear("color near to point",Color) = (0.0,1.0,0.0,1.0)
			_ColorFar("color far from point",Color) = (0.3,0.3,0.3,1.0)
}
	SubShader
	{
		
		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		float4 _Point;
	float _DistanceNear;
	float4 _ColorNear;
	float4 _ColorFar;
		struct vertexInput
		{
			float4 vertex:POSITION;
		};
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 pos_in_world_space:TEXCOORD0;
		};

	vertexOutput vert(vertexInput input) {
		vertexOutput output;
		output.pos = UnityObjectToClipPos(input.vertex);
		output.pos_in_world_space = mul(unity_ObjectToWorld, input.vertex);
		return output;
	}
	float4 frag(vertexOutput input):COLOR {
		float dist = distance(input.pos_in_world_space,_Point);
		if (dist <_DistanceNear) {
			return _ColorNear;
		}
		else {
			return _ColorFar;
		}
	}
		ENDCG
		}
		
	}
	
}
