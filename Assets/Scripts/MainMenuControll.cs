using UnityEngine;

public class MainMenuControll : MonoBehaviour
{
    [SerializeField] GameObject deckSelectCanvas;

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
    
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}
