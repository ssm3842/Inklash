using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeSceneToRun()
    {
        PlayerPrefs.SetInt("HasSaveData", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(1);
    }
}
