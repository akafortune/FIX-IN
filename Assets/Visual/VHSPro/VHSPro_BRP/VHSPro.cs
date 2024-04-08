using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

// using VladStorm;


// namespace UnityEngine.Rendering.PostProcessing {
namespace VladStorm {

[Serializable]
[PostProcess(typeof(VHSProRenderer), PostProcessEvent.AfterStack, "Custom/VHSPro")]
public sealed class VHSPro : PostProcessEffectSettings {


   //Toggles
   public BoolParameter g_pixel =      new BoolParameter{ value = true };
   public BoolParameter g_color =      new BoolParameter{ value = true };
   public BoolParameter g_palette =    new BoolParameter{ value = true };
   public BoolParameter g_crt =        new BoolParameter{ value = true };
   public BoolParameter g_noise =      new BoolParameter{ value = true };
   public BoolParameter g_jitter =     new BoolParameter{ value = true };
   public BoolParameter g_signal =     new BoolParameter{ value = true };
   public BoolParameter g_feedback =   new BoolParameter{ value = true };
   public BoolParameter g_extra =      new BoolParameter{ value = false };
   public BoolParameter g_bypass =     new BoolParameter{ value = false };   


   //Screen
   public BoolParameter            pixelOn = new BoolParameter{ value = false };
   public IntParameter             screenResPresetId = new IntParameter{ value = 2 }; //(2);
   public IntParameter             screenWidth = new IntParameter{ value = 640 };      //NoInterpIntParameter(640);
   public IntParameter             screenHeight = new IntParameter{ value = 480 };     //NoInterpIntParameter(480);

   //Color encoding
   public BoolParameter            colorOn = new BoolParameter{ value = false };       //(false);
   public IntParameter             colorMode = new IntParameter{ value = 2 };          //(2);
   public BoolParameter            colorSyncedOn = new BoolParameter{ value = true };  //(true);
   [Range(0,255)] public IntParameter  bitsR = new IntParameter{ value = 2 };         //ClampedIntParameter 2,
   [Range(0,255)] public IntParameter  bitsG = new IntParameter{ value = 2 };         //ClampedIntParameter 2,0,255
   [Range(0,255)] public IntParameter  bitsB = new IntParameter{ value = 2 };         //ClampedIntParameter 2,0,255
   [Range(0,255)] public IntParameter  bitsSynced = new IntParameter{ value = 2 };    //ClampedIntParameter 2,0,255
   [Range(0,255)] public IntParameter  bitsGray = new IntParameter{ value = 1 };      //ClampedIntParameter 1,0,255
   public ColorParameter           grayscaleColor = new ColorParameter{ value = Color.white }; //(Color.white);

   //Dither
   public BoolParameter            ditherOn = new BoolParameter{ value = false };    //(false);
   public IntParameter             ditherMode = new IntParameter{ value = 3 };       //NoInterpIntParameter 2
   [Range(-1f,3f)] public FloatParameter           ditherAmount = new FloatParameter{ value = 1f };  //ClampedFloatParameter(1f, -1f, 3f);

   //Palette
   public BoolParameter                paletteOn = new BoolParameter{ value = false };  //(false);
   public IntParameter                 paletteId = new IntParameter{ value = 0 };       //NoInterpIntParameter 0
   [Range(0,30)] public IntParameter   paletteDelta = new IntParameter{ value = 5 };    //ClampedIntParameter(5,0,30);
   public TextureParameter             paletteTex = new TextureParameter{ value = null }; //(null);
   
   public PalettePreset paletteCustom; 
   public string paletteCustomName = ""; //for automatic update when drag and drop texture
   public bool paletteCustomInit = false; 

   //crt
   public BoolParameter    bleedOn  = new BoolParameter{ value = false };     //(false); 
   public IntParameter     crtMode = new IntParameter{ value = 0 };            //NoInterpIntParameter 0
   [Range(0f,5f)] public FloatParameter bleedAmount  = new FloatParameter{ value = 1f };  //ClampedFloatParameter(1f, 0f, 5f)


   //Noise
   public BoolParameter noiseResGlobal  = new BoolParameter{ value = true };  //(true); 
   public IntParameter noiseResWidth = new IntParameter{ value = 640 };       //NoInterpIntParameter(640)
   public IntParameter noiseResHeight = new IntParameter{ value = 480 };      //NoInterpIntParameter(480)

   public BoolParameter filmgrainOn  = new BoolParameter{ value = false };       //(false);
   [Range(0f,1f)] public FloatParameter filmGrainAmount = new FloatParameter{ value = 0.016f }; //new ClampedFloatParameter(0.016f, 0f, 1f); 

   public BoolParameter signalNoiseOn  = new BoolParameter{ value = false };       //(false);
   [Range(0f,1f)] public FloatParameter signalNoiseAmount = new FloatParameter{ value = .3f };    //new ClampedFloatParameter(0.3f, 0f, 1f);
   [Range(0f,1f)] public FloatParameter signalNoisePower  = new FloatParameter{ value = .83f };   //new ClampedFloatParameter(0.83f, 0f, 1f);

   public BoolParameter lineNoiseOn  = new BoolParameter{ value = false };    //(false);
   [Range(0f,10f)] public FloatParameter lineNoiseAmount = new FloatParameter{ value = 1f };  //new ClampedFloatParameter(1f, 0f, 10f);
   [Range(0f,10f)] public FloatParameter lineNoiseSpeed = new FloatParameter{ value = 5f };   //new ClampedFloatParameter(5f, 0f, 10f);

   public BoolParameter tapeNoiseOn  = new BoolParameter{ value = false };    //(false);
   [Range(0f,1.5f)] public FloatParameter tapeNoiseTH = new FloatParameter{ value = .63f };    //new ClampedFloatParameter(0.63f, 0f, 1.5f);
   [Range(0f,1.5f)] public FloatParameter tapeNoiseAmount = new FloatParameter{ value = 1f };  //new ClampedFloatParameter(1f, 0f, 1.5f); 
   [Range(0f,1.5f)] public FloatParameter tapeNoiseSpeed = new FloatParameter{ value = 1f };   //new ClampedFloatParameter(1f, 0f, 1.5f);

   //Jitter
   public BoolParameter scanLinesOn  = new BoolParameter{ value = false };    //(false);
   [Range(0f,20f)] public FloatParameter scanLineWidth = new FloatParameter{ value = 10f };   //new ClampedFloatParameter(10f,0f,20f);
   
   public BoolParameter linesFloatOn  = new BoolParameter{ value = false };   //(false); 
   [Range(-3f,3f)] public FloatParameter linesFloatSpeed = new FloatParameter{ value = 1f };  //new ClampedFloatParameter(1f,-3f,3f);
   public BoolParameter stretchOn  = new BoolParameter{ value = false };      //(false);

   public BoolParameter jitterHOn  = new BoolParameter{ value = false };      //(false);
   [Range(0f,5f)] public FloatParameter jitterHAmount = new FloatParameter{ value = .5f };   //new ClampedFloatParameter(.5f,0f,5f);
   public BoolParameter jitterVOn  = new BoolParameter{ value = false };      //(false); 
   [Range(0f,15f)] public FloatParameter jitterVAmount = new FloatParameter{ value = 1f };    //new ClampedFloatParameter(1f,0f,15f);
   [Range(0f,5f)] public FloatParameter jitterVSpeed = new FloatParameter{ value = 1f };     //new ClampedFloatParameter(1f,0f,5f);

   public BoolParameter twitchHOn  = new BoolParameter{ value = false };   //(false);
   [Range(0f,5f)] public FloatParameter twitchHFreq = new FloatParameter{ value = 1f };   //new ClampedFloatParameter(1f,0f,5f);
   public BoolParameter twitchVOn  = new BoolParameter{ value = false };   //(false);
   [Range(0f,5f)] public FloatParameter twitchVFreq = new FloatParameter{ value = 1f };   //new ClampedFloatParameter(1f,0f,5f);
    
   //Signal Tweak
   public BoolParameter signalTweakOn  = new BoolParameter{ value = false };  //(false); 
   [Range(-.25f,.25f)] public FloatParameter signalAdjustY = new FloatParameter{ value = 0f };    //new ClampedFloatParameter(0f,-0.25f, 0.25f);
   [Range(-.25f,.25f)] public FloatParameter signalAdjustI = new FloatParameter{ value = 0f };    //new ClampedFloatParameter(0f,-0.25f, 0.25f);
   [Range(-.25f,.25f)] public FloatParameter signalAdjustQ = new FloatParameter{ value = 0f };    //new ClampedFloatParameter(0f,-0.25f, 0.25f);
   [Range(-2f,2f)] public FloatParameter signalShiftY = new FloatParameter{ value = 1f };     //new ClampedFloatParameter(1f,-2f, 2f);
   [Range(-2f,2f)] public FloatParameter signalShiftI = new FloatParameter{ value = 1f };     //new ClampedFloatParameter(1f,-2f, 2f);
   [Range(-2f,2f)] public FloatParameter signalShiftQ = new FloatParameter{ value = 1f };     //new ClampedFloatParameter(1f,-2f, 2f);

   //Feedback
   public BoolParameter feedbackOn  = new BoolParameter{ value = false };        //(false); 
   [Range(0f,1f)] public FloatParameter feedbackThresh = new FloatParameter{ value = .1f };     //new ClampedFloatParameter(.1f, 0f, 1f);
   [Range(0f,3f)] public FloatParameter feedbackAmount = new FloatParameter{ value = 2.0f };    //new ClampedFloatParameter(2.0f, 0f, 3f);  
   [Range(0f,1f)] public FloatParameter feedbackFade = new FloatParameter{ value = .82f };      //new ClampedFloatParameter(.82f, 0f, 1f);
   public ColorParameter feedbackColor = new ColorParameter{ value = new Color(1f,.5f,0f) }; //(new Color(1f,.5f,0f)); 
   public BoolParameter feedbackDebugOn  = new BoolParameter{ value = false };   //(false); 


   //Tools 
   public BoolParameter independentTimeOn  = new BoolParameter{ value = false };    //(false); 

   //Bypass     
   public BoolParameter            bypassOn = new BoolParameter{ value = false };      //(false);  
   public TextureParameter         bypassTex = new TextureParameter{ value = null };   //(null);


   public override bool IsEnabledAndSupported(PostProcessRenderContext context) {

      if (enabled.value==false) return false;

      //everything is off by default
      if(pixelOn.value==false &&
         colorOn.value==false &&
         ditherOn.value==false &&
         paletteOn.value==false &&
         bleedOn.value==false &&
         filmgrainOn.value==false &&
         signalNoiseOn.value==false &&
         lineNoiseOn.value==false &&
         tapeNoiseOn.value==false &&
         scanLinesOn.value==false &&
         linesFloatOn.value==false &&
         jitterHOn.value==false &&
         jitterVOn.value==false &&
         twitchHOn.value==false &&
         twitchVOn.value==false &&
         signalTweakOn.value==false &&
         feedbackOn.value==false &&
         bypassOn.value==false) {
         return false;
      }

      return true;

   }

}
}
