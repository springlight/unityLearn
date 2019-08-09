#if !defined(MY_LIGHTING_INCLUDED)

#define MY_LIGHTING_INCLUDED
#include "AutoLight.cginc"
#include "UnityPBSLighting.cginc"
sampler2D _MainTex;
float4 _MainTex_ST;
float4 _Tint;
float _Smoothness;
//float4 _SpecularTint;
float _Metallic;
struct a2v {
	float4 pos:POSITION;
	float3 normal:NORMAL;
	float2 uv:TEXCOORD0;

};
struct v2f {
	float4 pos:SV_POSITION;
	float2 uv:TEXCOORD0;
	float3 normal:TEXCOORD1;
	float3 worldPos:TEXCOORD2;
};

v2f vert(a2v v) {
	v2f o;
	o.pos = UnityObjectToClipPos(v.pos);

	o.uv = v.uv*_MainTex_ST.xy + _MainTex_ST.zw;
	o.worldPos = mul(unity_ObjectToWorld, v.pos);
	//o.normal = mul(unity_ObjectToWorld,float4(v.normal,0));
	o.normal = mul(transpose((float3x3)unity_WorldToObject), v.normal);
	o.normal = normalize(o.normal);
	return o;
}

UnityLight CreateLight(v2f i) {
	UnityLight light;
#if defined(POINT)||defined(POINT_COOKIE)||defined(SPOT)
		light.dir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
#else
		light.dir = _WorldSpaceLightPos0.xyz;
#endif
	//float3 lightVec = _WorldSpaceLightPos0.xyz - i.worldPos;
	//float attenuation = 1 / (1+ dot(lightVec, lightVec));
	UNITY_LIGHT_ATTENUATION(attenuation, 0, i.worldPos);
	light.color = _LightColor0.rgb * attenuation;
	light.ndotl = DotClamped(i.normal, light.dir);
	return light;
}

float4 frag(v2f i) :SV_Target{
	i.normal = normalize(i.normal);

//float3 lightDir = _WorldSpaceLightPos0.xyz;
float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

float3 albedo = tex2D(_MainTex, i.uv).rgb*_Tint.rgb;

float3 specularTint;// = albedo * _Metallic;
float oneMinusReflectivity;// = 1 - _Metallic;
albedo = DiffuseAndSpecularFromMetallic(albedo, _Metallic, specularTint, oneMinusReflectivity);

//UnityLight light;
//light.color = lightColor;
//light.dir = lightDir;
//light.ndotl = DotClamped(i.normal, lightDir);

UnityIndirect indirectLight;
indirectLight.diffuse = 0;
indirectLight.specular = 0;

return UNITY_BRDF_PBS(
albedo,specularTint, oneMinusReflectivity, _Smoothness,i.normal,viewDir,CreateLight(i), indirectLight);

}
#endif