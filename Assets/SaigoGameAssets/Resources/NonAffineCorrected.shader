
// NonAffineCorrected Shader
// =========================
// Map the triangle-mapped texture to arbitrary four-edge quad.
// Works with the transparent texture. The shader is a HLSL/Cg
// version of Jessy's original GLSL work. The GLSL code may not
// compile in some unknown circumstances, while the Cg version
// seems more reliable.
//
// http://forum.unity3d.com/threads/correcting-affine-texture-mapping-for-trapezoids.151283/#post-1036716
//
// Yue Marvin Tao

Shader "NonAffineCorrected" {
		
Properties {
	_MainTex ("Color (RGB) Alpha (A)", 2D) = "white" {}
	_Offset ("Offset", Vector) = (0.0, 0.0, 0.0, 0.0)
}

SubShader {

    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    Blend SrcAlpha OneMinusSrcAlpha
	LOD 200

Pass {

//	Cull Off
	
	CGPROGRAM

	#pragma vertex Vert
	#pragma fragment Frag

	#include "UnityCG.cginc"

	struct VertIn {
		float4 v : POSITION;
		float4 uv : TEXCOORD0;
		float4 uv2 : TEXCOORD1;
	};

	struct VertOut {
		float4 v : SV_POSITION;
		float4 uv : TEXCOORD0;
		float4 uv2 : TEXCOORD1;
	};

	float2 Shifted, Dimension;

	VertOut Vert(VertIn i) {
		VertOut o;
		o.uv = i.uv;
		o.uv2 = i.uv2;
		o.v = mul(UNITY_MATRIX_MVP, i.v);
	
		return o;
	}

	sampler2D _MainTex;
	float4    _Offset;

	float4 Frag(VertOut i) : COLOR{

		Shifted   = i.uv.xy;
		Dimension = i.uv2.xy;

		float4 finalColor = tex2D(_MainTex, Shifted / Dimension + _Offset.xy);

		return finalColor;
	}

	ENDCG

}}

}