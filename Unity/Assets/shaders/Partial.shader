Shader "Custom/Partial" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_ContentColor("Couleur à modifier", Color) = (0, 0, 1, 1)
		_DegradeLow("Degrade couleur (y = 0)", Color) = (1, 1, 0, 1)
		_DegradeHigh("Degrade couleur (y = 1)", Color) = (1, 0, 0, 1)
		_Ratio("Remplissage", Range(0, 1)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
			CGPROGRAM
			#pragma surface surf Lambert finalcolor:myColor vertex:myVert
			#include "UnityCG.cginc"
			
//struct appdata_full {
//    float4 vertex : POSITION;
//    float4 tangent : TANGENT;
//    float3 normal : NORMAL;
//    float4 texcoord : TEXCOORD0;
//    float4 texcoord1 : TEXCOORD1;
//    fixed4 color : COLOR;
//#if defined(SHADER_API_XBOX360)
//	half4 texcoord2 : TEXCOORD2;
//	half4 texcoord3 : TEXCOORD3;
//	half4 texcoord4 : TEXCOORD4;
//	half4 texcoord5 : TEXCOORD5;
//#endif
//};
			
			float4 _HeightColor;
			float4 _ContentColor;
			float4 _DegradeLow;
			float4 _DegradeHigh;
			
			float _Ratio;
			sampler2D _MainTex;
			
			struct Input {
				float2 uv_MainTex;				
			};
			
			void myVert (inout appdata_full v, out Input IN)
			{
									
			}
			
			void myColor (Input IN, SurfaceOutput o, inout fixed4 color) {
	
		    }
			
			void surf (Input IN, inout SurfaceOutput o) {
				half4 c = tex2D (_MainTex, IN.uv_MainTex);
		     	o.Albedo = c.rgb;
		     	o.Alpha = c.a;
			}
			
			ENDCG
	} 
	FallBack "Diffuse"
}
