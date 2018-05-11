// Shader created with Shader Forge v1.16 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.16;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4013,x:32839,y:32709,varname:node_4013,prsc:2|diff-1908-OUT,spec-751-OUT,gloss-5424-OUT,normal-332-RGB,lwrap-9978-RGB,clip-7103-OUT;n:type:ShaderForge.SFN_TexCoord,id:6586,x:29840,y:32888,varname:node_6586,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:684,x:31434,y:32686,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_684,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False|UVIN-4898-OUT;n:type:ShaderForge.SFN_Time,id:5256,x:29840,y:33111,varname:node_5256,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3201,x:30035,y:33131,varname:node_3201,prsc:2|A-5256-T,B-2134-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2134,x:29840,y:33241,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_2134,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Tex2d,id:7865,x:30704,y:32470,ptovrint:False,ptlb:Flow,ptin:_Flow,varname:node_7865,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bff13337e26c6704ea7cd87d9bdead4e,ntxv:0,isnm:False|UVIN-9492-UVOUT;n:type:ShaderForge.SFN_Panner,id:9492,x:30227,y:33057,varname:node_9492,prsc:2,spu:1,spv:0|UVIN-5244-UVOUT,DIST-3201-OUT;n:type:ShaderForge.SFN_Rotator,id:5244,x:30035,y:33002,varname:node_5244,prsc:2|UVIN-6586-UVOUT,ANG-3230-OUT;n:type:ShaderForge.SFN_Slider,id:3230,x:29683,y:33043,ptovrint:False,ptlb:Rosition,ptin:_Rosition,varname:node_3230,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-3.14,cur:0,max:3.14;n:type:ShaderForge.SFN_Color,id:8193,x:31433,y:32528,ptovrint:False,ptlb:Diffuse_Color,ptin:_Diffuse_Color,varname:node_8193,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5073529,c2:0.5073529,c3:0.5073529,c4:1;n:type:ShaderForge.SFN_Multiply,id:1908,x:31609,y:32664,varname:node_1908,prsc:2|A-8193-RGB,B-684-RGB;n:type:ShaderForge.SFN_Desaturate,id:751,x:32426,y:32345,varname:node_751,prsc:2|COL-1920-OUT;n:type:ShaderForge.SFN_Multiply,id:1920,x:31996,y:32276,varname:node_1920,prsc:2|A-1908-OUT,B-7555-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7555,x:31996,y:32410,ptovrint:False,ptlb:Gloss_Diffuse,ptin:_Gloss_Diffuse,varname:node_7555,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:3401,x:31996,y:32467,varname:node_3401,prsc:2|A-7865-RGB,B-2321-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2321,x:31996,y:32604,ptovrint:False,ptlb:Gloss_Flow,ptin:_Gloss_Flow,varname:node_2321,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_AmbientLight,id:9978,x:32528,y:32887,varname:node_9978,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:332,x:31492,y:33216,ptovrint:False,ptlb:Flow_nor,ptin:_Flow_nor,varname:node_332,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6a183d71c958ce44c933de2aed1e8ef0,ntxv:3,isnm:True|UVIN-9492-UVOUT;n:type:ShaderForge.SFN_Desaturate,id:5424,x:32370,y:32586,varname:node_5424,prsc:2|COL-3401-OUT;n:type:ShaderForge.SFN_ComponentMask,id:838,x:30973,y:32882,varname:node_838,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-7865-RGB;n:type:ShaderForge.SFN_Multiply,id:4898,x:31262,y:32686,varname:node_4898,prsc:2|A-5244-UVOUT,B-7792-OUT;n:type:ShaderForge.SFN_Power,id:7792,x:31247,y:32931,varname:node_7792,prsc:2|VAL-838-OUT,EXP-1462-OUT;n:type:ShaderForge.SFN_Slider,id:1462,x:30816,y:33069,ptovrint:False,ptlb:Warp,ptin:_Warp,varname:node_1462,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:0.1;n:type:ShaderForge.SFN_TexCoord,id:8197,x:32124,y:33145,varname:node_8197,prsc:2,uv:0;n:type:ShaderForge.SFN_Blend,id:7103,x:32498,y:33249,varname:node_7103,prsc:2,blmd:16,clmp:True|SRC-8197-U,DST-9823-OUT;n:type:ShaderForge.SFN_Slider,id:9823,x:31922,y:33357,ptovrint:False,ptlb:cut,ptin:_cut,varname:node_9823,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;proporder:684-8193-7555-7865-332-2321-2134-3230-1462-9823;pass:END;sub:END;*/

Shader "Jing Ge/Water2.2.1" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Diffuse_Color ("Diffuse_Color", Color) = (0.5073529,0.5073529,0.5073529,1)
        _Gloss_Diffuse ("Gloss_Diffuse", Float ) = 0
        _Flow ("Flow", 2D) = "white" {}
        _Flow_nor ("Flow_nor", 2D) = "bump" {}
        _Gloss_Flow ("Gloss_Flow", Float ) = 0
        _Speed ("Speed", Float ) = 0.2
        _Rosition ("Rosition", Range(-3.14, 3.14)) = 0
        _Warp ("Warp", Range(0, 0.1)) = 0
        _cut ("cut", Range(0, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
			Cull off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _Speed;
            uniform sampler2D _Flow; uniform float4 _Flow_ST;
            uniform float _Rosition;
            uniform float4 _Diffuse_Color;
            uniform float _Gloss_Diffuse;
            uniform float _Gloss_Flow;
            uniform sampler2D _Flow_nor; uniform float4 _Flow_nor_ST;
            uniform float _Warp;
            uniform float _cut;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD10;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
            #endif
            #ifdef DYNAMICLIGHTMAP_ON
                o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
            #endif
            o.normalDir = UnityObjectToWorldNormal(v.normal);
            o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
            o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
            o.posWorld = mul(_Object2World, v.vertex);
            float3 lightColor = _LightColor0.rgb;
            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
            UNITY_TRANSFER_FOG(o,o.pos);
            TRANSFER_VERTEX_TO_FRAGMENT(o)
            return o;
        }
        float4 frag(VertexOutput i) : COLOR {
            i.normalDir = normalize(i.normalDir);
            float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/// Vectors:
            float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
            float4 node_5256 = _Time + _TimeEditor;
            float node_5244_ang = _Rosition;
            float node_5244_spd = 1.0;
            float node_5244_cos = cos(node_5244_spd*node_5244_ang);
            float node_5244_sin = sin(node_5244_spd*node_5244_ang);
            float2 node_5244_piv = float2(0.5,0.5);
            float2 node_5244 = (mul(i.uv0-node_5244_piv,float2x2( node_5244_cos, -node_5244_sin, node_5244_sin, node_5244_cos))+node_5244_piv);
            float2 node_9492 = (node_5244+(node_5256.g*_Speed)*float2(1,0));
            float3 _Flow_nor_var = UnpackNormal(tex2D(_Flow_nor,TRANSFORM_TEX(node_9492, _Flow_nor)));
            float3 normalLocal = _Flow_nor_var.rgb;
            float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
            float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
            clip(saturate(round(1- 0.5*(i.uv0.g + _cut))) - 0.5);
            float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
            float3 lightColor = _LightColor0.rgb;
            float3 halfDirection = normalize(viewDirection+lightDirection);
// Lighting:
            float attenuation = LIGHT_ATTENUATION(i);
            float3 attenColor = attenuation * _LightColor0.xyz;
            float Pi = 3.141592654;
            float InvPi = 0.31830988618;
///// Gloss:
            float4 _Flow_var = tex2D(_Flow,TRANSFORM_TEX(node_9492, _Flow));
            float gloss = dot((_Flow_var.rgb*_Gloss_Flow),float3(0.3,0.59,0.11));
            float specPow = exp2( gloss * 10.0+1.0);
/// GI Data:
            UnityLight light;
            #ifdef LIGHTMAP_OFF
                light.color = lightColor;
                light.dir = lightDirection;
                light.ndotl = LambertTerm (normalDirection, light.dir);
            #else
                light.color = half3(0.f, 0.f, 0.f);
                light.ndotl = 0.0f;
                light.dir = half3(0.f, 0.f, 0.f);
            #endif
            UnityGIInput d;
            d.light = light;
            d.worldPos = i.posWorld.xyz;
            d.worldViewDir = viewDirection;
            d.atten = attenuation;
            #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                d.ambient = 0;
                d.lightmapUV = i.ambientOrLightmapUV;
            #else
                d.ambient = i.ambientOrLightmapUV;
            #endif
            d.boxMax[0] = unity_SpecCube0_BoxMax;
            d.boxMin[0] = unity_SpecCube0_BoxMin;
            d.probePosition[0] = unity_SpecCube0_ProbePosition;
            d.probeHDR[0] = unity_SpecCube0_HDR;
            d.boxMax[1] = unity_SpecCube1_BoxMax;
            d.boxMin[1] = unity_SpecCube1_BoxMin;
            d.probePosition[1] = unity_SpecCube1_ProbePosition;
            d.probeHDR[1] = unity_SpecCube1_HDR;
            UnityGI gi = UnityGlobalIllumination (d, 1, gloss, normalDirection);
            lightDirection = gi.light.dir;
            lightColor = gi.light.color;
// Specular:
            float NdotL = max(0, dot( normalDirection, lightDirection ));
            float LdotH = max(0.0,dot(lightDirection, halfDirection));
            float2 node_4898 = (node_5244*pow(_Flow_var.rgb.rg,_Warp));
            float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(node_4898, _Diffuse));
            float3 node_1908 = (_Diffuse_Color.rgb*_Diffuse_var.rgb);
            float3 diffuseColor = node_1908; // Need this for specular when using metallic
            float specularMonochrome;
            float3 specularColor;
            diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, dot((node_1908*_Gloss_Diffuse),float3(0.3,0.59,0.11)), specularColor, specularMonochrome );
            specularMonochrome = 1-specularMonochrome;
            float NdotV = max(0.0,dot( normalDirection, viewDirection ));
            float NdotH = max(0.0,dot( normalDirection, halfDirection ));
            float VdotH = max(0.0,dot( viewDirection, halfDirection ));
            float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
            float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
            float specularPBL = max(0, (NdotL*visTerm*normTerm) * unity_LightGammaCorrectionConsts_PIDiv4 );
            float3 directSpecular = 1 * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
            half grazingTerm = saturate( gloss + specularMonochrome );
            float3 indirectSpecular = (gi.indirect.specular);
            indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
            float3 specular = (directSpecular + indirectSpecular);
/// Diffuse:
            NdotL = dot( normalDirection, lightDirection );
            float3 w = UNITY_LIGHTMODEL_AMBIENT.rgb*0.5; // Light wrapping
            float3 NdotLWrap = NdotL * ( 1.0 - w );
            float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
            NdotL = max(0.0,dot( normalDirection, lightDirection ));
            half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
            NdotLWrap = max(float3(0,0,0), NdotLWrap);
            float3 directDiffuse = (forwardLight + ((1 +(fd90 - 1)*pow((1.00001-NdotLWrap), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL))*(0.5-max(w.r,max(w.g,w.b))*0.5) * attenColor;
            float3 indirectDiffuse = float3(0,0,0);
            indirectDiffuse += gi.indirect.diffuse;
            float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
// Final Color:
            float3 finalColor = diffuse + specular;
            fixed4 finalRGBA = fixed4(finalColor,1);
            UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
            return finalRGBA;
        }
        ENDCG
    }
    Pass {
        Name "FORWARD_DELTA"
        Tags {
            "LightMode"="ForwardAdd"
        }
        Blend One One
        
        
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #define UNITY_PASS_FORWARDADD
        #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
        #define _GLOSSYENV 1
        #include "UnityCG.cginc"
        #include "AutoLight.cginc"
        #include "Lighting.cginc"
        #include "UnityPBSLighting.cginc"
        #include "UnityStandardBRDF.cginc"
        #pragma multi_compile_fwdadd_fullshadows
        #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
        #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
        #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
        #pragma multi_compile_fog
        #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
        #pragma target 3.0
        uniform float4 _TimeEditor;
        uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
        uniform float _Speed;
        uniform sampler2D _Flow; uniform float4 _Flow_ST;
        uniform float _Rosition;
        uniform float4 _Diffuse_Color;
        uniform float _Gloss_Diffuse;
        uniform float _Gloss_Flow;
        uniform sampler2D _Flow_nor; uniform float4 _Flow_nor_ST;
        uniform float _Warp;
        uniform float _cut;
        struct VertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 tangent : TANGENT;
            float2 texcoord0 : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
            float2 texcoord2 : TEXCOORD2;
        };
        struct VertexOutput {
            float4 pos : SV_POSITION;
            float2 uv0 : TEXCOORD0;
            float2 uv1 : TEXCOORD1;
            float2 uv2 : TEXCOORD2;
            float4 posWorld : TEXCOORD3;
            float3 normalDir : TEXCOORD4;
            float3 tangentDir : TEXCOORD5;
            float3 bitangentDir : TEXCOORD6;
            LIGHTING_COORDS(7,8)
        };
        VertexOutput vert (VertexInput v) {
            VertexOutput o = (VertexOutput)0;
            o.uv0 = v.texcoord0;
            o.uv1 = v.texcoord1;
            o.uv2 = v.texcoord2;
            o.normalDir = UnityObjectToWorldNormal(v.normal);
            o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
            o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
            o.posWorld = mul(_Object2World, v.vertex);
            float3 lightColor = _LightColor0.rgb;
            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
            TRANSFER_VERTEX_TO_FRAGMENT(o)
            return o;
        }
        float4 frag(VertexOutput i) : COLOR {
            i.normalDir = normalize(i.normalDir);
            float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/// Vectors:
            float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
            float4 node_5256 = _Time + _TimeEditor;
            float node_5244_ang = _Rosition;
            float node_5244_spd = 1.0;
            float node_5244_cos = cos(node_5244_spd*node_5244_ang);
            float node_5244_sin = sin(node_5244_spd*node_5244_ang);
            float2 node_5244_piv = float2(0.5,0.5);
            float2 node_5244 = (mul(i.uv0-node_5244_piv,float2x2( node_5244_cos, -node_5244_sin, node_5244_sin, node_5244_cos))+node_5244_piv);
            float2 node_9492 = (node_5244+(node_5256.g*_Speed)*float2(1,0));
            float3 _Flow_nor_var = UnpackNormal(tex2D(_Flow_nor,TRANSFORM_TEX(node_9492, _Flow_nor)));
            float3 normalLocal = _Flow_nor_var.rgb;
            float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
            clip(saturate(round(1 - 0.5*(i.uv0.g + _cut))) - 0.5);
            float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
            float3 lightColor = _LightColor0.rgb;
            float3 halfDirection = normalize(viewDirection+lightDirection);
// Lighting:
            float attenuation = LIGHT_ATTENUATION(i);
            float3 attenColor = attenuation * _LightColor0.xyz;
            float Pi = 3.141592654;
            float InvPi = 0.31830988618;
///// Gloss:
            float4 _Flow_var = tex2D(_Flow,TRANSFORM_TEX(node_9492, _Flow));
            float gloss = dot((_Flow_var.rgb*_Gloss_Flow),float3(0.3,0.59,0.11));
            float specPow = exp2( gloss * 10.0+1.0);
// Specular:
            float NdotL = max(0, dot( normalDirection, lightDirection ));
            float LdotH = max(0.0,dot(lightDirection, halfDirection));
            float2 node_4898 = (node_5244*pow(_Flow_var.rgb.rg,_Warp));
            float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(node_4898, _Diffuse));
            float3 node_1908 = (_Diffuse_Color.rgb*_Diffuse_var.rgb);
            float3 diffuseColor = node_1908; // Need this for specular when using metallic
            float specularMonochrome;
            float3 specularColor;
            diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, dot((node_1908*_Gloss_Diffuse),float3(0.3,0.59,0.11)), specularColor, specularMonochrome );
            specularMonochrome = 1-specularMonochrome;
            float NdotV = max(0.0,dot( normalDirection, viewDirection ));
            float NdotH = max(0.0,dot( normalDirection, halfDirection ));
            float VdotH = max(0.0,dot( viewDirection, halfDirection ));
            float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
            float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
            float specularPBL = max(0, (NdotL*visTerm*normTerm) * unity_LightGammaCorrectionConsts_PIDiv4 );
            float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
            float3 specular = directSpecular;
/// Diffuse:
            NdotL = dot( normalDirection, lightDirection );
            float3 w = UNITY_LIGHTMODEL_AMBIENT.rgb*0.5; // Light wrapping
            float3 NdotLWrap = NdotL * ( 1.0 - w );
            float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
            NdotL = max(0.0,dot( normalDirection, lightDirection ));
            half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
            NdotLWrap = max(float3(0,0,0), NdotLWrap);
            float3 directDiffuse = (forwardLight + ((1 +(fd90 - 1)*pow((1.00001-NdotLWrap), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL))*(0.5-max(w.r,max(w.g,w.b))*0.5) * attenColor;
            float3 diffuse = directDiffuse * diffuseColor;
// Final Color:
            float3 finalColor = diffuse + specular;
            return fixed4(finalColor * 1,0);
        }
        ENDCG
    }
    Pass {
        Name "ShadowCaster"
        Tags {
            "LightMode"="ShadowCaster"
        }
        Offset 1, 1
        
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #define UNITY_PASS_SHADOWCASTER
        #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
        #define _GLOSSYENV 1
        #include "UnityCG.cginc"
        #include "Lighting.cginc"
        #include "UnityPBSLighting.cginc"
        #include "UnityStandardBRDF.cginc"
        #pragma fragmentoption ARB_precision_hint_fastest
        #pragma multi_compile_shadowcaster
        #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
        #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
        #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
        #pragma multi_compile_fog
        #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
        #pragma target 3.0
        uniform float _cut;
        struct VertexInput {
            float4 vertex : POSITION;
            float2 texcoord0 : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
            float2 texcoord2 : TEXCOORD2;
        };
        struct VertexOutput {
            V2F_SHADOW_CASTER;
            float2 uv0 : TEXCOORD1;
            float2 uv1 : TEXCOORD2;
            float2 uv2 : TEXCOORD3;
            float4 posWorld : TEXCOORD4;
        };
        VertexOutput vert (VertexInput v) {
            VertexOutput o = (VertexOutput)0;
            o.uv0 = v.texcoord0;
            o.uv1 = v.texcoord1;
            o.uv2 = v.texcoord2;
            o.posWorld = mul(_Object2World, v.vertex);
            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
            TRANSFER_SHADOW_CASTER(o)
            return o;
        }
        float4 frag(VertexOutput i) : COLOR {
/// Vectors:
            float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
            clip(saturate(round(1- 0.5*(i.uv0.g + _cut))) - 0.5);
            SHADOW_CASTER_FRAGMENT(i)
        }
        ENDCG
    }
    Pass {
        Name "Meta"
        Tags {
            "LightMode"="Meta"
        }
        Cull Off
        
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #define UNITY_PASS_META 1
        #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
        #define _GLOSSYENV 1
        #include "UnityCG.cginc"
        #include "Lighting.cginc"
        #include "UnityPBSLighting.cginc"
        #include "UnityStandardBRDF.cginc"
        #include "UnityMetaPass.cginc"
        #pragma fragmentoption ARB_precision_hint_fastest
        #pragma multi_compile_shadowcaster
        #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
        #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
        #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
        #pragma multi_compile_fog
        #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
        #pragma target 3.0
        uniform float4 _TimeEditor;
        uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
        uniform float _Speed;
        uniform sampler2D _Flow; uniform float4 _Flow_ST;
        uniform float _Rosition;
        uniform float4 _Diffuse_Color;
        uniform float _Gloss_Diffuse;
        uniform float _Gloss_Flow;
        uniform float _Warp;
        struct VertexInput {
            float4 vertex : POSITION;
            float2 texcoord0 : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
            float2 texcoord2 : TEXCOORD2;
        };
        struct VertexOutput {
            float4 pos : SV_POSITION;
            float2 uv0 : TEXCOORD0;
            float2 uv1 : TEXCOORD1;
            float2 uv2 : TEXCOORD2;
            float4 posWorld : TEXCOORD3;
        };
        VertexOutput vert (VertexInput v) {
            VertexOutput o = (VertexOutput)0;
            o.uv0 = v.texcoord0;
            o.uv1 = v.texcoord1;
            o.uv2 = v.texcoord2;
            o.posWorld = mul(_Object2World, v.vertex);
            o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
            return o;
        }
        float4 frag(VertexOutput i) : SV_Target {
/// Vectors:
            float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
            UnityMetaInput o;
            UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
            
            o.Emission = 0;
            
            float node_5244_ang = _Rosition;
            float node_5244_spd = 1.0;
            float node_5244_cos = cos(node_5244_spd*node_5244_ang);
            float node_5244_sin = sin(node_5244_spd*node_5244_ang);
            float2 node_5244_piv = float2(0.5,0.5);
            float2 node_5244 = (mul(i.uv0-node_5244_piv,float2x2( node_5244_cos, -node_5244_sin, node_5244_sin, node_5244_cos))+node_5244_piv);
            float4 node_5256 = _Time + _TimeEditor;
            float2 node_9492 = (node_5244+(node_5256.g*_Speed)*float2(1,0));
            float4 _Flow_var = tex2D(_Flow,TRANSFORM_TEX(node_9492, _Flow));
            float2 node_4898 = (node_5244*pow(_Flow_var.rgb.rg,_Warp));
            float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(node_4898, _Diffuse));
            float3 node_1908 = (_Diffuse_Color.rgb*_Diffuse_var.rgb);
            float3 diffColor = node_1908;
            float specularMonochrome;
            float3 specColor;
            diffColor = DiffuseAndSpecularFromMetallic( diffColor, dot((node_1908*_Gloss_Diffuse),float3(0.3,0.59,0.11)), specColor, specularMonochrome );
            float roughness = 1.0 - dot((_Flow_var.rgb*_Gloss_Flow),float3(0.3,0.59,0.11));
            o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
            
            return UnityMetaFragment( o );
        }
        ENDCG
    }
}
FallBack "Diffuse"
CustomEditor "ShaderForgeMaterialInspector"
}
