using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomEventCanvas : MonoBehaviour
{
    [SerializeField] RandomEventSO[] randomEventData;
    RandomEventSO currentRandomEvent;

    [SerializeField] Image eventImage;
    [SerializeField] TextMeshProUGUI eventDecription;

    public void Init()
    {
        currentRandomEvent = randomEventData[Random.Range(0, randomEventData.Length)];
        
        eventImage.sprite = currentRandomEvent.eventImage;
        eventDecription.text = currentRandomEvent.eventDescription;

        gameObject.SetActive(true);
    }

    public void _OnRandomEventButtonClicked()
    {
        //클리어 판정.
        RunManager.Inst.mapManager.ClearLastRoom();
        
        //다시 맵 표시.
        gameObject.SetActive(false);
        RunManager.Inst.mapManager.SetVisible();
    }
}
