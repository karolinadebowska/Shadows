Shader "Custome/TransparentShadowSpotLight" { 
 Properties { 
     _Tex("Base (RGB)", 2D) = "white" {} 
 } 

 SubShader {
     Pass {
         Blend One One
         Tags { "LightMode" = "ForwardAdd" } 
         CGPROGRAM 
         #pragma vertex vert 
         #pragma fragment frag 
         #pragma multi_compile_fwdadd_fullshadows 
         #include "UnityCG.cginc" 
         #include "AutoLight.cginc" 

         sampler2D _Tex;
         float4 _Tex_ST;

         struct v2f { 
             float4 pos : SV_POSITION; 
             LIGHTING_COORDS(0,1) 
             float2 uv : TEXCOORD2;
         }; 

         v2f vert(appdata_base v) { 
             v2f o; 
             o.pos = UnityObjectToClipPos(v.vertex); 
             o.uv = TRANSFORM_TEX (v.texcoord, _Tex);
             TRANSFER_VERTEX_TO_FRAGMENT(o); 
             return o; 
         } 

         fixed4 frag(v2f i) : COLOR
         { 
             float attenuation = LIGHT_ATTENUATION(i); 
             return tex2D (_Tex, i.uv) * attenuation; 
         } 
         ENDCG 
     }
 }

 Fallback "VertexLit" 
}