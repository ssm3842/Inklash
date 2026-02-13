using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckViewCard : MonoBehaviour
{
    public CardContent cardContent;

    [SerializeField] Image ATKImageUI;
    [SerializeField] Image HPImageUI;
    [SerializeField] Image TimeImageUI;

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

        switch(cardContent.firstInfo)
        {
            case CardUIInfo.None:
                ATKImageUI.gameObject.SetActive(false);
                TimeImageUI.gameObject.SetActive(false);
                ATKText.gameObject.SetActive(false);
                break;
            case CardUIInfo.ATK:
                ATKImageUI.gameObject.SetActive(true);
                TimeImageUI.gameObject.SetActive(false);
                ATKText.gameObject.SetActive(true);
                break;
            case CardUIInfo.Time:
                ATKImageUI.gameObject.SetActive(false);
                TimeImageUI.gameObject.SetActive(true);
                ATKText.gameObject.SetActive(true);
                break;
        }

        switch(cardContent.secondInfo)
        {
            case CardUIInfo.None:
                HPImageUI.gameObject.SetActive(false);
                HPText.gameObject.SetActive(false);
                break;
            case CardUIInfo.HP:
                HPImageUI.gameObject.SetActive(true);
                HPText.gameObject.SetActive(true);
                break;
        }

        costText.text = content.cost.ToString();
        nameText.text = content.name;

        ATKText.text = content.stats.baseATK.ToString();
        HPText.text = content.stats.baseMaxHp.ToString();
    }
}