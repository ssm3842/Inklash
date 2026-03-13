using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardRewardCardUI : MonoBehaviour
{
    public CardContent cardContent;

    [SerializeField] Image cardImage;

    [SerializeField] Image ATKImageUI;
    [SerializeField] Image HPImageUI;
    [SerializeField] Image TimeImageUI;

    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TextMeshProUGUI ATKText;
    [SerializeField] TextMeshProUGUI HPText;

    [SerializeField] Image[] sealImageComponents;
    [SerializeField] CanvasGroup[] sealBackgroundImageComponents;

    public void Setup(CardContent content)
    {
        cardContent = content;

        if(cardContent.cardImage == null) cardImage.color = new Color(0,0,0,0);
        else cardImage.sprite = cardContent.cardImage;

        GetComponent<Image>().sprite = SpriteDataContainer.Inst.GetCardBackgroundSprite(cardContent.cardType);

        //인장 이미지 일단 비활성화 후 있으면 활성화
        for(int i=0; i<sealImageComponents.Length; i++)
        {
            sealBackgroundImageComponents[i].alpha = 0f;
        }
        
        int index = 0;
        foreach (SealType type in System.Enum.GetValues(typeof(SealType)))
        {
            if (type != SealType.None && (cardContent.seals & type) == type)
            {
                sealBackgroundImageComponents[index].alpha = 1f;
                sealImageComponents[index].sprite = SpriteDataContainer.Inst.GetSealSprite(type);
                index++;
            }
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
    public void SetTransparent(bool boolean)
    {   
        GetComponent<CanvasGroup>().alpha = boolean ? 0f : 1f;
        GetComponent<CanvasGroup>().interactable = !boolean;
        GetComponent<CanvasGroup>().blocksRaycasts = !boolean;
    }
}
