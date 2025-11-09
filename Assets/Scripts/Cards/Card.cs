using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardContent cardContent;
    public int originalIndex;
    Vector3 originalPosition;
    Quaternion originalRotation;

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
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetSiblingIndex(5);

        originalPosition = transform.position;
        transform.position = new Vector3(transform.position.x, 450, 0);

        originalRotation = transform.rotation;
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(1.5f, 1.5f, 1f);

        // Time.timeScale = 0.1f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.SetSiblingIndex(originalIndex);

        transform.position = originalPosition;

        transform.rotation = originalRotation;
        transform.localScale = new Vector3(1f, 1f, 1f);

        // Time.timeScale = 1f;
    }

    public void Setup(CardManager newCardManager, CardContent content, int newIndex)
    {
        cardManager = newCardManager;
        cardContent = content;
        originalIndex = newIndex;

        costText.text = content.cost.ToString();
        nameText.text = content.name;
        descriptionText.text = content.description;
    }

    public int GetOriginalIndex()
    {
        return originalIndex;    
    }
}