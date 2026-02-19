using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfo : MonoBehaviour
{
    [SerializeField] Image cardInfoImage;
    [SerializeField] TextMeshProUGUI cardInfoName;

    public void Setup(CardContent cardContent)
    {
        cardInfoImage.sprite = cardContent.cardImage;
        cardInfoName.text = cardContent.name;
    }
}
