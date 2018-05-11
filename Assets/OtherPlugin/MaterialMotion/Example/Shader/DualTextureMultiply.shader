Shader "Custom/DualTextureMultiply" 
{
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_SecondTex ("Second (RGB) Trans (A)", 2D) = "white" {}
	_SecondColor ("Second Color", Color) = (1,1,1,1)
	_Multiplier( "Strengh", float ) = 1.0
}

SubShader {
	Tags { "RenderType" = "Opaque" }
	LOD 200

CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
sampler2D _SecondTex;
fixed4 _Color;
fixed4 _SecondColor;
fixed _Multiplier;

struct Input 
{
	float2 uv_MainTex;
	float2 uv2_SecondTex;
	float4 color : COLOR;
};

void surf (Input IN, inout SurfaceOutput o) 
{
	fixed4 c =  tex2D(_MainTex, IN.uv_MainTex) * _Color;
	fixed4 c2 = tex2D( _SecondTex, IN.uv2_SecondTex)* _SecondColor;
		
	fixed4 color = lerp(c,c2, c2.a) * _Multiplier; 
	
	o.Emission = color.rgb;

}
ENDCG
}

Fallback "Transparent/VertexLit"
}
