Shader "Hidden/VHSPro_bleed"{

   HLSLINCLUDE

   #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

   TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex); //default input texture
   TEXTURE2D_SAMPLER2D(_FeedbackTex, sampler_FeedbackTex); 

   #pragma shader_feature VHS_BLEED_ON
   #pragma shader_feature VHS_OLD_THREE_PHASE
   #pragma shader_feature VHS_THREE_PHASE
   #pragma shader_feature VHS_TWO_PHASE

   #pragma shader_feature VHS_SIGNAL_TWEAK_ON
   #include "vhs_bleed.hlsl" 

   float4 Frag(VaryingsDefault i) : SV_Target {

      float3 outColor = vhs2(i);
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

