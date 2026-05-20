using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CardRewardCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent CardHoverEnter;
    public UnityEvent CardHoverExit;

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
        
        //카드에 부착된 인장 수 만큼 반복.
        for(int i=0; i<cardContent.seals.Count; i++)
        {
            sealBackgroundImageComponents[i].alpha = 1f;
            sealImageComponents[i].sprite = SpriteDataContainer.Inst.GetSealData(cardContent.seals[i]).sealIcon;
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

    public void SetCardDark(bool isTrue)
    {
        if(isTrue)
        {
            GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f);

            cardImage.color = new Color(0.25f, 0.25f, 0.25f);
            ATKImageUI.color = new Color(0.25f, 0.25f, 0.25f);
            HPImageUI.color = new Color(0.25f, 0.25f, 0.25f);
            TimeImageUI.color = new Color(0.25f, 0.25f, 0.25f);

            costText.color = new Color(0.25f, 0.25f, 0.25f);

            for(int i=0; i<sealImageComponents.Length; i++)
            {
                sealBackgroundImageComponents[i].gameObject.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f);
            }
        }
        else
        {
            GetComponent<Image>().color = Color.white;

            cardImage.color = Color.white;
            ATKImageUI.color = Color.white;
            HPImageUI.color = Color.white;
            TimeImageUI.color = Color.white;

            costText.color = Color.white;

            for(int i=0; i<sealImageComponents.Length; i++)
            {
                sealBackgroundImageComponents[i].gameObject.GetComponent<Image>().color = Color.white;
            }
        }
    }
    public void SetTransparent(bool boolean)
    {   
        GetComponent<CanvasGroup>().alpha = boolean ? 0f : 1f;
        GetComponent<CanvasGroup>().interactable = !boolean;
        GetComponent<CanvasGroup>().blocksRaycasts = !boolean;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CardHoverEnter.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CardHoverExit.Invoke();
    }
}
