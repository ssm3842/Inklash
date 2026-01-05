using UnityEngine;

public class CampfireCanvas : MonoBehaviour
{
    public void _OnCampfireButtonClicked()
    {
        //휴식 공간 효과 발동.
        RunManager.Inst.resourceManager.HealLife(10);

        //클리어 판정.
        RunManager.Inst.mapManager.ClearLastRoom();
        
        //다시 맵 표시.
        gameObject.SetActive(false);
        RunManager.Inst.mapManager.SetVisible();
    }
}
