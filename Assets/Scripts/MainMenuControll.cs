using UnityEngine;
using UnityEngine.SceneManagement;

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
            SettingManger.Inst.LoadSetting();
            SettingManger.Inst.gameObject.SetActive(false);
        }

        if(BGMManager.Inst == null) Instantiate(bgmManager);
        else BGMManager.Inst.PlayBGM(mainMenuBGM, 0.1f);
    }

    public void _OnClickNewGame()
    {
        // gameObject.SetActive(false);
        // deckSelectCanvas.SetActive(true);

        SceneManager.LoadScene(1);
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
