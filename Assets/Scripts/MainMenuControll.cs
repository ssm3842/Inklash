using UnityEngine;
using UnityEngine.UI;

public class MainMenuControll : MonoBehaviour
{

    public GameObject loadGameButton;

  void Awake()
    {
        bool hasSaveData = (PlayerPrefs.GetInt("HasSaveData", 0) == 1);

        if (hasSaveData)
        {
            LoadGameState(true);
        }
        else
        {
            LoadGameState(false);
        }
    }

    public void OnClickNewGame()
    {
        PlayerPrefs.SetInt("HasSaveData", 1);
        PlayerPrefs.Save();
    }

    public void LoadGameState(bool state)
    {
        loadGameButton.SetActive(state);
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
