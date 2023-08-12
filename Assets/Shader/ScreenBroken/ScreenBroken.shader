Shader "AD/RenderImage/ScreenBroken" {
	Properties {
		_MainTex ("Main Tex", 2D) = "white" {}
		_BrokenNormalMap("BrokenNormal Map",2D)="bump"{}
		_BrokenScale("BrokenScale",Range(0,1))=1.0
	}
	SubShader {
		
 
		Pass{
			Tags { "LightMode"="ForwardBase" }
 
			CGPROGRAM
 
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
 
			#pragma vertex vert
			#pragma fragment frag
 
			//这一部分参数的定义要根据Properties
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BrokenNormalMap;
			float4 _BrokenNormalMap_ST;
			float _BrokenScale;
 
			struct a2v{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
 
			//输出部分要和输入部分对应起来,而输出部分又要由片元着色器里的计算模型来确定
			struct v2f{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
			};
 
			v2f vert(a2v v){
				v2f o;
				o.pos=UnityObjectToClipPos(v.vertex);
				
				o.uv.xy=TRANSFORM_TEX(v.texcoord,_MainTex);
				o.uv.zw=TRANSFORM_TEX(v.texcoord, _BrokenNormalMap);
				return o;
			}
 
			fixed4 frag(v2f i) : SV_Target{
 
				fixed4 packedNormal = tex2D(_BrokenNormalMap,i.uv.zw);
				fixed3 tangentNormal;
				tangentNormal=UnpackNormal(packedNormal);
				
				tangentNormal.xy*=_BrokenScale;
				float2 offset = tangentNormal.xy;
 
				fixed3 lightColor = fixed3(1,1,1);
				fixed3 col=tex2D(_MainTex,i.uv.xy+offset).rgb;
				
				fixed luminance = (col.r + col.g + col.b) / 3;
				fixed3 finalCol = lerp(fixed3(luminance,luminance,luminance),col,0.25);
				
				return fixed4(col,1.0f);
			}
			ENDCG
			}
		
		}
	FallBack "Diffuse"
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenBroken : MonoBehaviour
{

    public Material mat;
    public float NormalScale = 0;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture src0 = RenderTexture.GetTemporary(source.width, source.height);
        mat.SetTexture("_MainTex", source);
        mat.SetFloat("_BrokenScale", NormalScale);
        Graphics.Blit(source, src0, mat, 0);
        Graphics.Blit(src0, destination);

        RenderTexture.ReleaseTemporary(src0);
    }
}

*/
