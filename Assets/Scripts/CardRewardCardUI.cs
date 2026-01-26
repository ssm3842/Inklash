using UnityEngine;
using TMPro;

public class CardRewardCardUI : MonoBehaviour
{
    public CardContent cardContent;

    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;

    public void Setup(CardContent content)
    {
        cardContent = content;

        costText.text = content.cost.ToString();
        nameText.text = content.name;
        descriptionText.text = content.description;
    }
}
