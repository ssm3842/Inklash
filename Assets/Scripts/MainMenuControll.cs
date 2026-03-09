using UnityEngine;

public class MainMenuControll : MonoBehaviour
{

    public GameObject loadGameButton;
    [SerializeField] GameObject deckSelectCanvas;

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
    void Start()
    {
        gameObject.SetActive(true);
        deckSelectCanvas.SetActive(false);
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
