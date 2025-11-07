using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public CardContent cardContent;
    public int originIndex;

    [SerializeField] CardManager cardManager;

    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Renderer[] subObjectsRenderers;

    public void OnPointerClick(PointerEventData eventData)
    {
        // 좌클릭
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            cardManager.CardLeftClicked(this);
        }
        // 우클릭
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            cardManager.CardRightClicked();
        }
    }

    public void Setup(CardManager newCardManager, CardContent content, int newIndex)
    {
        cardManager = newCardManager;
        cardContent = content;
        originIndex = newIndex;

        costText.text = content.cost.ToString();
        nameText.text = content.name;
        descriptionText.text = content.description;
    }

    public void SetOrderInLayer(int order)
    {
        GetComponent<SpriteRenderer>().sortingOrder = order * 10;
        foreach (var renderer in subObjectsRenderers)
        {
            renderer.sortingOrder = order * 10 + 1;
        }
    }

}