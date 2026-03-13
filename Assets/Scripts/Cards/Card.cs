using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public CardContent cardContent;
    public int originalIndex;
    Vector3 originalPosition;
    Quaternion originalRotation;

    [SerializeField] CardManager cardManager;

    [SerializeField] Image cardImage;

    [SerializeField] Image ATKImageUI;
    [SerializeField] Image HPImageUI;
    [SerializeField] Image TimeImageUI;

    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text ATKText;
    [SerializeField] TMP_Text HPText;

    [SerializeField] Image[] sealImageComponents;
    [SerializeField] CanvasGroup[] sealBackgroundImageComponents;

    [SerializeField] Renderer[] subObjectsRenderers;

    //좌클릭과 우클릭 시 
    public void OnPointerClick(PointerEventData eventData)
    {
        // 좌클릭
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(!eventData.dragging)
            {
                cardManager.CardLeftClicked(this);
            }
        }
        // 우클릭
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            cardManager.CardRightClicked();
        }
    }
    //마우스 호버 시
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();

        originalPosition = transform.position;
        transform.position = new Vector3(transform.position.x, 225, 0);

        originalRotation = transform.rotation;
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(1.5f, 1.5f, 1f);

        // Time.timeScale = 0.1f;
    }
    //마우스 호버 종료 시
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.SetSiblingIndex(originalIndex);

        transform.position = originalPosition;

        transform.rotation = originalRotation;
        transform.localScale = new Vector3(1f, 1f, 1f);

        // Time.timeScale = 1f;
    }

    //마우스 드래그 시작 시
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            cardManager.StartDraggingCard(this, isSticky: false);
        }
    }

    //마우스 드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            cardManager.ProcessDraggingCard(eventData);
        }
    }

    //마우스 드래그 종료
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            cardManager.EndDraggingCard(this);
        }
    }

    public void Setup(CardManager newCardManager, CardContent content, int newIndex)
    {
        cardManager = newCardManager;
        cardContent = content;
        originalIndex = newIndex;

        if(cardContent.cardImage == null) cardImage.color = new Color(0,0,0,0);
        else cardImage.sprite = cardContent.cardImage;

        GetComponent<Image>().sprite = SpriteDataContainer.Inst.GetCardBackgroundSprite(content.cardType);

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

        costText.text = cardContent.cost.ToString();
        nameText.text = cardContent.name;

        ATKText.text = cardContent.stats.baseATK.ToString();
        HPText.text = cardContent.stats.baseMaxHp.ToString();
    }

    public int GetOriginalIndex()
    {
        return originalIndex;    
    }
}