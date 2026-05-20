using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using TMPro;

public class SettingManger : MonoBehaviour
{
    public static SettingManger Inst { get; private set; }
    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;

        DontDestroyOnLoad(gameObject);
    }
    
    [Header("메인 창")]
    public GameObject SettingWindow; 
    
    [Header("탭 관리")]
    public GameObject panelGraphics;
    public GameObject panelSound;    
    // public GameObject panelControls; 
    // public GameObject panelEtc;    
    
    [Header("사운드 설정 (Sound Panel)")]
    public AudioMixer mainMixer; 
    public Slider masterSlider; 
    public Slider bgmSlider;
    public Slider sfxSlider;         

    [Header("그래픽 설정 (Graphic Panel)")]
    public Button prevButton;      
    public Button nextButton;     
    public TextMeshProUGUI screenModeText; 

    private List<FullScreenMode> screenModes;
    private int currentModeIndex;

    void Start()
    {
        ShowPanel("Graphic");

        // Application.targetFrameRate = 60;

        prevButton.onClick.AddListener(() => OnClickScreenMode(-1));
        nextButton.onClick.AddListener(() => OnClickScreenMode(1));
        
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetEffectVolume);

        LoadSetting();
    }

    public void LoadSetting()
    {
        screenModes = new List<FullScreenMode> { FullScreenMode.Windowed, FullScreenMode.FullScreenWindow };
        currentModeIndex = PlayerPrefs.GetInt("ScreenModeIndex", 0);
        UpdateScreenMode();
        
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 1f);
        masterSlider.value = masterVol;
        SetMasterVolume(masterVol); 

        float bgmVol = PlayerPrefs.GetFloat("BGMVolume", 1f);
        bgmSlider.value = bgmVol;
        SetBGMVolume(bgmVol);

        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sfxSlider.value = sfxVol;
        SetEffectVolume(sfxVol);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingWindow.activeSelf) CloseSetting();
            else OpenSetting();
        }
    }

    public void ShowPanel(string panelName)
    {
        panelGraphics.SetActive(false);
        panelSound.SetActive(false);
        // panelControls.SetActive(false);
        // panelEtc.SetActive(false);

        if (panelName == "Graphic")
            panelGraphics.SetActive(true);
        else if (panelName == "Sound")
            panelSound.SetActive(true);
        // else if (panelName == "Control")
        //     panelControls.SetActive(true);
        // else if (panelName == "Etc")
        //     panelEtc.SetActive(true);
    }

   // 사운드
    public void SetMasterVolume(float value)
    {
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
    
    public void SetBGMVolume(float value)
    {
        mainMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }
    
    public void SetEffectVolume(float value)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    // 그래픽
    public void OnClickScreenMode(int direction)
    {
        currentModeIndex += direction;

        if (currentModeIndex < 0) currentModeIndex = screenModes.Count - 1;
        else if (currentModeIndex >= screenModes.Count) currentModeIndex = 0;
        
        UpdateScreenMode();
    }

    private void UpdateScreenMode()
    {
        FullScreenMode selectedMode = screenModes[currentModeIndex];
        Screen.fullScreenMode = selectedMode;

        screenModeText.text = selectedMode == FullScreenMode.Windowed ? "Window" : "Full Screen";

        PlayerPrefs.SetInt("ScreenModeIndex", currentModeIndex);
    }

  
    public void OpenSetting()
    {
        SettingWindow.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseSetting()
    {
        SettingWindow.SetActive(false);
        if(RunManager.Inst != null) { if(RunManager.Inst.battleManager.GetBattleProgress()) {Time.timeScale = 1f; }}
        PlayerPrefs.Save();  
    }

    public void ClearSetting()
    {
        PlayerPrefs.DeleteKey("MasterVolume");
        PlayerPrefs.DeleteKey("BGMVolume");
        PlayerPrefs.DeleteKey("SFXVolume");
        PlayerPrefs.DeleteKey("ScreenModeIndex");

        masterSlider.value = 1f;
        bgmSlider.value = 1f;
        sfxSlider.value = 1f;
        currentModeIndex = 0;

        SetMasterVolume(1f);
        SetBGMVolume(1f);
        SetEffectVolume(1f);
        UpdateScreenMode();
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}