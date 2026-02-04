using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckViewCard : MonoBehaviour
{
    public CardContent cardContent;

    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TextMeshProUGUI ATKText;
    [SerializeField] TextMeshProUGUI HPText;
    [SerializeField] Renderer[] subObjectsRenderers;

    [SerializeField] Sprite[] cardBackground;


    public void Setup(CardContent content)
    {
        cardContent = content;

        switch(cardContent.cardType)
        {
            case CardType.Unit:
                GetComponent<Image>().sprite = cardBackground[0];
                break;
            case CardType.Spell:
                GetComponent<Image>().sprite = cardBackground[1];
                break;
            case CardType.Word:
                GetComponent<Image>().sprite = cardBackground[2];
                break;
        }

        costText.text = content.cost.ToString();
        nameText.text = content.name;

        ATKText.text = content.stats.baseATK.ToString();
        HPText.text = content.stats.baseMaxHp.ToString();
    }
}