using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

using System;

using VladStorm;

public class VHSProRenderer : PostProcessEffectRenderer<VHSPro> {

   //Note: important for shader_features to be included in a build
   //we use 4 Materials in Resources folder - they will be included in the build 
   //we switch keywords for those materials that unity will keep correct shader variants for those materials in the build
   //we need to switch keywords in shader sheets here and meterials simultaneously - seems it doesnt work just in shader sheets 

   protected VHSPro cmpt;     //component from Post Processing Stack Inspector 

   //Materials
   public Material mat1;            //1st pass (signal distortion)
   public Material matTape;         //2nd pass vhs bleeding + mix with feedback
   public Material matBleed;        //tape noise
   public Material matFeedback;     //feedback

   public Shader shader1;         
   public Shader shaderBleed;     
   public Shader shaderTape;      
   public Shader shaderFeedback;  
   PropertySheet sheet1;
   PropertySheet sheetTape;
   PropertySheet sheetBleed;
   PropertySheet sheetFeedback;
   MaterialPropertyBlock mpb1;
   MaterialPropertyBlock mpbTape;
   MaterialPropertyBlock mpbBleed;
   MaterialPropertyBlock mpbFeedback;

   //textures (URP way)
   int texIdPass1 =        Shader.PropertyToID("_TexPass1");
   int texIdTape =         Shader.PropertyToID("_TexTape");
   int texIdFeedback =     Shader.PropertyToID("_TexFeedback");
   RenderTargetIdentifier texPass1;
   RenderTargetIdentifier texTape;
   RenderTargetIdentifier texFeedback;

   //these 2 we need to pass to the next frame 
   RenderTexture texFeedbackLast;
   RenderTexture texLast;
   
   RenderTexture texNull;// just coz we need to pass sth into Blit fn


   float _time = 0f;
   Vector4 _ResOg; 
   Vector4 _Res;
   Vector4 _ResN;


   //configure render targets, their clear state, and temporary render target textures.
   public void _OnCameraSetup(PostProcessRenderContext context) {
 
      //BuiltIn RP 
      CommandBuffer cmd = context.command;
      cmpt = settings; //settings from param part of the class

      //TODO check with og plugin! - that screenWidth and screenHeight are the values i need 
      // RenderTargetIdentifier texSource = context.source;
      RenderTextureDescriptor desc = new RenderTextureDescriptor(context.screenWidth, context.screenHeight);

      //init palettes and resolution presets
      VHSHelper.Init();

      //Resolution Presents
      ResPreset resPreset = VHSHelper.GetResPresets()[cmpt.screenResPresetId.value];
      if(resPreset.isCustom!=true){
         cmpt.screenWidth.value  = resPreset.screenWidth;
         cmpt.screenHeight.value = resPreset.screenHeight;
      }
      if(resPreset.isFirst==true || cmpt.pixelOn.value==false){
         cmpt.screenWidth.value  = desc.width; 
         cmpt.screenHeight.value = desc.height; 
      }

      //original screen resolution (.xy resolution .zw one pixel)
      _ResOg = new Vector4(desc.width, desc.height, 0f, 0f);
      _ResOg[2] = 1f/_ResOg.x; 
      _ResOg[3] = 1f/_ResOg.y;  

      //resolution after pixelation
      _Res = new Vector4(cmpt.screenWidth.value, cmpt.screenHeight.value, 0f,0f);
      _Res[2] = 1f/_Res.x;                                    
      _Res[3] = 1f/_Res.y;                                    

      //resolution of noise 
      _ResN = new Vector4(_Res.x, _Res.y, _Res.z, _Res.w);
      if(!cmpt.noiseResGlobal.value){
         _ResN = new Vector4(cmpt.noiseResWidth.value, cmpt.noiseResHeight.value, 0f, 0f);
         _ResN[2] = 1f/_ResN.x;                                    
         _ResN[3] = 1f/_ResN.y;                                                
      }

      //loading materials from resources
      if(mat1==null)          LoadMat(ref mat1,          "Materials/VHSPro_pass1");
      if(matTape==null)       LoadMat(ref matTape,       "Materials/VHSPro_tape");
      if(matBleed==null)      LoadMat(ref matBleed,      "Materials/VHSPro_bleed");
      if(matFeedback==null)   LoadMat(ref matFeedback,   "Materials/VHSPro_feedback");

      shader1 =         mat1.shader; 
      shaderTape =      matTape.shader; 
      shaderBleed =     matBleed.shader; 
      shaderFeedback =  matFeedback.shader; 

      sheet1 =        context.propertySheets.Get(shader1);
      sheetTape =     context.propertySheets.Get(shaderTape);
      sheetBleed =    context.propertySheets.Get(shaderBleed);
      sheetFeedback = context.propertySheets.Get(shaderFeedback);

      mpb1 =        sheet1.properties;
      mpbTape =     sheetTape.properties;
      mpbBleed =    sheetBleed.properties;
      mpbFeedback = sheetFeedback.properties;


      if(texNull==null) texNull = new RenderTexture(desc);      

      //init textures
      cmd.GetTemporaryRT(texIdPass1,         desc.width, desc.height); 
      texPass1 = new RenderTargetIdentifier(texIdPass1);  

      if(cmpt.tapeNoiseOn.value || cmpt.filmgrainOn.value || cmpt.lineNoiseOn.value){
         context.GetScreenSpaceTemporaryRT(cmd, texIdTape, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Point);
         texTape = new RenderTargetIdentifier(texIdTape);  
      }

      if(cmpt.feedbackOn.value){
         context.GetScreenSpaceTemporaryRT(cmd, texIdFeedback, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Point);
         texFeedback = new RenderTargetIdentifier(texIdFeedback);  

         //if cam res change or 1st pass -> create textures -> keep them for the next frame
         //Note: unity has a bug with renderingData.cameraData.cameraType, 
         //always shows CameraType.Game
         //TODO dont re-create RTs when CameraType.Preview
         if(texFeedbackLast==null || texFeedbackLast.width!=desc.width || texFeedbackLast.height!=desc.height){
            texFeedbackLast = new RenderTexture(desc);
         } 
         if(texLast==null || texLast.width!=desc.width || texLast.height!=desc.height){
            texLast = new RenderTexture(desc);
         }
      }


   }


   //Cleans the temporary RTs when we don't need them anymore
   public void _OnCameraCleanup(CommandBuffer cmd) {
      
      //textures   
      cmd.ReleaseTemporaryRT(texIdPass1);
      cmd.ReleaseTemporaryRT(texIdTape);
      cmd.ReleaseTemporaryRT(texIdFeedback);
      
   }


   //main fn
   public override void Render(PostProcessRenderContext context){

      CommandBuffer cmd = context.command;

      //dont run in scene view 
      if(context.isSceneView) {
         cmd.Blit(context.source, context.destination); //no bleed pass
         return;
      }

      _OnCameraSetup(context);

      RenderTargetIdentifier texSource = context.source;

      if(cmpt.independentTimeOn.value) _time = Time.unscaledTime; 
      else                             _time = Time.time; 


      mpb1.SetFloat("_time",      _time);  
      mpb1.SetVector("_ResOg",    _ResOg);
      mpb1.SetVector("_Res",      _Res);
      mpb1.SetVector("_ResN",     _ResN);

      //Pixelation
      //...

      //Color Decimat1ion
      FeatureToggle(sheet1, mat1, cmpt.colorOn.value, "VHS_COLOR");
      mpb1.SetInt("_colorMode",                cmpt.colorMode.value);
      mpb1.SetInt("_colorSyncedOn",            cmpt.colorSyncedOn.value?1:0);

      mpb1.SetInt("bitsR",                     cmpt.bitsR.value);
      mpb1.SetInt("bitsG",                     cmpt.bitsG.value);
      mpb1.SetInt("bitsB",                     cmpt.bitsB.value);
      mpb1.SetInt("bitsSynced",                cmpt.bitsSynced.value);

      mpb1.SetInt("bitsGray",                  cmpt.bitsGray.value);
      mpb1.SetColor("grayscaleColor",          cmpt.grayscaleColor.value);        

      FeatureToggle(sheet1, mat1, cmpt.ditherOn.value, "VHS_DITHER");       
      mpb1.SetInt("_ditherMode",            cmpt.ditherMode.value);
      mpb1.SetFloat("ditherAmount",         cmpt.ditherAmount.value);


      //Signal Tweak
      FeatureToggle(sheet1, mat1, cmpt.signalTweakOn.value, "VHS_SIGNAL_TWEAK_ON");

      mpb1.SetFloat("signalAdjustY", cmpt.signalAdjustY.value);
      mpb1.SetFloat("signalAdjustI", cmpt.signalAdjustI.value);
      mpb1.SetFloat("signalAdjustQ", cmpt.signalAdjustQ.value);

      mpb1.SetFloat("signalShiftY", cmpt.signalShiftY.value);
      mpb1.SetFloat("signalShiftI", cmpt.signalShiftI.value);
      mpb1.SetFloat("signalShiftQ", cmpt.signalShiftQ.value);


      //Palette
      FeatureToggle(sheet1, mat1, cmpt.paletteOn.value, "VHS_PALETTE");

      if(cmpt.paletteOn.value){

         PalettePreset pal = VHSHelper.GetPalettes()[cmpt.paletteId.value];
            
         Texture2D texPaletteSorted = pal.texSortedPre;            
         cmd.SetGlobalTexture(Shader.PropertyToID("_PaletteTex"), texPaletteSorted);
         mpb1.SetInt("_ResPalette",       pal.texSortedWidth);

         mpb1.SetInt("paletteDelta",           cmpt.paletteDelta.value);

      }

      //VHS 1st Pass (Distortions, Decimations) 
      FeatureToggle(sheet1, mat1, cmpt.filmgrainOn.value, "VHS_FILMGRAIN_ON");
      FeatureToggle(sheet1, mat1, cmpt.tapeNoiseOn.value, "VHS_TAPENOISE_ON");
      FeatureToggle(sheet1, mat1, cmpt.lineNoiseOn.value, "VHS_LINENOISE_ON");


      //Jitter & Twitch
      FeatureToggle(sheet1, mat1, cmpt.jitterHOn.value, "VHS_JITTER_H_ON");
      mpb1.SetFloat("jitterHAmount", cmpt.jitterHAmount.value);

      FeatureToggle(sheet1, mat1, cmpt.jitterVOn.value, "VHS_JITTER_V_ON");
      mpb1.SetFloat("jitterVAmount", cmpt.jitterVAmount.value);
      mpb1.SetFloat("jitterVSpeed", cmpt.jitterVSpeed.value);

      FeatureToggle(sheet1, mat1, cmpt.linesFloatOn.value, "VHS_LINESFLOAT_ON");     
      mpb1.SetFloat("linesFloatSpeed", cmpt.linesFloatSpeed.value);

      FeatureToggle(sheet1, mat1, cmpt.twitchHOn.value, "VHS_TWITCH_H_ON");
      mpb1.SetFloat("twitchHFreq", cmpt.twitchHFreq.value);

      FeatureToggle(sheet1, mat1, cmpt.twitchVOn.value, "VHS_TWITCH_V_ON");
      mpb1.SetFloat("twitchVFreq", cmpt.twitchVFreq.value);

      FeatureToggle(sheet1, mat1, cmpt.scanLinesOn.value, "VHS_SCANLINES_ON");
      mpb1.SetFloat("scanLineWidth", cmpt.scanLineWidth.value);

      FeatureToggle(sheet1, mat1, cmpt.signalNoiseOn.value, "VHS_YIQNOISE_ON");
      mpb1.SetFloat("signalNoisePower", cmpt.signalNoisePower.value);
      mpb1.SetFloat("signalNoiseAmount", cmpt.signalNoiseAmount.value);

      FeatureToggle(sheet1, mat1, cmpt.stretchOn.value, "VHS_STRETCH_ON");

      
      //Noises Pass
      if(cmpt.tapeNoiseOn.value || cmpt.filmgrainOn.value || cmpt.lineNoiseOn.value){

         mpbTape.SetFloat("_time",  _time);  
         mpbTape.SetVector("_ResN", _ResN); //URP

         FeatureToggle(sheetTape, matTape, cmpt.filmgrainOn.value, "VHS_FILMGRAIN_ON");
         mpbTape.SetFloat("filmGrainAmount", cmpt.filmGrainAmount.value);
         
         FeatureToggle(sheetTape, matTape, cmpt.tapeNoiseOn.value, "VHS_TAPENOISE_ON");
         mpbTape.SetFloat("tapeNoiseTH", cmpt.tapeNoiseTH.value);
         mpbTape.SetFloat("tapeNoiseAmount", cmpt.tapeNoiseAmount.value);
         mpbTape.SetFloat("tapeNoiseSpeed", cmpt.tapeNoiseSpeed.value);
         
         FeatureToggle(sheetTape, matTape, cmpt.lineNoiseOn.value, "VHS_LINENOISE_ON");
         mpbTape.SetFloat("lineNoiseAmount", cmpt.lineNoiseAmount.value);
         mpbTape.SetFloat("lineNoiseSpeed", cmpt.lineNoiseSpeed.value);

         //trying with input 
         cmd.BlitFullscreenTriangle(texSource, texTape, sheetTape, 0);
         
         cmd.SetGlobalTexture(Shader.PropertyToID("_TapeTex"), texTape);
         mpb1.SetFloat("tapeNoiseAmount", cmpt.tapeNoiseAmount.value);          

      }


      //VHS 2nd Pass (Bleed)
      mpbBleed.SetFloat("_time",  _time);  
      mpbBleed.SetVector("_ResOg", _ResOg);//  - resolution before pixelation

      //CRT       
      FeatureToggle(sheetBleed, matBleed, cmpt.bleedOn.value, "VHS_BLEED_ON");

      sheetBleed.DisableKeyword("VHS_OLD_THREE_PHASE");
      sheetBleed.DisableKeyword("VHS_THREE_PHASE");
      sheetBleed.DisableKeyword("VHS_TWO_PHASE");    
           if(cmpt.crtMode.value==0){ sheetBleed.EnableKeyword("VHS_OLD_THREE_PHASE"); }
      else if(cmpt.crtMode.value==1){ sheetBleed.EnableKeyword("VHS_THREE_PHASE"); }
      else if(cmpt.crtMode.value==2){ sheetBleed.EnableKeyword("VHS_TWO_PHASE"); }

      matBleed.DisableKeyword("VHS_OLD_THREE_PHASE");
      matBleed.DisableKeyword("VHS_THREE_PHASE");
      matBleed.DisableKeyword("VHS_TWO_PHASE");    
           if(cmpt.crtMode.value==0){ matBleed.EnableKeyword("VHS_OLD_THREE_PHASE"); }
      else if(cmpt.crtMode.value==1){ matBleed.EnableKeyword("VHS_THREE_PHASE"); }
      else if(cmpt.crtMode.value==2){ matBleed.EnableKeyword("VHS_TWO_PHASE"); }

      mpbBleed.SetFloat("bleedAmount", cmpt.bleedAmount.value);


      //1st pass
      //Bypass Texture
      if(cmpt.bypassOn.value==true){
         cmd.SetGlobalTexture(Shader.PropertyToID("_InputTex"), cmpt.bypassTex.value);
      }else{
         cmd.SetGlobalTexture(Shader.PropertyToID("_InputTex"), texSource); //texCam
      }

      //Note: we are using null and _InputTexture, 
      //instead of passing texture directly as _MainTex
      cmd.BlitFullscreenTriangle(texSource, texPass1, sheet1, 0);

      
      if(cmpt.feedbackOn.value){

         //recalc feedback buffer
         mpbFeedback.SetFloat("feedbackThresh",   cmpt.feedbackThresh.value);
         mpbFeedback.SetFloat("feedbackAmount",   cmpt.feedbackAmount.value);
         mpbFeedback.SetFloat("feedbackFade",     cmpt.feedbackFade.value);
         mpbFeedback.SetColor("feedbackColor",    cmpt.feedbackColor.value);

         cmd.SetGlobalTexture(Shader.PropertyToID("_InputTex"),      texPass1);
         cmd.SetGlobalTexture(Shader.PropertyToID("_LastTex"),       texLast);
         cmd.SetGlobalTexture(Shader.PropertyToID("_FeedbackTex"),   texFeedbackLast);


         cmd.BlitFullscreenTriangle(texNull, texFeedback, sheetFeedback, 0);

         cmd.Blit(texFeedback,   texFeedbackLast);  //save prev frame feedback
         cmd.Blit(texPass1,      texLast);          //save prev frame color

      }

      mpbBleed.SetInt("feedbackOn",            cmpt.feedbackOn.value?1:0);
      mpbBleed.SetInt("feedbackDebugOn",       cmpt.feedbackDebugOn.value?1:0);
      if(cmpt.feedbackOn.value || cmpt.feedbackDebugOn.value){
         cmd.SetGlobalTexture(Shader.PropertyToID("_FeedbackTex"),   texFeedback);
      }
      

      //2nd pass
      if(cmpt.bleedOn.value==true){         
         cmd.BlitFullscreenTriangle(texPass1, context.destination, sheetBleed, 0);
      }else{
         //TODO add feedback pass?
         cmd.Blit(texPass1, context.destination); //no bleed pass
      }

      _OnCameraCleanup(cmd);

   }


   //Helper Tools
   void FeatureToggle(PropertySheet sheet, Material mat, bool propVal, string featureName){  

      //turn on/off shader features
      //we need to do both at the same time 
      //sheet is for runtime and mat to keep correct shader_features for the build 

      if(propVal)     {
         mat.EnableKeyword(featureName);
         sheet.EnableKeyword(featureName);
      }else{
         mat.DisableKeyword(featureName);
         sheet.DisableKeyword(featureName);
      }
   }

   void LoadMat(ref Material m, string materialPath){      
      m = Resources.Load<Material>(materialPath);
      if(m==null) 
         Debug.LogError($"Unable to find material '{materialPath}'. Post-Process Volume VHSPro is unable to load.");
   }

}
