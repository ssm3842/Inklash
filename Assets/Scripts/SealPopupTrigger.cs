using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SealPopupTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]Image imageComponent;
    [SerializeField]SealPopup sealPopup;
    SealDataSO sealData;

    public void SetSealData(SealDataSO sealDataSO)
    {
        sealData = sealDataSO;
        imageComponent.sprite = sealData.sealIcon;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        sealPopup.transform.position = transform.position;
        sealPopup.SetupPopup(sealData);
        sealPopup.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        sealPopup.gameObject.SetActive(false);
    }
}
