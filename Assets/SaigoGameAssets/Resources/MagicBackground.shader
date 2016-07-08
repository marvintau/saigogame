Shader "Unlit/MagicBackground"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			#define Iterations 64
			#define Thickness 0.1
			#define SuperQuadPower 8.0
			#define Fisheye 0.5

			float rand(float3 r) {
				return frac(sin(dot(r.xy,float2(1.38984*sin(r.z),1.13233*cos(r.z))))*653758.5453);
			}

			float truchetarc(float3 pos) {
				float r=length(pos.xy);
				return pow(pow(abs(r-0.5),SuperQuadPower)+pow(abs(pos.z-0.5),SuperQuadPower),1.0/SuperQuadPower)-Thickness;
			}

			float truchetcell(float3 pos) {
				return min(min( truchetarc(pos), truchetarc(float3(pos.z,1.0-pos.x,pos.y))), truchetarc(float3(1.0-pos.y,1.0-pos.z,pos.x)));
			}

			float distfunc(float3 pos) {
				float3 cellpos=frac(pos);
				float3 gridpos=floor(pos);

				float rnd = rand(gridpos);

				if(rnd<1.0/8.0) return truchetcell(float3(cellpos.x,cellpos.y,cellpos.z));
				else if(rnd<2.0/8.0) return truchetcell(float3(cellpos.x,1.0-cellpos.y,cellpos.z));
				else if(rnd<3.0/8.0) return truchetcell(float3(1.0-cellpos.x,cellpos.y,cellpos.z));
				else if(rnd<4.0/8.0) return truchetcell(float3(1.0-cellpos.x,1.0-cellpos.y,cellpos.z));
				else if(rnd<5.0/8.0) return truchetcell(float3(cellpos.y,cellpos.x,cellpos.z));
				else if(rnd<6.0/8.0) return truchetcell(float3(cellpos.y,1.0-cellpos.x,cellpos.z));
				else if(rnd<7.0/8.0) return truchetcell(float3(1.0-cellpos.y,cellpos.x,cellpos.z));
				else  return truchetcell(float3(1.0-cellpos.y,1.0-cellpos.x,cellpos.z));
			}

			float3 gradient(float3 pos) {
				const float eps=0.0001;
				float mid=distfunc(pos);
				return float3(
				distfunc(pos+float3(eps,0.0,0.0))-mid,
				distfunc(pos+float3(0.0,eps,0.0))-mid,
				distfunc(pos+float3(0.0,0.0,eps))-mid);
			}

			float4 mainImage( float2 fragCoord ) {
				const float pi=3.141592;

				float2 coords=(2.0*fragCoord.xy-_ScreenParams.xy)/length(_ScreenParams.xy);

				float a1=_Time.x/3.0;
				float3x3 m=float3x3(
				0.0,1.0,0.0,
				-sin(a1),0.0,cos(a1),
				cos(a1),0.0,sin(a1));
				m*=m;
				m*=m;

				float3 ray_dir=mul(m, normalize(float3(1.4*coords,-1.0+Fisheye*(coords.x*coords.x+coords.y*coords.y))));

				float t=_Time.z/3.0;
				float3 ray_pos=float3(
			    2.0*(sin(t+sin(2.0*t)/2.0)/2.0+0.5),
			    2.0*(sin(t-sin(2.0*t)/2.0-pi/2.0)/2.0+0.5),
			    2.0*((-2.0*(t-sin(4.0*t)/4.0)/pi)+0.5+0.5));

				float i=float(Iterations);
				for(int j=0;j<Iterations;j++)
				{
					float dist=distfunc(ray_pos);
					ray_pos+=dist*ray_dir;

					if(abs(dist)<0.001) { i=float(j); break; }
				}

				float3 normal=normalize(gradient(ray_pos));

				float ao=1.0-i/float(Iterations);
				float what=pow(max(0.0,dot(normal,-ray_dir)),2.0);
				float vignette=pow(1.0-length(coords),0.3);
				float light=ao*what*vignette*1.4;

				float z=ray_pos.z/2.0;
				float3 col=(cos(ray_pos/2.0)+2.0)/3.0;

				float3 reflected=reflect(ray_dir,normal);

				return float4(col*light,1.0);
			}

			float4 vert (float4 v: POSITION) : SV_POSITION  {
				return mul(UNITY_MATRIX_MVP, v);
			}

			float4 frag (float4 v: SV_POSITION) : COLOR {
				return mainImage(v.xy);
			}

			ENDCG
		}
	}
}
	