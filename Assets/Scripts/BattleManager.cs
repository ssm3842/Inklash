using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{

    [SerializeField] GameObject battleUICanvas;
    [SerializeField] BattleRewardController battleRewardCanvas;

    [SerializeField] GameObject defeatCanvas;

    public CardManager cardManager;
    public CardUseManager cardUseManager;

    public UnityEvent CardUse;

    public void InitBattle()
    {
        battleRewardCanvas.gameObject.SetActive(false);

        cardManager.cardUseManager = cardUseManager;

        //덱, 코스트 초기화 및 전투 시작.
        cardManager.Init();
        battleUICanvas.SetActive(true);

        Time.timeScale = 1f;
        gameObject.SetActive(true);

        //이전 전투에서 소환된 유닛을 제거.
        foreach(Transform unit in cardUseManager.transform)
        {
            Destroy(unit.gameObject);
        }
        
        //각 기지를 초기화하고 적 유닛풀 설정.
        cardUseManager.InitUnitManager();
    }

    public void OnBattleWin()
    {
        Time.timeScale = 0f;

        //보상을 설정
        battleRewardCanvas.AddRewards();

        battleRewardCanvas.gameObject.SetActive(true);

        cardManager.CardRightClicked();

        RunManager.Inst.mapManager.ClearLastRoom();
    }

    public void OnBattleLose()
    {
        Time.timeScale = 0f;

        EnemyBaseDataSO currentEnemy = RunManager.Inst.battleManager.cardUseManager.CurrentEnemyData;

        //int lifePenalty = currentEnemy.isElite ? 999 : 1 ; //TODO: Elite -> Boss
        int lifePenalty = 1;
        bool isGameOver = RunManager.Inst.resourceManager.DecreaseLife(lifePenalty);
        if (isGameOver)
        {
            cardManager.CardRightClicked();
            
            RunManager.Inst.mapManager.FailLastRoom();
            RunManager.Inst.mapManager.SetVisible();
        }
        else
        {
            defeatCanvas.gameObject.SetActive(true);
        }

    }

    public void OnCardUse()
    {
        CardUse?.Invoke();
    }
}
