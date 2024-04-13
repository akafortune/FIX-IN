using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using VladStorm;

public class SettingsManager : MonoBehaviour
{
    Resolution[] resolutions;
    public TMPro.TMP_Dropdown ResolutionDropdown;

    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider SFXSlider;
    public Slider masterSlider;

    public Toggle fullscreenToggle;
    public Toggle BXToggle;
    public bool BXisToggled;
    public bool CRTtoggle;
    bool codeChange = false;
    public void SetFullcreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void checkFullscreen()
    {
        if (Screen.fullScreen == true)
        {
            fullscreenToggle.isOn = true;
        }
        else
        {
            fullscreenToggle.isOn = false;
        }
    }
    public void checkBXSwap()
    {
        if (PlayerPrefs.HasKey("BXSwap")&&PlayerPrefs.GetString("BXSwap").Equals("True"))
        {
            BXToggle.isOn = true;
        }
        else
        {
            BXToggle.isOn = false;
        }
    }
    public void SetResolution(int resIndex)
    {
        if (!codeChange)
        {
            Resolution res = resolutions[resIndex];
            PlayerPrefs.SetInt("Resolution", resIndex);
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }
    }
    void Start()
    {
        codeChange = true;
        CheckMusic();

        SetMusicVolume();
        CheckSFX();

        SetSFXVolume();
        CheckMaster();

        SetMasterVolume();
        
        checkFullscreen();
        checkBXSwap();
        CheckCRTtoggle();
        resolutions = Screen.resolutions;
        int buffer = -1;
        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        if (PlayerPrefs.HasKey("Resolution"))
        {
            buffer = PlayerPrefs.GetInt("Resolution");
        }
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                if (!PlayerPrefs.HasKey("Resolution"))
                {
                    PlayerPrefs.SetInt("Resolution", i);
                    currentResolutionIndex = i;
                }
            }
        }

        ResolutionDropdown.AddOptions(options);
        
        if(buffer != -1)
        {
            ResolutionDropdown.value = buffer;
        }
        else
        {
            ResolutionDropdown.value = currentResolutionIndex;
        }

        ResolutionDropdown.RefreshShownValue();
        codeChange = false;
        this.gameObject.SetActive(false);

    }

    public void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");

        SetMusicVolume();
    }
    public void LoadSFXVolume()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetSFXVolume();
    }
    public void LoadMasterVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");

        SetSFXVolume();
    }
    public void SetMusicVolume()
    {
        float vol = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("musicVolume", vol);
    }
    public void SetSFXVolume()
    {
        float vol = SFXSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("sfxVolume", vol);
    }
    public void SetMasterVolume()
    {
        float vol = masterSlider.value;
        audioMixer.SetFloat("master", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("masterVolume", vol);
    }
    public void CheckMusic()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume();
        }
    }
    public void CheckSFX()
    {
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolume();
        }
    }
    public void CheckMaster()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            LoadMasterVolume();
        }
        else
        {
            SetMasterVolume();
        }
    }
    public void BXSwap(bool isSwapped)
    {
        BXisToggled = isSwapped;
        PlayerPrefs.SetString("BXSwap", "" + isSwapped);
    }
    public void SetCRTtoggle()
    {
        CRTtoggle = !CRTtoggle;
        if (CRTtoggle) { PlayerPrefs.SetInt("CRT", 1); }
        else { PlayerPrefs.SetInt("CRT", 0); }
        CheckCRTtoggle();
    }
    public void CheckCRTtoggle()
    {
        if (PlayerPrefs.HasKey("CRT"))
        {
            if (PlayerPrefs.GetInt("CRT") == 1) { CRTtoggle = true; }
            else { CRTtoggle = false; }
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3))
            {
                GameObject.Find("CRTCam").GetComponent<PostProcessVolume>().profile.TryGetSettings<VHSPro>(out VHSPro vhp);
                vhp.enabled.value = CRTtoggle;
                GameObject.Find("CRTCam").GetComponent<PostProcessVolume>().profile.TryGetSettings<LensDistortion>(out LensDistortion ld);
                ld.enabled.value = CRTtoggle;
            }
        }
        else
        {
            CRTtoggle = true;
            PlayerPrefs.SetInt("CRT", 1);
            CheckCRTtoggle();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
