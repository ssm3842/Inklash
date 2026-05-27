using System;
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
            RunManager.Inst.resourceManager?.EarnGold(UnityEngine.Random.Range(35, 50));
            //짤랑 사운드 설정

            //카드 보상 캔버스 설정


            List<CardDataSO> cardRewardList = new List<CardDataSO>();
            List<CardDataSO> allCardRewardPool = new List<CardDataSO>(RunManager.Inst.unitDataManager.GetCardRewardPool());
            for(int i=0; i<3; i++)
            {
                int randomI = UnityEngine.Random.Range(0, allCardRewardPool.Count);

                CardDataSO originalCardData = allCardRewardPool[randomI];
                CardDataSO instantiatedCardData = Instantiate(originalCardData);

                cardRewardList.Add(instantiatedCardData);
                allCardRewardPool.Remove(originalCardData); //선택지에 같은 카드가 나오는 것 방지.
            }

            //5층 이상부터는 강화 카드가 보상으로 나올 수 있음.
            if(RunManager.Inst.mapManager.floorClimbed >= 5)
            {
                foreach(CardDataSO card in cardRewardList)
                {
                    //단어카드는 강화 없음
                    if(card.card.cardType == CardType.Word) continue;

                    //강화 카드 등장 확률 기본 10% + 클리어한 층마다 5% 추가.
                    if(UnityEngine.Random.Range(0, 100) < (10 + RunManager.Inst.mapManager.floorClimbed * 5))
                    {
                        //강화가 여러번 될 확률.
                        int enchantedCount = 1;
                        do
                        {
                            int enchantType = UnityEngine.Random.Range(0, 4);

                            //마법카드는 체력 강화가 나올 수 없음.
                            while(card.card.cardType == CardType.Spell && enchantType == 1)
                            {
                                enchantType = UnityEngine.Random.Range(0, 4);
                            }

                            //강화 실행
                            switch(enchantType)
                            {
                                //공격력 강화
                                case 0:
                                    card.card.stats.baseATK += 5;
                                    break;
                                //체력 강화
                                case 1:
                                    card.card.stats.baseMaxHp += 10;
                                    break;
                                //코스트 강화
                                case 2:
                                    card.card.cost = Mathf.Max(0, card.card.cost - 1);
                                    break;
                                //랜덤 인장 부여
                                case 3:
                                    //이미 인장이 3개면 다시 강화 시도
                                    if(card.card.seals.Count >= 3) continue;

                                    Array sealValues = Enum.GetValues(typeof(SealType));
                                    int randomIndex = UnityEngine.Random.Range(1, sealValues.Length);

                                    //인장 중복을 방지
                                    while(card.card.seals.Contains((SealType)sealValues.GetValue(randomIndex)))
                                    {
                                        randomIndex = UnityEngine.Random.Range(1, sealValues.Length);
                                    }

                                    card.card.seals.Add((SealType)sealValues.GetValue(randomIndex));
                                    break;
                                default:
                                    break;
                            }
                            enchantedCount++;
                            
                        } while (UnityEngine.Random.Range(0, 100) < 100 * Mathf.Pow(0.5f, enchantedCount + 1));
                    }
                }
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
