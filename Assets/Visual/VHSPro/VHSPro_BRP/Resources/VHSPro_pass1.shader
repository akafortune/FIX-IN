Shader "Hidden/VHSPro_pass1"{

   HLSLINCLUDE

   #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

   // List of properties to control your post process effect
   //TEXTURE2D_X is use to be compatible with single instancing VR path
   //https://teodutra.com/unity/shaders/urp/graphics/2020/05/18/From-Built-in-to-URP/
   TEXTURE2D_SAMPLER2D(_InputTex, sampler_InputTex); //BRP

   float    _time;
   float4   _ResOg;  // before pixelation (.xy resolution, .zw one pixel )
   float4   _Res;    // after pixelation  (.xy resolution, .zw one pixel )
   float4   _ResN;   // noise resolution  (.xy resolution, .zw one pixel )

   //Color Reduction Part (former graphics adapter pro)
   #pragma shader_feature _ VHS_COLOR
   #pragma shader_feature _ VHS_PALETTE   
   #pragma shader_feature _ VHS_DITHER   
   #pragma shader_feature _ VHS_SIGNAL_TWEAK_ON  
   TEXTURE2D_SAMPLER2D(_PaletteTex, sampler_PaletteTex); //BRP 
   // TEXTURE2D_X(_PaletteTex); SAMPLER(sampler_PaletteTex);
   #include "vhs_gap.hlsl"



   //Signal Distortion Part
   #pragma shader_feature _ VHS_FILMGRAIN_ON
   #pragma shader_feature _ VHS_LINENOISE_ON
   #pragma shader_feature _ VHS_TAPENOISE_ON
   #pragma shader_feature _ VHS_YIQNOISE_ON
   #pragma shader_feature _ VHS_TWITCH_H_ON
   #pragma shader_feature _ VHS_TWITCH_V_ON  
   #pragma shader_feature _ VHS_JITTER_H_ON
   #pragma shader_feature _ VHS_JITTER_V_ON 
   #pragma shader_feature _ VHS_LINESFLOAT_ON
   #pragma shader_feature _ VHS_SCANLINES_ON
   #pragma shader_feature _ VHS_STRETCH_ON  
   
   TEXTURE2D_SAMPLER2D(_TapeTex, sampler_TapeTex); //BRP 
   #include "vhs_pass1.hlsl" 


   float4 Frag(VaryingsDefault i) : SV_Target{

      //TODO figure out i.texcoordStereo

      float3 outColor = vhs(i);    
      return float4(outColor, 1);

   }

   ENDHLSL

   SubShader {

      Cull Off ZWrite Off ZTest Always
      Pass {
         HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
         ENDHLSL
      }
   }
}


