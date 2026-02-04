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

    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text ATKText;
    [SerializeField] TMP_Text HPText;

    [SerializeField] Renderer[] subObjectsRenderers;

    [SerializeField] Sprite[] cardBackground;

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

    public int GetOriginalIndex()
    {
        return originalIndex;    
    }
}