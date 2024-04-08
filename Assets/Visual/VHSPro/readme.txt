VHS Pro 3.0 Manual. 

[Intro]

VHS Pro is a camera post-processing effect for Unity 3D which emulates look and feel of retro videocards, graphics adapters and CRT (cathode ray tube) screens. It emulates resolution decimation, signal color encoding and downsampling, screen bleeding and phosphor trail (ray tail/fading on a phosphor screen), VHS tape noise (VHS data corruption), interlacing and jitter (deviation) in the analog video signal, color dithering and color palette matching from existing palettes or your own custom one. It's best for 2D console-like games, 80's/90's/retro/VHS look, security cameras and robot/computer vision and video processing.


[Setup, Installation] 

Use Unity version 2022.2.1f+ 
Use Linear color space (Edit->Project Settings->Player->Color Space: Linear) 

-BRP (Built-in Render Pipeline)
	1. Install Post-Processing
	https://docs.unity3d.com/Packages/com.unity.postprocessing@3.2/manual/Installation.html
	https://docs.unity3d.com/Packages/com.unity.postprocessing@3.2/manual/Quick-start.html
	2. Add VHSPro component to Post-Processing Volume  


-URP (Universal Render Pipeline)
	1. Add VHSPro Render Feature to current URP Asset Renderer (which is selected in Main Camera)
	2. Add VHSPro component to Post-Processing Volume 


-HDRP (High Definition Render Pipeline) 
	1. Go to "Edit->Project Settings" and select the "HDRP Global Settings" tab.
	2. Scroll down until you find the "Custom Post Process Orders" section. This section contains three lists, one for each injection point.
	3. Add VHS Pro to "After Post Process" list so that HDRP can render them.
	4. Add VHS Pro to your Post-Processing Volume. 
	more info https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@14.0/manual/Custom-Post-Process.html 




[Parameters Description]

1. Resolution
Resolution of the output image

	a. Pixelization (Screen Resolution Quantization)
	The picture will be quantized vertically and horizontally. You can choose the amount of pixels for the output image.
	
	"Preset" - You can choose one of the existing resolution presets to set both parameters at once. You can also choose Fullscreen or Custom.
	"Width" - The pixels per width in the output picture
	"Height" - The pixels per height in the output picture


2. Signal Encoding
Nowadays we are used to see million of colors on the monitor because the videocards became very powerful. However, back in the days, the old videocards had very limited memory and data bandwidth. The same story was with the monitors. In 70s and 80s most of the time a videocard could encode only a couple of bits of color per channel. This section allows you to emulate color per channel downsampling. The information each color channel can carry.


	a. Color Encoding
	Different videocard used different way to encode the signal. So, you can choose between few color encoding modes: "Grayscale", "RGB", "YIQ (NTSC)". Each mode allows you to adjust the amount of colors per channel (it's not the same as bits per channels).

		"Type" - Type of color encoding. Colors per channel are different for different color encoding modes.

		• Grayscale type (1 channel)
		"Channel Gray" - colors per one channel
		"Color" - shading color. allows you to colour the output

		• RGB type (3 channels)
		"Channel R,G,B" - allows you to adjust colors per each channel

		• YIQ (NTSC) type (3 channels)
		"Channel Y,I,Q" - allows you to adjust colors per each channel

		"Sync Channels" - You can use this mode to tweak colors per channel simultaneously for RGB, YIQ modes.
		"All Channels" - Adjusts color per channel for all 3 channels in the same time

	b. Dithering
	Dithering creates the illusion of "color depth" in images with a limited color palette. In a dithered image, colors that are not available in the palette are approximated by a diffusion of colored pixels from within the available palette.
	
		"Type" - You can choose between different ways/algorithms of dithering such as 
			'Horizontal Lines', 
			'Vertical Lines', 
			'2x2 Ordered', 
			'2x2 Ordered 2', 
			'Uniform noise', 
			'Triangle noise'

		"Dither Amount" - Allows you to adjust the amount of dithering.

	c. Signal tweak
	YIQ is the color space used by the NTSC color TV system. The analog video signal is transmitted in YIQ and not in RGB. In this section you can adjust the YIQ values. 

		"Shift Y,I,Q" - use these to tweak/shift the values.
		"Adjust Y,I,Q" - Use these to make a permanent adjustment.


3. Palette
Allows you to limit the picture colors by the palette. There's a special algorithm which allows you to use big palettes up to 256-512 colors.

	"Preset" - 	You can use one of the existing color palettes from the the old videocards
		'IBM CGA > Full Palette', 
		'IBM CGA > Palette 1, High Intensity',
		'IBM CGA > Palette 1, Low Intensity',
		'IBM CGA > Palette 2, High Intensity',
		'IBM CGA > Palette 2, Low Intensity Brown', 
		'IBM CGA > Palette 2, Low Intensity Dark Yellow',
		'Atari > NTSC', 
		'Atari > PAL', 
		'Atari > SECAM', 
		'MS DOS > Duke Nukem 3D',
		'MS DOS > Doom 1', 
		'MS DOS > Quake 1', 
		'Nintendo > Gameboy', 
		'Nintendo > NES', 
		'Apple II > Palette Hi',
		'Apple II > Palette Low',
		etc.

	"Accuracy" - This parameter allows you set how accurate the color matching is. Small values work faster but bigger are more precise. This also allow you to make more unique look of the picture.


4. CRT(Cathode Ray Tube) Emulation
Emulates CRT Screen and signal bleeding. 

	a. Bleeding
	"Type" (CRT screen bleeding type) - You can choose between these modes: "Old Three Phase", "Three Phase", "Two Phase" (works slower). These modes emulate different screen bleeding curves. 
	Bleeding is a cathode ray tail on the phosphor screen. When the ray (electron beam) passing certain point of the phosphor screen, the point keep glowing some small amount of time after the ray is already gone. So, the ray has a fading tail behind itself while going across the screen. This effect called bleeding. 
	
	"Stretch" - Makes bleeding curve longer or shorter by stretching the it. 

5. Noise 
Emulates various noises.

	a. Global Resolution (Noise Resolution)
	The noise resolution (quantization). If it's checked then noise resolution will be inherited from the main pixelization resolution.
	Uncheck it if you want to use custom noise resolution. 
	"Width, Height" - Adjust custom noise resolution parameters

	c. Film Grain
	This is a simple background noise. 
	"Alpha" - Adjusts transparency of background noise.

	d. Signal Noise
	Emulates signal noise. Changes the YIQ colors. 
	"Amount" - Adjusts amount of noise.
	"Power" - Adjusts density of noise.

	e. Line Noise
	Emulates noise in the analog video signal and VHS cassettes. Noise lines popping up randomly within the screen.
	"Amount" - Adjusts transparency.
	"Speed" - Adjusts speed.

	f. Tape Noise
	Emulates noise which you can find on old VHS cassettes. Noise lines floating down the screen.
	"Amount" - Adjusts noise cutoff.
	"Speed" - Adjusts speed.
	"Alpha" - Adjusts transparency.


6. Jitter & Twitch
Emulates deviations in the analog video signal and CRT.

	a. Show Scanlines 
	Draws the black lines in between the quantized screenlines.
	"Width" - Adjusts the width of the scanlines.

	b. Floating Lines
	After the screen was quantized vertically it consist of the horizontal lines which will float down the screen if this option is on. It works the best on the low resolution. 

	c. Stretch Noise
	Emulates noise, and data corruption on the VHS cassette plus some CRT jitter. Looks like if some of the screen lines were stretched and floating up and down the screen.

	d. Jitter Horizontal
	Emulates interlacing jitter.
	"Amount" - Adjusts the amount.

	e. Jitter Vertical
	Emulates analog video signal and CRT jitter. Adds a bit of YIQ shifting.
	"Amount" - Adjusts transparency.
	"Speed" - Adjusts speed.

	f. Twitch Horizontal
	Shifts/displaces the image horizontally sometimes.
	"Frequency" - Adjusts how often.

	g. Twitch Vertical
	Shakes/Shifts screen horizontally sometimes. The images "jumps" or "falls" vertically.
	"Frequency" - Adjusts how often.


7. Phosphor trail
Emulates phosphor screen decay. It basically works as a feedback and adds a part of previous frame to the current one. 
For easier understanding of this effect you can turn on "Tools->Debug Trail". Then you will see only the trail. It's better to adjust it that way.

	"Input Cutoff" - Adjusts brightness threshold of input signal. How much of each frame affects the trail. If it equal to X value, it means all pixels with brightness lower than X are not gonna affect the trail and all pixels with brightness above X are going to affect the trail. 
	"Amount" - Amplifies the input amount after cutoff. 
	"Fade" - Adjusts how fast the trail fades. In other words it's a feedback amount.

8. Tools
Additional tools.

	a. Unscaled Time.
	When you are pausing the game using Time.timeScale = 0 (or Application.timeScale = 0) it stops all the animation, sound, etc. It also stops shader timer and it's animation. If you still need shader to run while this sort of pause you can use "unscaled time" feature. The shader will keep running even when your Time.timeScale == 0. It will use Time.unscaledTime instead of Time.time.

	b. Debug Trail
	Helps to debug phosphor trail.


9. Bypass Texture
You can use custom image texture if you want it to be processed by the effect instead of screen image.



[Notes]

- Shader Feature switching in a Build
	I'm following the best practices and using shader features for this plugin. It means that you can play with the plugin in the Editor but after you made a build, all the shader features are baked and can't be changed in the build. You still can tweak the shader parameters tho. 
	If you need to change shader features in a build then you have to use multi_compile instead of shader feature in shaders for the effects you need.
	For example, if you wanna switch film grain in runtime (in the build) then you have to change shader_compile line to the main shader file to this
	#pragma multi_compile ___ VHS_FILMGRAIN_ON

	more info about it here https://docs.unity3d.com/Manual/SL-MultipleProgramVariants.html

-Custom Palette
	if you want to add a custom palette just add it in VHSHelper.cs and Resources/Palettes folder

[Contact]

If you have any questions or want to report a bug, please, write me here
Email: vladstormx@gmail.com
Twitter: https://twitter.com/vladstorm_


