using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine.Serialization;

using VladStorm;


[PostProcessEditor(typeof(VHSPro))]
public sealed class VHSProEditor : PostProcessEffectEditor<VHSPro> {
	
	//toggles
	SerializedParameterOverride g_pixel;
	SerializedParameterOverride g_color;
	SerializedParameterOverride g_palette;
	SerializedParameterOverride g_bypass;		
	SerializedParameterOverride g_crt;
	SerializedParameterOverride g_noise;
	SerializedParameterOverride g_jitter;
	SerializedParameterOverride g_signal;
	SerializedParameterOverride g_feedback;
	SerializedParameterOverride g_extra;

	//screen
	SerializedParameterOverride pixelOn;
	SerializedParameterOverride screenResPresetId;
	SerializedParameterOverride screenWidth;
	SerializedParameterOverride screenHeight;


	//color
	SerializedParameterOverride colorOn;
	SerializedParameterOverride colorMode;
	SerializedParameterOverride colorSyncedOn;
	SerializedParameterOverride bitsSynced;
	SerializedParameterOverride bitsR;
	SerializedParameterOverride bitsG;
	SerializedParameterOverride bitsB;
	SerializedParameterOverride bitsGray;
	SerializedParameterOverride grayscaleColor;

	//dither
	SerializedParameterOverride ditherOn;
	SerializedParameterOverride ditherMode;
	SerializedParameterOverride ditherAmount;


	//palette
	SerializedParameterOverride paletteOn;
	SerializedParameterOverride paletteId;
	SerializedParameterOverride paletteDelta;
	SerializedParameterOverride paletteTex;


	//CRT
	SerializedParameterOverride bleedOn; 
	SerializedParameterOverride crtMode; 
	SerializedParameterOverride bleedAmount;

	//NOISE
	SerializedParameterOverride noiseResGlobal;
	SerializedParameterOverride noiseResWidth;
	SerializedParameterOverride noiseResHeight;		

	SerializedParameterOverride filmgrainOn;
	SerializedParameterOverride filmGrainAmount; 
	SerializedParameterOverride tapeNoiseOn;
	SerializedParameterOverride tapeNoiseTH; 
	SerializedParameterOverride tapeNoiseAmount; 
	SerializedParameterOverride tapeNoiseSpeed; 
	SerializedParameterOverride lineNoiseOn;
	SerializedParameterOverride lineNoiseAmount; 
	SerializedParameterOverride lineNoiseSpeed; 


	//JITTER
	SerializedParameterOverride scanLinesOn;
	SerializedParameterOverride scanLineWidth;
	
	SerializedParameterOverride linesFloatOn; 
	SerializedParameterOverride linesFloatSpeed; 
	SerializedParameterOverride stretchOn;

	SerializedParameterOverride twitchHOn; 
	SerializedParameterOverride twitchHFreq; 
	SerializedParameterOverride twitchVOn; 
	SerializedParameterOverride twitchVFreq; 

	SerializedParameterOverride jitterHOn; 
	SerializedParameterOverride jitterHAmount; 
	SerializedParameterOverride jitterVOn; 
	SerializedParameterOverride jitterVAmount; 
	SerializedParameterOverride jitterVSpeed; 
	

	//SIGNAL TWEAK
	SerializedParameterOverride signalTweakOn; 
	SerializedParameterOverride signalAdjustY; 
	SerializedParameterOverride signalAdjustI; 
	SerializedParameterOverride signalAdjustQ; 

	SerializedParameterOverride signalShiftY; 
	SerializedParameterOverride signalShiftI; 
	SerializedParameterOverride signalShiftQ; 

	SerializedParameterOverride signalNoiseOn; 
	SerializedParameterOverride signalNoiseAmount; 
	SerializedParameterOverride signalNoisePower; 


	//FEEDBACK
	SerializedParameterOverride feedbackOn; 

	SerializedParameterOverride feedbackAmount; 
	SerializedParameterOverride feedbackFade; 
	SerializedParameterOverride feedbackColor; 	

	SerializedParameterOverride feedbackThresh; 	
	SerializedParameterOverride feedbackDebugOn; 	


	//TOOLS
	SerializedParameterOverride independentTimeOn; 


	//bypass texture
	SerializedParameterOverride bypassOn;	
	SerializedParameterOverride bypassTex;	


	public override void OnEnable() {

		base.OnEnable();

		g_pixel = 			FindParameterOverride(x => x.g_pixel);
		g_color = 			FindParameterOverride(x => x.g_color);
		g_palette = 		FindParameterOverride(x => x.g_palette);
		g_bypass = 			FindParameterOverride(x => x.g_bypass);
		g_crt = 				FindParameterOverride(x => x.g_crt);
		g_noise = 			FindParameterOverride(x => x.g_noise);
		g_jitter = 			FindParameterOverride(x => x.g_jitter);
		g_signal = 			FindParameterOverride(x => x.g_signal);
		g_feedback = 		FindParameterOverride(x => x.g_feedback);
		g_extra = 			FindParameterOverride(x => x.g_extra);
		g_bypass = 			FindParameterOverride(x => x.g_bypass);


		//screen
		pixelOn = 				FindParameterOverride(x => x.pixelOn);
		screenWidth = 			FindParameterOverride(x => x.screenWidth);
		screenHeight = 		FindParameterOverride(x => x.screenHeight);

		//color 		
		colorOn = 				FindParameterOverride(x => x.colorOn);
		colorMode = 			FindParameterOverride(x => x.colorMode);
		colorSyncedOn = 		FindParameterOverride(x => x.colorSyncedOn);

		bitsGray = 				FindParameterOverride(x => x.bitsGray);
		bitsSynced = 			FindParameterOverride(x => x.bitsSynced);
		bitsR = 					FindParameterOverride(x => x.bitsR);
		bitsG = 					FindParameterOverride(x => x.bitsG);
		bitsB = 					FindParameterOverride(x => x.bitsB);
		grayscaleColor = 		FindParameterOverride(x => x.grayscaleColor);

		//dither
		ditherOn = 				FindParameterOverride(x => x.ditherOn);
		ditherMode = 			FindParameterOverride(x => x.ditherMode);
		ditherAmount = 		FindParameterOverride(x => x.ditherAmount);


		//palette
		paletteOn = 			FindParameterOverride(x => x.paletteOn);
		paletteId = 			FindParameterOverride(x => x.paletteId);
		paletteDelta = 		FindParameterOverride(x => x.paletteDelta);
		paletteTex = 			FindParameterOverride(x => x.paletteTex);


		//CRT
		bleedOn = 				FindParameterOverride(x => x.bleedOn); 
		crtMode = 				FindParameterOverride(x => x.crtMode); 
		screenResPresetId = 	FindParameterOverride(x => x.screenResPresetId);
		bleedAmount = 			FindParameterOverride(x => x.bleedAmount);


		//NOISE
		noiseResGlobal = 		FindParameterOverride(x => x.noiseResGlobal);
		noiseResWidth = 		FindParameterOverride(x => x.noiseResWidth); 
		noiseResHeight = 		FindParameterOverride(x => x.noiseResHeight); 

		filmgrainOn = 			FindParameterOverride(x => x.filmgrainOn);
		filmGrainAmount = 	FindParameterOverride(x => x.filmGrainAmount); 
		tapeNoiseOn = 			FindParameterOverride(x => x.tapeNoiseOn);
		tapeNoiseTH = 			FindParameterOverride(x => x.tapeNoiseTH); 
		tapeNoiseAmount = 	FindParameterOverride(x => x.tapeNoiseAmount); 
		tapeNoiseSpeed = 		FindParameterOverride(x => x.tapeNoiseSpeed); 
		lineNoiseOn = 			FindParameterOverride(x => x.lineNoiseOn);
		lineNoiseAmount = 	FindParameterOverride(x => x.lineNoiseAmount); 
		lineNoiseSpeed = 		FindParameterOverride(x => x.lineNoiseSpeed); 


		//JITTER
		scanLinesOn = 			FindParameterOverride(x => x.scanLinesOn);
		scanLineWidth = 		FindParameterOverride(x => x.scanLineWidth);
		
		linesFloatOn = 		FindParameterOverride(x => x.linesFloatOn); 
		linesFloatSpeed = 	FindParameterOverride(x => x.linesFloatSpeed); 
		stretchOn = 			FindParameterOverride(x => x.stretchOn);

		twitchHOn = 			FindParameterOverride(x => x.twitchHOn); 
		twitchHFreq = 			FindParameterOverride(x => x.twitchHFreq); 
		twitchVOn = 			FindParameterOverride(x => x.twitchVOn); 
		twitchVFreq = 			FindParameterOverride(x => x.twitchVFreq); 

		jitterHOn = 			FindParameterOverride(x => x.jitterHOn); 
		jitterHAmount = 		FindParameterOverride(x => x.jitterHAmount); 
		jitterVOn = 			FindParameterOverride(x => x.jitterVOn); 
		jitterVAmount = 		FindParameterOverride(x => x.jitterVAmount); 
		jitterVSpeed = 		FindParameterOverride(x => x.jitterVSpeed); 
		

		//SIGNAL TWEAK
		signalTweakOn = 		FindParameterOverride(x => x.signalTweakOn); 
		signalAdjustY = 		FindParameterOverride(x => x.signalAdjustY); 
		signalAdjustI = 		FindParameterOverride(x => x.signalAdjustI); 
		signalAdjustQ = 		FindParameterOverride(x => x.signalAdjustQ); 

		signalShiftY = 		FindParameterOverride(x => x.signalShiftY); 
		signalShiftI = 		FindParameterOverride(x => x.signalShiftI); 
		signalShiftQ = 		FindParameterOverride(x => x.signalShiftQ); 

		signalNoiseOn = 		FindParameterOverride(x => x.signalNoiseOn); 
		signalNoiseAmount = 	FindParameterOverride(x => x.signalNoiseAmount); 
		signalNoisePower = 	FindParameterOverride(x => x.signalNoisePower); 

		// gammaCorection = 		FindParameterOverride(x => x.gammaCorection); 


		//FEEDBACK
		feedbackOn = 			FindParameterOverride(x => x.feedbackOn); 

		feedbackAmount = 		FindParameterOverride(x => x.feedbackAmount); 
		feedbackFade = 		FindParameterOverride(x => x.feedbackFade); 
		feedbackColor = 		FindParameterOverride(x => x.feedbackColor); 

		feedbackThresh = 		FindParameterOverride(x => x.feedbackThresh); 
		feedbackDebugOn = 	FindParameterOverride(x => x.feedbackDebugOn); 


		//TOOLS
		independentTimeOn = 	FindParameterOverride(x => x.independentTimeOn); 
		
		//custom tex
		bypassOn = 				FindParameterOverride(x => x.bypassOn);
		bypassTex = 			FindParameterOverride(x => x.bypassTex);


	}

	public override void OnInspectorGUI() {


		GUIStyle boldFoldout = new GUIStyle(EditorStyles.foldout); // EditorStyles.miniLabel
		boldFoldout.font = EditorStyles.miniFont;
		boldFoldout.fontSize = 10;
		// boldFoldout.fontStyle = FontStyle.Bold;

		
		g_pixel.value.boolValue = EditorGUILayout.Foldout(g_pixel.value.boolValue, "Resolution", boldFoldout);
		if(g_pixel.value.boolValue){

			PropertyField(pixelOn, EditorGUIUtility.TrTextContent("Pixelization", ""));
   		indP();

         // using (new EditorGUILayout.HorizontalScope()) { DrawOverrideCheckbox(screenResPresetId);
    		// using (new EditorGUI.DisabledScope(!screenResPresetId.overrideState.boolValue)) {
			
		   	screenResPresetId.value.intValue = EditorGUILayout.Popup("Preset", screenResPresetId.value.intValue, 
		   		VHSHelper.GetResPresetNames());
        	// }}

   		if(VHSHelper.GetResPresets()[screenResPresetId.value.intValue].isCustom){
				PropertyField(screenWidth, EditorGUIUtility.TrTextContent("Width", ""));
				PropertyField(screenHeight, EditorGUIUtility.TrTextContent("Height", ""));
      	}
      	indM();
      	EditorGUILayout.Space();
			
			
		}

		
		//color
		g_color.value.boolValue = EditorGUILayout.Foldout(g_color.value.boolValue, "Signal Encoding", boldFoldout); //Color Encoding & Downsampling
		if(g_color.value.boolValue){

			PropertyField(colorOn, EditorGUIUtility.TrTextContent("Color Encoding", ""));

				indP();
            // using (new EditorGUILayout.HorizontalScope()) { DrawOverrideCheckbox(colorMode);
       		// using (new EditorGUI.DisabledScope(!colorMode.overrideState.boolValue)) {

					colorMode.value.intValue = EditorGUILayout.Popup("Type", colorMode.value.intValue, 
							VHSHelper.colorModes);

				// }}

				if(colorMode.value.intValue==0){
					
					PropertyField(bitsGray, EditorGUIUtility.TrTextContent("Channel Gray", ""));
					// bitsGray.intValue = 	EditorGUILayout.Slider("Channel Gray", bitsGray.intValue, 0, 255); //BRP
					PropertyField(grayscaleColor, EditorGUIUtility.TrTextContent("Color", ""));

				}

				if(colorMode.value.intValue==1){

					PropertyField(colorSyncedOn, EditorGUIUtility.TrTextContent("Sync Channels", ""));
					if(colorSyncedOn.value.boolValue){
						PropertyField(bitsSynced, EditorGUIUtility.TrTextContent("All Channels", ""));
					}else{
						PropertyField(bitsR, EditorGUIUtility.TrTextContent("Channel R", ""));
						PropertyField(bitsG, EditorGUIUtility.TrTextContent("Channel G", ""));
						PropertyField(bitsB, EditorGUIUtility.TrTextContent("Channel B", ""));
					}

				}

				if(colorMode.value.intValue==2){
					PropertyField(colorSyncedOn, EditorGUIUtility.TrTextContent("Sync Channels", ""));
					if(colorSyncedOn.value.boolValue){
						PropertyField(bitsSynced, EditorGUIUtility.TrTextContent("All Channels", ""));
					}else{
						PropertyField(bitsR, EditorGUIUtility.TrTextContent("Channel Y", ""));
						PropertyField(bitsG, EditorGUIUtility.TrTextContent("Channel I", ""));
						PropertyField(bitsB, EditorGUIUtility.TrTextContent("Channel Q", ""));							
					}	
				}	
					
				indM();
				EditorGUILayout.Space();


        
	      //SIGNAL
	   	PropertyField(signalTweakOn, EditorGUIUtility.TrTextContent("Signal Tweak", ""));

				indP(); 
				PropertyField(signalAdjustY, EditorGUIUtility.TrTextContent("Shift Y", ""));
				PropertyField(signalAdjustI, EditorGUIUtility.TrTextContent("Shift I", ""));
				PropertyField(signalAdjustQ, EditorGUIUtility.TrTextContent("Shift Q", ""));
				PropertyField(signalShiftY, EditorGUIUtility.TrTextContent("Adjust Y", ""));
				PropertyField(signalShiftI, EditorGUIUtility.TrTextContent("Adjust I", ""));
				PropertyField(signalShiftQ, EditorGUIUtility.TrTextContent("Adjust Q", ""));
			   indM();
		   	EditorGUILayout.Space();



		   PropertyField(ditherOn, EditorGUIUtility.TrTextContent("Dithering", ""));

		   	indP();
            // using (new EditorGUILayout.HorizontalScope()) { DrawOverrideCheckbox(ditherMode);
       		// using (new EditorGUI.DisabledScope(!ditherMode.overrideState.boolValue)) {

				   ditherMode.value.intValue = EditorGUILayout.Popup("Type", ditherMode.value.intValue, VHSHelper.ditherModes);

			   // }}

			   if(ditherMode.value.intValue!=0){
					PropertyField(ditherAmount, EditorGUIUtility.TrTextContent("Amount", ""));						   	
			   }  
			   indM();
			   EditorGUILayout.Space(); 			   	

		} //color

		

		//palette
		g_palette.value.boolValue = EditorGUILayout.Foldout(g_palette.value.boolValue, "Palette", boldFoldout);
		if(g_palette.value.boolValue){
				
			PropertyField(paletteOn, EditorGUIUtility.TrTextContent("Enable", ""));

			indP();
         // using (new EditorGUILayout.HorizontalScope()) { DrawOverrideCheckbox(paletteId);
    		// using (new EditorGUI.DisabledScope(!paletteId.overrideState.boolValue)) {

				// string[] paletteNames = 		VHSHelper.GetPaletteNames();
			   paletteId.value.intValue = EditorGUILayout.Popup("Preset", paletteId.value.intValue, 
			   	VHSHelper.GetPaletteNames());
		   
		   // }}

		   PropertyField(paletteDelta, EditorGUIUtility.TrTextContent("Accruacy", ""));
		   /*
		   if(VHSHelper.GetPalettes()[paletteId.value.intValue].isCustom){
				PropertyField(paletteTex, EditorGUIUtility.TrTextContent("Custom Palette", ""));
			}
			*/
			indM();
			EditorGUILayout.Space();

		}

      //CRT
	   g_crt.value.boolValue = EditorGUILayout.Foldout(g_crt.value.boolValue, "CRT Emulation", boldFoldout);
	   if(g_crt.value.boolValue){

	   	PropertyField(bleedOn, EditorGUIUtility.TrTextContent("Bleeding", ""));
	   	
	   	indP();
         // using (new EditorGUILayout.HorizontalScope()) { DrawOverrideCheckbox(crtMode);
    		// using (new EditorGUI.DisabledScope(!crtMode.overrideState.boolValue)) {

			   crtMode.value.intValue = EditorGUILayout.Popup("Type", crtMode.value.intValue,  
			   		new string[3] {"Old Three Phase", "Three Phase", "Two Phase (slow)"}); //, "Custom Curve"
			// }}

		   PropertyField(bleedAmount, EditorGUIUtility.TrTextContent("Stretch", ""));	//URP
			indM();

		}


	   //NOISE
	   g_noise.value.boolValue = 		EditorGUILayout.Foldout(g_noise.value.boolValue, "Noise", boldFoldout);
	   if(g_noise.value.boolValue){		   
			
   		PropertyField(noiseResGlobal, EditorGUIUtility.TrTextContent("Global Resolution", ""));
      		indP();
	      	if(noiseResGlobal.value.boolValue==false) {
	      		PropertyField(noiseResWidth, EditorGUIUtility.TrTextContent("Width", ""));
	      		PropertyField(noiseResHeight, EditorGUIUtility.TrTextContent("Height", ""));
	      	}
      		indM();
				EditorGUILayout.Space();	   
		   

		   PropertyField(filmgrainOn, EditorGUIUtility.TrTextContent("Film Grain", ""));
			   indP();
			   PropertyField(filmGrainAmount, EditorGUIUtility.TrTextContent("Alpha", ""));
			   indM();
			   EditorGUILayout.Space();

		  	PropertyField(signalNoiseOn, EditorGUIUtility.TrTextContent("Signal Noise", ""));
			   indP();
			   PropertyField(signalNoiseAmount, EditorGUIUtility.TrTextContent("Amount", ""));
			   PropertyField(signalNoisePower, EditorGUIUtility.TrTextContent("Power", ""));
			   indM();
			   EditorGUILayout.Space();

			PropertyField(lineNoiseOn, EditorGUIUtility.TrTextContent("Line Noise", ""));
		   	indP();
		   	PropertyField(lineNoiseAmount, EditorGUIUtility.TrTextContent("Alpha", ""));
		   	PropertyField(lineNoiseSpeed, EditorGUIUtility.TrTextContent("Speed", ""));
		   	indM();
		   	EditorGUILayout.Space();

		   PropertyField(tapeNoiseOn, EditorGUIUtility.TrTextContent("Tape Noise", ""));	
				indP();
				PropertyField(tapeNoiseTH, EditorGUIUtility.TrTextContent("Amount", ""));
				PropertyField(tapeNoiseSpeed, EditorGUIUtility.TrTextContent("Speed", ""));
				PropertyField(tapeNoiseAmount, EditorGUIUtility.TrTextContent("Alpha", ""));
			   indM();
				EditorGUILayout.Space();	   
		}


      //JITTER
	   g_jitter.value.boolValue = EditorGUILayout.Foldout(g_jitter.value.boolValue, "Jitter & Twitch", boldFoldout);
	   if(g_jitter.value.boolValue){

	   	PropertyField(scanLinesOn, EditorGUIUtility.TrTextContent("Show Scanlines", ""));
			   indP();
			   PropertyField(scanLineWidth, EditorGUIUtility.TrTextContent("Width", ""));
			   indM();
				EditorGUILayout.Space();			

			PropertyField(linesFloatOn, EditorGUIUtility.TrTextContent("Floating Lines", ""));
				indP();
				PropertyField(linesFloatSpeed, EditorGUIUtility.TrTextContent("Speed", ""));
				indM();
				EditorGUILayout.Space();

			PropertyField(stretchOn, EditorGUIUtility.TrTextContent("Stretch Noise", ""));
				EditorGUILayout.Space();

			PropertyField(jitterHOn, EditorGUIUtility.TrTextContent("Interlacing", ""));
	      	indP();
	      	PropertyField(jitterHAmount, EditorGUIUtility.TrTextContent("Amount", ""));
	      	indM();
	      	EditorGUILayout.Space();

			PropertyField(jitterVOn, EditorGUIUtility.TrTextContent("Jitter", ""));		      	
	      	indP();
	      	PropertyField(jitterVAmount, EditorGUIUtility.TrTextContent("Amount", ""));
	      	PropertyField(jitterVSpeed, EditorGUIUtility.TrTextContent("Speed", ""));
		      indM();
      		EditorGUILayout.Space();

      	PropertyField(twitchHOn, EditorGUIUtility.TrTextContent("Twitch Horizontal", ""));
				indP();
				PropertyField(twitchHFreq, EditorGUIUtility.TrTextContent("Frequency", ""));
	      	indM();
	      	EditorGUILayout.Space();

      	PropertyField(twitchVOn, EditorGUIUtility.TrTextContent("Twitch Vertical", ""));	
				indP();
				PropertyField(twitchVFreq, EditorGUIUtility.TrTextContent("Frequency", ""));
		      indM();
				EditorGUILayout.Space();
   	}




	   //FEEDBACK
	   g_feedback.value.boolValue = EditorGUILayout.Foldout(g_feedback.value.boolValue, "Phosphor Trail", boldFoldout);
	   if(g_feedback.value.boolValue){

	   	PropertyField(feedbackOn, EditorGUIUtility.TrTextContent("Phosphor Trail", ""));

				indP();   
				PropertyField(feedbackThresh, EditorGUIUtility.TrTextContent("Input Cutoff", ""));
				PropertyField(feedbackAmount, EditorGUIUtility.TrTextContent("Amount", ""));
				PropertyField(feedbackFade, EditorGUIUtility.TrTextContent("Fade", ""));
				PropertyField(feedbackColor, EditorGUIUtility.TrTextContent("Color", ""));
				indM();
				EditorGUILayout.Space();
		}


		//TOOLS
	   g_extra.value.boolValue = EditorGUILayout.Foldout(g_extra.value.boolValue, "Tools", boldFoldout);
	   if(g_extra.value.boolValue){
	   	indP(); 
	   	PropertyField(independentTimeOn, EditorGUIUtility.TrTextContent("Use unscaled time", ""));
	   	PropertyField(feedbackDebugOn, EditorGUIUtility.TrTextContent("Debug Trail", ""));
	   	indM();
	   	EditorGUILayout.Space();
	   }


      //BYPASS
		g_bypass.value.boolValue = EditorGUILayout.Foldout(g_bypass.value.boolValue, "Use Bypass Texture", boldFoldout);
		if(g_bypass.value.boolValue){
			indP(); 
			PropertyField(bypassOn, EditorGUIUtility.TrTextContent("Enable", ""));
			PropertyField(bypassTex, EditorGUIUtility.TrTextContent("Bypass Texture", ""));
			indM();
	   	EditorGUILayout.Space();
		}


		
	}

   //Helpers
   void indP(){ EditorGUI.indentLevel+=2; }
   void indM(){ EditorGUI.indentLevel-=2; }		

}

