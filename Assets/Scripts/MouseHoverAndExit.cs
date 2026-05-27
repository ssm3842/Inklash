using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MouseHoverAndExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent CardHoverEnter;
    public UnityEvent CardHoverExit;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        CardHoverEnter.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CardHoverExit.Invoke();
    }
}
