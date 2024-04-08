Shader "Hidden/VHSPro_feedback"{

   HLSLINCLUDE

   #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

   // List of properties to control your post process effect
   TEXTURE2D_SAMPLER2D(_InputTex, sampler_InputTex); //BRP
   TEXTURE2D_SAMPLER2D(_FeedbackTex, sampler_FeedbackTex); //BRP
   TEXTURE2D_SAMPLER2D(_LastTex, sampler_LastTex); //BRP


   float feedbackAmount;
   float feedbackFade;
   float feedbackThresh;
   half3 feedbackColor;


   half3 bms(half3 a, half3 b){  return 1.-(1.-a)*(1.-b); }
   half grey(half3 a){  return (a.x+a.y+a.z)/3.; }

   half len(half3 a, half3 b){
      return (abs(a.x-b.x)+abs(a.y-b.y)+abs(a.z-b.z))/3.;
   }


   float4 Frag(VaryingsDefault i) : SV_Target {


      float2 p = i.texcoord; // og normalized tex coordnates 0..1  
      float one_x = 1./_ScreenParams.x;

      //new feedback value
      half3 fc = SAMPLE_TEXTURE2D(_InputTex, sampler_InputTex, p ).xyz; //_X
      half3 fl = SAMPLE_TEXTURE2D(_LastTex,  sampler_FeedbackTex, p ).xyz; //_X

      // return half4(fl, 0.);
      // return half4(grey(saturate(fc-fl)).xxx, 0.);
      // half3 fc =  tex2D( _MainTex, i.uvn).rgb;     //current frame without feedback
      // half3 fl =  tex2D( _LastTex, i.uvn).rgb;     //last frame without feedback
      float diff = grey(saturate(fc-fl)); //dfference between frames
      // float diff = len(fc,fl); //dfference between frames
      // float diff = len(fl,fc); //dfference between frames
      // float diff = abs(fl.x-fc.x + fl.y-fc.y + fl.z-fc.z)/3.; //dfference between frames
      if(diff<feedbackThresh) diff = 0.;

      half3 fbn = fc*diff*feedbackAmount; //feedback new
      // half3 fbn = fc*diff*feedbackAmount; //feedback new
      // fbn = half3(0.0, 0.0, 0.0);
      

      //old feedback buffer
      half3 fbb = ( //blur old buffer a bit
         SAMPLE_TEXTURE2D(_FeedbackTex, sampler_FeedbackTex, p ).xyz *.5 +
         SAMPLE_TEXTURE2D(_FeedbackTex, sampler_FeedbackTex, (p+ float2(one_x,0)) ).xyz *.25 +
         SAMPLE_TEXTURE2D(_FeedbackTex, sampler_FeedbackTex, (p- float2(one_x,0)) ).xyz *.25 
      );// / 3.;
      fbb *= feedbackFade;
      // if( (fbb.x+fbb.y+fbb.z)/3.<.01 ) fbb = half3(0,0,0);

      fbn = bms(fbn, fbb); 

      return float4(fbn * feedbackColor, 1.); //*feedbackColor 

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
