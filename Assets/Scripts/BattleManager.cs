using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    [SerializeField] GameObject battleUICanvas;
    [SerializeField] BattleRewardController battleRewardCanvas;

    [SerializeField] RunEndCanvas runEndCanvas;

    bool isBattleProgress = false;

    public CardManager cardManager;
    public CardUseManager cardUseManager;

    public void InitBattle(bool isBoss)
    {
        battleRewardCanvas.gameObject.SetActive(false);

        cardManager.cardUseManager = cardUseManager;

        gameObject.SetActive(true);

        //덱, 코스트 초기화 및 전투 시작.
        cardManager.Init();
        battleUICanvas.SetActive(true);

        //데미지 텍스트 삭제
        DamageTextCanvas.Inst.Init();

        Time.timeScale = 1f;

        //이전 전투에서 소환된 유닛을 제거.
        foreach(Transform unit in cardUseManager.transform)
        {
            Destroy(unit.gameObject);
        }
        
        //각 기지를 초기화하고 적 유닛풀 설정.
        cardUseManager.InitUnitManager(isBoss);
        isBattleProgress = true;
    }

    public void OnBattleWin()
    {
        isBattleProgress = false;

        EnemyBaseDataSO currentEnemy = RunManager.Inst.battleManager.cardUseManager.CurrentEnemyData;
        if(currentEnemy.isBoss)
        {
            runEndCanvas.SetCanvas("승리했습니다!");
        }
        else
        {
            //골드 획득
            RunManager.Inst.resourceManager?.EarnGold(Random.Range(50, 70));
            //짤랑 사운드 설정

            //카드 보상 캔버스 설정


            List<CardDataSO> cardRewardList = new List<CardDataSO>();
            List<CardDataSO> allCardRewardPool = RunManager.Inst.unitDataManager.GetCardRewardPool();
            for(int i=0; i<3; i++)
            {
                int randomI = Random.Range(0, allCardRewardPool.Count);

                CardDataSO currentCardData = allCardRewardPool[randomI];
                cardRewardList.Add(currentCardData);
            }
            RunManager.Inst.cardRewardCanvas.GetComponent<CardRewardUI>().ShowCardReward(cardRewardList);


            //보상을 설정
            // battleRewardCanvas.AddRewards(goldButtons: 1, cardButtons: 1);


            cardManager.CardRightClicked();

            RunManager.Inst.mapManager.ClearLastRoom();
        }
    }

    public void OnBattleLose()
    {
        isBattleProgress = false;

        EnemyBaseDataSO currentEnemy = RunManager.Inst.battleManager.cardUseManager.CurrentEnemyData;

        int lifePenalty = currentEnemy.isBoss ? 999 : 1 ;
        bool isGameOver = RunManager.Inst.resourceManager.DecreaseLife(lifePenalty);
        if (isGameOver)
        {
            cardManager.CardRightClicked();
            
            RunManager.Inst.mapManager.ClearLastRoom();
            RunManager.Inst.mapManager.SetVisible();
        }
        else
        {
            runEndCanvas.SetCanvas("패배했습니다");
        }
    }

    public bool GetBattleProgress()
    {
        return isBattleProgress;
    }
}
