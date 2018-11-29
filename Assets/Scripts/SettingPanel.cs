using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour {

    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public Dropdown resolutionDropDown;


    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "X" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width==Screen.currentResolution.width
                && resolutions[i].height==Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    public void SetResolution(int resolutioIndex)
    {
        Resolution resolution = resolutions[resolutioIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolum(float volum)
    {
        audioMixer.SetFloat("volum", volum);
        //Debug.Log(volum);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


}
