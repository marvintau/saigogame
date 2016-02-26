Shader "Unlit/ProgressBar" {
	Properties {
		_MainTex ("Color (RGB) Alpha (A)", 2D) = "white" {}
		_Progress ("Ratio of Filling", Vector) = (0.0, 0.0, 0.0, 1.0)
		_Width ("Changed by Boosting", Vector) = (1.0, 0.0, 0.0, 1.0)
		_Opacity ("Opacity", Range(0,1)) = 1
	}

	SubShader {

		Tags {"Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200

		Pass {
			CGPROGRAM

			#pragma vertex Vert
			#pragma fragment Frag

			float _Opacity;
			sampler2D _MainTex;

			#include "UnityCG.cginc"

			#define M_PI 3.1415926535897932384626433832795

			float4 _Progress;

			float4 _Width;

			struct VertIn {
				float4 v: POSITION;
				float4 uv: TEXCOORD0;
				float4 uv2: TEXCOORD1;
			};

			struct VertOut {
				float4 v: POSITION;
				float4 uv: TEXCOORD0;
				float4 uv2: TEXCOORD1;
			};

			// Vertex here only need to be multiplied with
			// the ModelViewProjection matrix, and no more
			// use.
			VertOut Vert( VertIn i) {
				VertOut o;
				o.v = mul(UNITY_MATRIX_MVP, i.v);

				o.uv = i.uv;
				o.uv2 = i.uv2;

				return o;
			}

			float4 ComputeObjectPos(float4 modelPoint) {

				// P for the actual coordinate relative to the center of
				// the object.
				float4 p = mul(UNITY_MATRIX_MVP, modelPoint);
				p.xy /= p.w;
				p.xy  = 0.5 * (p.xy + 1.0) * _ScreenParams.xy;
				return p;
			}

			bool CircleCondition(float4 uv, float4 center, float radius){
				return length(uv.xy - center.xy) < radius;
			}

			bool RectCondition(float4 uv, float4 left, float4 right, float halfHeight){
				return uv.x > left.x && uv.x < right.x && abs(uv.y - left.y) < halfHeight;
			}

			bool BarCondition(float4 uv, float4 left, float4 right, float radius){
				bool conditions = CircleCondition(uv, left, radius);
				conditions = conditions || CircleCondition(uv, right, radius);
				conditions = conditions || RectCondition(uv, left, right, radius);
				return conditions;
			}

			bool StripCondition(float4 uv, float slope, float speed, float frontColorWidth){
				return sin( ( uv.x  - uv.y * slope) * 0.1 - _Time.z*speed ) < frontColorWidth;
			}

			float disk(in float2 center, in float radius, in float2 p) {
				return length (p - center) - radius;
			}

			float lineDis( in float2 p1, in float2 p2, in float2 p )
			{
				float2 pa = p - p1;
				float2 ba = p2 - p1;
				float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
				return length( pa - ba*h );
			}


			float4 Frag( VertOut i ) : COLOR {

				float dist = 1e20;
				float distBack = 1e20;
				float distLine = 1e20;

				float barLength = 0.28;
				float4 posLeft = ComputeObjectPos(float4(-barLength, 0.00, 0, 1));
				float4 posRight = ComputeObjectPos(float4(barLength * _Width.x, 0.00, 0, 1));

				float4 posBar = ComputeObjectPos( _Progress - float4(0.28, 0, 0, 2));

				dist = min(dist, lineDis (posLeft, posRight, i.v));
				distLine = min(distLine, lineDis(posLeft, posBar, i.v));
//
			    float strip = sin( ( i.v.x  - i.v.y * 1) * 0.1 - _Time.z*12.3); 
			    float w = 0.5*fwidth(dist); 
    			w *= 2.; // extra blur

			    float4 finalColor = float4(0, 0, 0, 0);

			    float4 stripColor = float4(1, 0.6, 0, 1);
			    stripColor = lerp( float4(1.0, 1.0, 0.0, 1), stripColor, smoothstep(-0.5, 0.5, strip) );

		        finalColor = lerp( float4(0.0, 0.0, 0.0, 1.), finalColor, smoothstep(-w, w, dist - 20) ); // black

		    	finalColor = lerp( stripColor, finalColor, smoothstep(-w, w, distLine - 12) ); // orange


			    return finalColor;
		
			}

			ENDCG
		}
	}
}
