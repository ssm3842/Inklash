using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using TMPro;

public class SettingManger : MonoBehaviour
{
    //public AudioMixer masterMixer; 
    //public Slider soundSlider;     

    public Button prevButton; 
    public Button nextButton; 
    public TextMeshProUGUI screenModeText;

    private List<FullScreenMode> screenModes;
    private int currentModeIndex;
    
    public GameObject SettingWindow;

    void Start()
    {
        screenModes = new List<FullScreenMode> { FullScreenMode.Windowed, FullScreenMode.FullScreenWindow };
        currentModeIndex = PlayerPrefs.GetInt("ScreenModeIndex", 0);
        UpdateScreenMode();

        //float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        //soundSlider.value = savedVolume;
        //SetMasterVolume(savedVolume); 

        //soundSlider.onValueChanged.AddListener(SetMasterVolume);

        prevButton.onClick.AddListener(() => OnClickScreenMode(-1));
        nextButton.onClick.AddListener(() => OnClickScreenMode(1));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingWindow.activeSelf) CloseSetting();
            else OpenSetting();
        }
    }

   /* public void SetMasterVolume(float value)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }
    */
    
    public void OnClickScreenMode(int direction)
    {
        currentModeIndex += direction;

        if (currentModeIndex < 0)
        {
            currentModeIndex = screenModes.Count - 1;
        }
        else if (currentModeIndex >= screenModes.Count)
        {
            currentModeIndex = 0;
        }
        
        UpdateScreenMode();
    }

    private void UpdateScreenMode()
    {
        FullScreenMode selectedMode = screenModes[currentModeIndex];
        Screen.fullScreenMode = selectedMode;

        switch (selectedMode)
        {
            case FullScreenMode.Windowed:
                screenModeText.text = "Window";
                break;
            case FullScreenMode.FullScreenWindow:
                screenModeText.text = "Full Screen";
                break;
        }

        PlayerPrefs.SetInt("ScreenModeIndex", currentModeIndex);
        PlayerPrefs.Save();
    }    
    public void OpenSetting()
    {
        SettingWindow.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseSetting()
    {
        SettingWindow.SetActive(false);
        Time.timeScale = 1f;

    }

    public void ClearSetting()
    {
        
    }

}
