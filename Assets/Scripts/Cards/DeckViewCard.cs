using UnityEngine;
using TMPro;

public class DeckViewCard : MonoBehaviour
{
    public CardContent cardContent;

    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Renderer[] subObjectsRenderers;


    public void Setup(CardContent content)
    {
        cardContent = content;

        costText.text = content.cost.ToString();
        nameText.text = content.name;
        descriptionText.text = content.description;
    }
}