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
                RunManager.Inst.mapManager.SetMapText("카 드   획 득");
                addCardObjects.gameObject.SetActive(true);
                UpgradeObjects.gameObject.SetActive(false);
                mixCardObjects.gameObject.SetActive(false);
                makeSealObjects.gameObject.SetActive(false);

                addCardObjects.SetEvent();
                break;
            case EventRoomType.UPGRADE:
                RunManager.Inst.mapManager.SetMapText("카 드   강 화");
                addCardObjects.gameObject.SetActive(false);
                UpgradeObjects.gameObject.SetActive(true);
                mixCardObjects.gameObject.SetActive(false);
                makeSealObjects.gameObject.SetActive(false);

                UpgradeObjects.SetEvent();
                break;
            case EventRoomType.MIXCARD:
                RunManager.Inst.mapManager.SetMapText("카 드   융 합");
                addCardObjects.gameObject.SetActive(false);
                UpgradeObjects.gameObject.SetActive(false);
                mixCardObjects.gameObject.SetActive(true);
                makeSealObjects.gameObject.SetActive(false);

                mixCardObjects.EnterEvent();
                break;
            case EventRoomType.MAKESEAL:
                RunManager.Inst.mapManager.SetMapText("인 장   부 여");
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
