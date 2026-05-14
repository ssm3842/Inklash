using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //마우스 호버 시
    virtual public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.3f, 1.3f, 1f);
    }
    //마우스 호버 종료 시
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
