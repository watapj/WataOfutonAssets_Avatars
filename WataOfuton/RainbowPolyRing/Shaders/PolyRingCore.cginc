/* PolyRingCore.cginc */

// Reference URLs
// https://github.com/lilxyzw/Shader-MEMO/blob/main/Assets/SPSITest.shader
// https://www.shadertoy.com/view/WssyDX
// https://wgld.org/d/glsl/g017.html

#include "UnityCG.cginc"

uniform float _PolySize;
uniform float _Polymix;
uniform float _Thickness;
uniform float _wav;
uniform float _r2s;
#ifdef _ALPHABLEND_ON
uniform float _Alpha;
#endif

#if !defined(SHADOWCASTER)
#include "Lighting.cginc"
#include "AutoLight.cginc"

uniform float _UseTwoColor;
uniform float _MixMode;
uniform float4 _Color1, _Color2;
uniform float _Brightness;
uniform float _vivid;
uniform float _lineOn;
uniform float4 _LineCol;
uniform float _lightOn;
uniform float _minLighting;
uniform float _OLWidth;
uniform float4 _OLCol;

#else

// Reference "UnityStandardShadow.cginc"
#include "UnityStandardUtils.cginc"

#if (defined(_ALPHABLEND_ON) || defined(_ALPHAPREMULTIPLY_ON)) && defined(UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS)
	#define UNITY_STANDARD_USE_DITHER_MASK 1
#endif
#define UNITY_STANDARD_USE_SHADOW_UVS 1
#define UNITY_STANDARD_USE_SHADOW_OUTPUT_STRUCT 1

#ifdef UNITY_STANDARD_USE_DITHER_MASK
sampler3D   _DitherMaskLOD;
#endif
#endif


float mod (float  a, float  b){ return a-b*floor(a/b);}
float mod3(float3 a, float3 b){ return a-b*floor(a/b);}
float2 rot2d(float2 p, float r){
	return mul(float2x2(cos(r),-sin(r),sin(r),cos(r)), p);
}
float3 hash33(uint3 n) {
    uint3 k = uint3(0x456789abu, 0x6789ab45u, 0x89ab4567u);
    n ^= (n.yzx << 9);
    n ^= (n.yzx >> 1);
    n *= k;
    n ^= (n.yzx << 1);
    n *= k;
    return float3(n) / float(0xffffffffu);
}
float3 rotate(float3 p, float angle, float3 axis){
	float3 a = normalize(axis);
	float s = sin(angle);
	float c = cos(angle);
	float r = 1.0 - c;
	float3x3 m = float3x3(
		a.x * a.x * r + c,
		a.y * a.x * r + a.z * s,
		a.z * a.x * r - a.y * s,
		a.x * a.y * r - a.z * s,
		a.y * a.y * r + c,
		a.z * a.y * r + a.x * s,
		a.x * a.z * r + a.y * s,
		a.y * a.z * r - a.x * s,
		a.z * a.z * r + c
	);
	return mul(m , p);
}

struct appdata
{
	float4 vertex : POSITION;
	// float2 uv : TEXCOORD0;
    float4 color : COLOR;
	float3 normal : NORMAL;
#if !defined(SHADOWCASTER)
	uint vid : SV_VertexID;
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    #if !defined(SHADOWCASTER)
	float4 pos : SV_POSITION;
	// float2 uv : TEXCOORD0;
    nointerpolation float4 color : COLOR;
	float3 normal : NORMAL;
	float3 lpos : TEXCOORD1;
	float3 bary : TEXCOORD2;
	UNITY_FOG_COORDS(4)
    // SHADOW_COORDS(5)
    #endif
	
#ifdef UNITY_STANDARD_USE_SHADOW_OUTPUT_STRUCT
    V2F_SHADOW_CASTER_NOPOS
#endif
	// UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO
};

v2f vert(appdata v
	#if defined(SHADOWCASTER)
		, out float4 opos : SV_POSITION
	#endif
	)
{
    v2f o = (v2f)0;
    UNITY_SETUP_INSTANCE_ID(v);
    // UNITY_TRANSFER_INSTANCE_ID(v, o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

    float3 offs = v.color.rgb*2.0-1.0;
    float3 p = v.vertex.xyz - offs;
	p *= _PolySize;
	
    float3 r0 = hash33(asuint(v.color.rgb));

	float3 jiku = lerp(v.normal, r0*2.0-1.0, sin(_Time.x)*0.5+0.5);
    p = rotate(p, _Time.y, jiku);

	float th = r0.x * UNITY_TWO_PI + _Time.y/2.0 * (r0.y+0.2);
	float t = r0.z*2.0-1.0;
	float3 s = float3(cos(th), t*_r2s, sin(th));
	s.xz *= lerp(1.0, sqrt(1.0 - t*t), _r2s);
	float rd = dot(r0, 1.0)/3.0;
	s *= lerp(1.0, pow(rd, 0.3333), _Thickness);

	float rcos = cos(_Time.y) * _wav;
	float rsin = sin(_Time.y) * _wav;
	if(r0.x >= 0.5){
		s.xy = rot2d(s.xy, rsin);
		s.yz = rot2d(s.yz, rcos);
	}else{
		s.xy = rot2d(s.xy, rcos);
		s.yz = rot2d(s.yz, rsin);
	}
	s.xz = rot2d(s.xz, _Time.x);

	p += s;
	v.vertex = float4(p, 1.0);

#if defined(SHADOWCASTER)
    TRANSFER_SHADOW_CASTER_NOPOS(o,opos)
	
#else
	o.lpos = p;
    o.pos = UnityObjectToClipPos(v.vertex);
	// o.uv = v.uv;
	o.normal = rotate(v.normal, _Time.y, jiku);

	uint id = v.vid%3u;
	o.bary = float3(id == 0u, id == 1u, id == 2u);
	
	o.color = (r0.x>=0.5) ? _Color1 : _Color2;
	o.color.rgb = _UseTwoColor ? _MixMode ? r0.y<_Polymix ? _Color1 : _Color2 : o.color
                               : cos(float3(1,-1,0)*UNITY_TWO_PI/3.0 + (th + r0.z*_Polymix*UNITY_TWO_PI)) * 0.5 + 0.5;
	o.color.rgb = pow(o.color.rgb, _vivid);
	#ifdef _ALPHABLEND_ON
	o.color.a = _Alpha;
	#else
	o.color.a = 1.0;
	#endif
	
	UNITY_TRANSFER_FOG(o,o.pos);
	// TRANSFER_SHADOW(o)
#endif
	
	return o;
}

#if !defined(SHADOWCASTER)

float4 frag(v2f i) : SV_Target
{
	// UNITY_SETUP_INSTANCE_ID(i);
	// UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

	float4 col = i.color;

	// Lines
	float3 p = rotate(i.lpos, -UNITY_TWO_PI/8, i.normal);
	float line1 = max((mod((p.x+p.y+p.z*2.0)-_Time.y*2.0, 5.0)-4.0)*1.5, 0.0);
	float3 gl1 = abs(mod3(p, 0.07));
	line1 = (gl1.x<0.06 && gl1.z<0.06) ? 0.0 : line1;
	p = rotate(i.lpos, UNITY_TWO_PI/8, i.normal);
	float line2 = max((mod((p.x+p.y+p.z*2.0)-_Time.y*2.0, 5.0)-4.0)*1.5, 0.0);
	float3 gl2 = abs(mod3(p, 0.07));
	line2 = (gl2.x<0.06 && gl2.z<0.06) ? 0.0 : line2;
	float lines = (line1 + line2)*_lineOn;
	
	// OutLine
	col.rgb = any(bool3(i.bary.x < _OLWidth, i.bary.y < _OLWidth, i.bary.z < _OLWidth))
			? _OLCol.rgb : col.rgb;

	// Lighting
	float LightPow = dot(_LightColor0, 1.0)/3.0;
	float3 indirectLighting = ShadeSH9(float4(0,1,0,1));
	float indirectLightingPow = dot(indirectLighting, 1.0)/3.0;
	float power = lerp(LightPow, indirectLightingPow, 1.0-LightPow);
	power = max(power, _minLighting);
	float3 lc = col.rgb * lerp(1.0, power, _lightOn);

	// float4 shadow = SHADOW_ATTENUATION(i);
	// lc.rgb *= lerp(1.0, shadow.xyz, 0.5);

	// Final Color
	float3 LightingCol = saturate(lc - lines);
	col = float4(saturate(LightingCol + lines*_LineCol)*_Brightness, col.a);

	UNITY_APPLY_FOG(i.fogCoord, col);
	return col;
}

#else

half4 frag_shadow (
#ifdef UNITY_STANDARD_USE_SHADOW_OUTPUT_STRUCT
    v2f i
#endif
#ifdef UNITY_STANDARD_USE_DITHER_MASK
    , UNITY_VPOS_TYPE vpos : VPOS
#endif
    ) : SV_Target
{
#ifdef _ALPHABLEND_ON
    #if defined(UNITY_STANDARD_USE_SHADOW_UVS)
        float alpha = _Alpha;
        #if defined(UNITY_STANDARD_USE_DITHER_MASK)
            half alphaRef = tex3D(_DitherMaskLOD, float3(vpos.xy*0.25,alpha*0.9375)).a;
            clip (alphaRef - 0.01);
        #endif
    #endif
#endif

    SHADOW_CASTER_FRAGMENT(i)
}

#endif