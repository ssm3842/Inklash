using UnityEngine;

public class MainMenuControll : MonoBehaviour
{
    [SerializeField]GameObject deckSelectCanvas;
    [SerializeField]GameObject settingCavnas;
    [SerializeField]GameObject bgmManager;

    [SerializeField]AudioClip mainMenuBGM;

    void Start()
    {
        gameObject.SetActive(true);
        deckSelectCanvas.SetActive(false);

        if(SettingManger.Inst == null)
        {
            Instantiate(settingCavnas);
        }
        else
        {
            settingCavnas.GetComponent<SettingManger>().LoadSetting();
            settingCavnas.SetActive(false);
        }

        if(BGMManager.Inst == null) Instantiate(bgmManager);
        else BGMManager.Inst.PlayBGM(mainMenuBGM);
    }

    public void _OnClickNewGame()
    {
        gameObject.SetActive(false);
        deckSelectCanvas.SetActive(true);
    }
    public void _OnClickReturnToNewGame()
    {
        gameObject.SetActive(true);
        deckSelectCanvas.SetActive(false);
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
