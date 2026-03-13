using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField]AddCardEvent addCardObjects;
    [SerializeField]UpgradeEvent UpgradeObjects;
    [SerializeField]MixCardEvent mixCardObjects;
    [SerializeField]MakeSealEvent makeSealObjects;

    [SerializeField]BattleRewardController battleRewardCanvas;


    public void SetEventCanvas(RoomContent room)
    {
        switch(room.eventRoomType)
        {
            case EventRoomType.ADDCARD:
                addCardObjects.gameObject.SetActive(true);
                UpgradeObjects.gameObject.SetActive(false);
                mixCardObjects.gameObject.SetActive(false);
                makeSealObjects.gameObject.SetActive(false);

                addCardObjects.SetEvent();
                break;
            case EventRoomType.Upgrade:
                addCardObjects.gameObject.SetActive(false);
                UpgradeObjects.gameObject.SetActive(true);
                mixCardObjects.gameObject.SetActive(false);
                makeSealObjects.gameObject.SetActive(false);

                UpgradeObjects.FilterDeckCard();
                break;
            case EventRoomType.MIXCARD:
                addCardObjects.gameObject.SetActive(false);
                UpgradeObjects.gameObject.SetActive(false);
                mixCardObjects.gameObject.SetActive(true);
                makeSealObjects.gameObject.SetActive(false);

                mixCardObjects.FilterDeckCard();
                break;
            case EventRoomType.MAKESEAL:
                addCardObjects.gameObject.SetActive(false);
                UpgradeObjects.gameObject.SetActive(false);
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
}
