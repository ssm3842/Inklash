using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]GameObject tooltipObject;
    [SerializeField]TextMeshProUGUI tooltipTMPro;
    // 툴팁에 표시할 텍스트박스
    [TextArea(2, 10)]
    [SerializeField]string tooltipContent;
    [SerializeField]float hoverDelay = 0.5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Invoke("ShowTooltip", hoverDelay);
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke("ShowTooltip");
        tooltipObject.SetActive(false);
    }

    void ShowTooltip()
    {
        tooltipObject.SetActive(true);
        tooltipTMPro.text = tooltipContent;
    }
}
