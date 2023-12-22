Shader "WataOfuton/RainbowPolyRing/RainbowPolyRing"
{
Properties
{
    [Header(Shape Settings)]
	_PolySize("Poly Size", float) = 1.0
	_Thickness("Thickness", Range(0.0,1.0)) = 0.0
	_wav("Wave Power", Range(0.0,1.0)) = 0.2
	_r2s("Ring to Sphere", Range(0.0,1.0)) = 0.1

    [Header(Color Settings)]
	[ToggleUI]_UseTwoColor("Use Two Color Mode", float) = 0.0
	[ToggleUI]_MixMode("Color Mix Mode", float) = 0.0
	_Color1("Color 1", color) = (1,1,1,1)
	_Color2("Color 2", color) = (1,1,1,1)
	_Polymix("Poly Color Mix Rate", Range(0.0,1.0)) = 0.0
	_vivid("Poly Color Vivid", float) = 2.2
	_Brightness("Brightness", Range(0.0,2.0)) = 1.0

    [Header(Other Settings)]
	[ToggleUI]_lineOn("Lines Switch", float) = 1
    [HDR]_LineCol("Lines Color", Color) = (1.0, 1.0, 1.0, 1.0)
	[ToggleUI]_lightOn("Lighting", float) = 1
    _minLighting("Min Lighting", Range(0, 1)) = 0.1
    _OLWidth("OutLine Width", Float) = 0.05
    [HDR]_OLCol("OutLine Color", Color) = (1.0, 1.0, 1.0, 1.0)
}
SubShader
{
Tags{
    "Queue" = "Geometry"
    "RenderType"="Opaque"
    "DisableBatching" = "True"
    // "IgnoreProjector" = "True"
    }
Cull Off

Pass
{
Tags { "LightMode"="ForwardBase" }
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
// #pragma multi_compile_fwdbase_fullshadows
#pragma multi_compile_fog
#pragma multi_compile_instancing
#include "UnityCG.cginc"

#include "PolyRingCore.cginc"

ENDCG
}

Pass{
Tags{ "LightMode" = "ShadowCaster" }
Offset 1, 1
CGPROGRAM
#pragma vertex vert
#pragma fragment frag_shadow
#pragma fragmentoption ARB_precision_hint_fastest
#pragma multi_compile_shadowcaster
#pragma multi_compile_instancing

#define SHADOWCASTER
#include "PolyRingCore.cginc"

ENDCG
}
}
}
