using TMPro;
using UnityEngine;

public class RunEndCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;
    public void SetCanvas(string text)
    {
        gameObject.SetActive(true);
        resultText.text = text;
    }
}
