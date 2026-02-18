using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // [SerializeField]GameObject addCardObjects;
    [SerializeField]CampfireEvent campfireObjects;
    [SerializeField]MixCardEvent mixCardObjects;
    [SerializeField]MakeFlagEvent makeFlagObjects;

    [SerializeField]BattleRewardController battleRewardCanvas;


    public void SetEventCanvas(RoomContent room)
    {
        switch(room.eventRoomType)
        {
            case EventRoomType.ADDCARD:
                // List<CardDataSO> cardRewardList = new List<CardDataSO>();
                // List<CardDataSO> allCardRewardPool = RunManager.Inst.unitDataManager.GetCardRewardPool();
                // for(int i=0; i<3; i++)
                // {
                //     int randomI = Random.Range(0, allCardRewardPool.Count);

                //     CardDataSO currentCardData = allCardRewardPool[randomI];
                //     cardRewardList.Add(currentCardData);
                // }
                // RunManager.Inst.cardRewardCanvas.GetComponent<CardRewardUI>().ShowCardReward(cardRewardList);
                battleRewardCanvas.AddRewards(goldButtons: 0, cardButtons: 2);
                break;
            case EventRoomType.CAMPFIRE:
                //0이면 체력, 1이면 공격력을 올리는 방이 됨.
                StatType targetStat = UnityEngine.Random.Range(0, 2) == 0? StatType.MAX_HP : StatType.ATK;

                campfireObjects.gameObject.SetActive(true);
                mixCardObjects.gameObject.SetActive(false);
                makeFlagObjects.gameObject.SetActive(false);

                campfireObjects.FilterDeckCard(targetStat, CardType.Unit);
                break;
            case EventRoomType.MIXCARD:
                campfireObjects.gameObject.SetActive(false);
                mixCardObjects.gameObject.SetActive(true);
                makeFlagObjects.gameObject.SetActive(false);

                mixCardObjects.FilterDeckCard();
                break;
            case EventRoomType.MOVEFLAG:
                campfireObjects.gameObject.SetActive(false);
                mixCardObjects.gameObject.SetActive(false);
                makeFlagObjects.gameObject.SetActive(true);

                makeFlagObjects.SetEvent();
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
