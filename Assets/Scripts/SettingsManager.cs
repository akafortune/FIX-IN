using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    Resolution[] resolutions;
    public TMPro.TMP_Dropdown ResolutionDropdown;

    
    public void SetFullcreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    void Start()
    {
        Screen.fullScreen = true;
        resolutions = Screen.resolutions;

        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int StartingResolution = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            if (options.Contains(option))
            continue;
            options.Add(option);
           

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                StartingResolution = i;
            }
        }
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = StartingResolution;
        SetResolution(StartingResolution);
        ResolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
