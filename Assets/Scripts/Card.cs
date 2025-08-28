using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public CardContent cardContent;
    public HandCardSlot slot;

    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Renderer[] subObjectsRenderers;

    public void OnPointerClick(PointerEventData eventData)
    {
        // 좌클릭
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CardManager.Inst.CardLeftClicked(this);
        }
        // 우클릭
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            CardManager.Inst.CardRightClicked();
        }
    }

    public void Setup(CardContent content)
    {
        cardContent = content;

        costText.text = content.cost.ToString();
        nameText.text = content.name;
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
