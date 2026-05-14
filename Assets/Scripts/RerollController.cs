using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RerollController : InteractableUI
{
    [SerializeField]Image black;
    float remainTime;
    public void _OnReroll()
    {
        if(remainTime <= 0f)
        {
            StartCoroutine(RunManager.Inst.battleManager.cardManager.DrawNewHand());
            remainTime = GameRule.REROLL_WAIT_SECOND;
        }
    }

    void Update()
    {
        remainTime -= Time.deltaTime;
        black.fillAmount = (remainTime <= 0 ? 0f : remainTime) / GameRule.REROLL_WAIT_SECOND;
    }

    //마우스 호버 시
    override public void OnPointerEnter(PointerEventData eventData)
    {
        if(remainTime <= 0f) transform.localScale = new Vector3(1.3f, 1.3f, 1f);
    }
}
