#if !defined(MY_LIGHTING_INCLUDED)
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members tangent)
#pragma exclude_renderers d3d11

#define MY_LIGHTING_INCLUDED
#include "AutoLight.cginc"
#include "UnityPBSLighting.cginc"
sampler2D _MainTex;
float4 _MainTex_ST;
sampler2D _NormalMap;
//sampler2D _HeightMap;
//float4 _HeightMap_TexelSize;
float4 _Tint;
float _Smoothness;
float _Metallic;
float _BumpScale;
sampler2D _DetailTex;
float4 _DetailTex_ST;
sampler2D _DetailNormalMap;
float _DetailBumpScale;
struct a2v {
	float4 pos:POSITION;
	float3 normal:NORMAL;
	float4 tangent:TANGENT;
	float2 uv:TEXCOORD0;

};
struct v2f {
	float4 pos:SV_POSITION;
	//float2 uv:TEXCOORD0;
	float4 uv:TEXCOORD0;
	float3 normal:TEXCOORD1;
#if defined(BINORMAL_PER_FRAGMENT)
	float4 tangent : TEXCOORD2;
#else
	float3 tangent : TEXCOORD2;
	float3 binormal : TEXCOORD3;
#endif

	float3 worldPos : TEXCOORD4;

#if defined(VERTEXLIGHT_ON)
	float3 vertexLightColor : TEXCOORD5;
#endif
};


void ComputeVertexLightColor(inout v2f i) {
#if defined(VERTEXLIGHT_ON)
	i.vertexLightColor = Shade4PointLights(
		unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
		unity_LightColor[0].rgb, unity_LightColor[1].rgb,
		unity_LightColor[2].rgb,unity_LightColor[3].rgb,
		unity_4LightAtten0,i.worldPos,i.normal);

#endif

}

float3 CreateBinormal(float3 normal, float3 tangent, float binormalSign) {
	return cross(normal, tangent.xyz) *
		(binormalSign * unity_WorldTransformParams.w);
}
v2f vert(a2v v) {
	v2f o;
	o.pos = UnityObjectToClipPos(v.pos);

	
	o.worldPos = mul(unity_ObjectToWorld, v.pos);
	//o.normal = mul(unity_ObjectToWorld,float4(v.normal,0));
	o.normal = mul(transpose((float3x3)unity_WorldToObject), v.normal);
	o.normal = normalize(o.normal);
	//Transform the tangent to world space in the vertex program,
#if defined(BINORMAL_PER_FRAGMENT)
	o.tangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
#else
	o.tangent = UnityObjectToWorldDir(v.tangent.xyz);
	o.binormal = CreateBinormal(o.normal, o.tangent, v.tangent.w);
#endif
	o.uv.xy = v.uv*_MainTex_ST.xy + _MainTex_ST.zw;
	o.uv.zw = v.uv*_DetailTex_ST + _DetailTex_ST.zw;
	ComputeVertexLightColor(o);
	return o;
}

UnityLight CreateLight(v2f i) {
	UnityLight light;
#if defined(POINT)||defined(POINT_COOKIE)||defined(SPOT)
		light.dir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
#else
		light.dir = _WorldSpaceLightPos0.xyz;
#endif

	UNITY_LIGHT_ATTENUATION(attenuation, 0, i.worldPos);
	light.color = _LightColor0.rgb * attenuation;
	light.ndotl = DotClamped(i.normal, light.dir);
	return light;
}



UnityIndirect CreateIndirectLight(v2f i) {
	UnityIndirect indirectLight;
	indirectLight.diffuse = 0;
	indirectLight.specular = 0;
#if defined(VERTEXLIGHT_ON)
	indirectLight.diffuse = i.vertexLightColor;
#endif
#if defined(FORWARD_BASE_PASS)
	indirectLight.diffuse += max(0, ShadeSH9(float4(i.normal,1)));
#endif
	return indirectLight;
}

void InitializeFragmentNormal(inout v2f i) {
	/*
	i.normal.xy = tex2D(_NormalMap, i.uv).wy * 2 - 1;
	i.normal.xy *=  _BumpScale;
	i.normal.z = sqrt(1 - saturate(dot(i.normal.xy, i.normal.xy)));*/

	float3 mainNormal = UnpackScaleNormal(tex2D(_NormalMap, i.uv.xy), _BumpScale);

	float3 detailNormal = UnpackScaleNormal(tex2D(_DetailNormalMap, i.uv.zw), _DetailBumpScale);
	
	
	float3 tangentSpaceNormal = BlendNormals(mainNormal, detailNormal);
#if defined(BINORMAL_PER_FRAGMENT)
	float3 binormal = CreateBinormal(i.normal, i.tangent.xyz, i.tangent.w);
#else
	float3 binormal = i.binormal;
#endif
	/*i.normal = tex2D(_NormalMap, i.uv).xyz * 2 - 1;
	i.normal = i.normal.xzy;
*/

	/*高度贴图代码
	float2 du = float2(_HeightMap_TexelSize.x*0.5, 0);

	//float h1 = tex2D(_HeightMap, i.uv);
	float h1 = tex2D(_HeightMap, i.uv - du);
	float h2 = tex2D(_HeightMap, i.uv + du);

	//float3 tu = float3(1, h2 - h1, 0);
	//i.normal = float3(1,(h2-h1)/du.x,0);
	//i.normal = float3(du.x, (h2 - h1), 0);
	//1，切线向量
	//i.normal = float3(1, (h2 - h1), 0);
	//切线向量绕着z轴旋转90度，得到upwards方向
	//i.normal = float3(h1-h2,1, 0);
	


	float2 dv = float2(0, _HeightMap_TexelSize.y *0.5);
	float v1 = tex2D(_HeightMap, i.uv - dv);
	float v2 = tex2D(_HeightMap, i.uv + dv);

	//float3 tv = float3(0, v2 - v1, 1);
	//i.normal = cross(tv, tu);
	i.normal = float3(h1 - h2, 1, v1 - v2);
	i.normal = normalize(i.normal);
	//i.normal = float3(0, 1, v1 - v2);
	//i.normal = normalize(i.normal);*/
}

float4 frag(v2f i) :SV_Target{
	
	InitializeFragmentNormal(i);

float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

float3 albedo = tex2D(_MainTex, i.uv.xy).rgb*_Tint.rgb;
albedo *= tex2D(_DetailTex, i.uv.zw) *unity_ColorSpaceDouble;
float3 specularTint;
float oneMinusReflectivity;
albedo = DiffuseAndSpecularFromMetallic(albedo, _Metallic, specularTint, oneMinusReflectivity);

return UNITY_BRDF_PBS(
albedo,specularTint, oneMinusReflectivity, _Smoothness,i.normal,viewDir,CreateLight(i), CreateIndirectLight(i));

}
#endif