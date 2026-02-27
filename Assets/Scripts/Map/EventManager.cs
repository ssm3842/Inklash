using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField]AddCardEvent addCardObjects;
    [SerializeField]CampfireEvent campfireObjects;
    [SerializeField]MixCardEvent mixCardObjects;
    [SerializeField]MakeSealEvent makeSealObjects;

    [SerializeField]BattleRewardController battleRewardCanvas;


    public void SetEventCanvas(RoomContent room)
    {
        switch(room.eventRoomType)
        {
            case EventRoomType.ADDCARD:
                addCardObjects.gameObject.SetActive(true);
                campfireObjects.gameObject.SetActive(false);
                mixCardObjects.gameObject.SetActive(false);
                makeSealObjects.gameObject.SetActive(false);

                addCardObjects.SetEvent();
                break;
            case EventRoomType.CAMPFIRE:
                //0이면 체력, 1이면 공격력을 올리는 방이 됨.
                StatType targetStat = UnityEngine.Random.Range(0, 2) == 0? StatType.MAX_HP : StatType.ATK;

                addCardObjects.gameObject.SetActive(false);
                campfireObjects.gameObject.SetActive(true);
                mixCardObjects.gameObject.SetActive(false);
                makeSealObjects.gameObject.SetActive(false);

                campfireObjects.FilterDeckCard(targetStat, CardType.Unit);
                break;
            case EventRoomType.MIXCARD:
                addCardObjects.gameObject.SetActive(false);
                campfireObjects.gameObject.SetActive(false);
                mixCardObjects.gameObject.SetActive(true);
                makeSealObjects.gameObject.SetActive(false);

                mixCardObjects.FilterDeckCard();
                break;
            case EventRoomType.MAKESEAL:
                addCardObjects.gameObject.SetActive(false);
                campfireObjects.gameObject.SetActive(false);
                mixCardObjects.gameObject.SetActive(false);
                makeSealObjects.gameObject.SetActive(true);

                makeSealObjects.SetEvent();
                break;
        }
        gameObject.SetActive(true);
        RunManager.Inst.mapManager.ClearLastRoom();
    }

    public void _OnEventEnd()
    {
        RunManager.Inst.mapManager.SetVisible();
        gameObject.SetActive(false);
    }

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
