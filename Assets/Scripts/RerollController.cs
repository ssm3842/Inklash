using UnityEngine;
using UnityEngine.UI;

public class RerollController : MonoBehaviour
{
    [SerializeField]Image black;
    float remainTime;
    public void _OnReroll()
    {
        if(remainTime <= 0f)
        {
            RunManager.Inst.battleManager.cardManager.DrawNewHand();
            remainTime = GameRule.REROLL_WAIT_SECOND;
        }
    }

    void Update()
    {
        remainTime -= Time.deltaTime;
        black.fillAmount = (remainTime <= 0 ? 0f : remainTime) / GameRule.REROLL_WAIT_SECOND;
    }
}
