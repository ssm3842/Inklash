using UnityEngine;

public class BattleManager : MonoBehaviour
{

    [SerializeField] GameObject battleUICanvas;
    [SerializeField] BattleRewardController battleRewardCanvas;

    [SerializeField] RunEndCanvas runEndCanvas;

    public CardManager cardManager;
    public CardUseManager cardUseManager;

    public void InitBattle()
    {
        battleRewardCanvas.gameObject.SetActive(false);

        cardManager.cardUseManager = cardUseManager;

        //덱, 코스트 초기화 및 전투 시작.
        cardManager.Init();
        battleUICanvas.SetActive(true);

        //데미지 텍스트 삭제
        DamageTextCanvas.Inst.Init();

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
        battleRewardCanvas.AddRewards(goldButtons: 1, cardButtons: 2);

        cardManager.CardRightClicked();

        RunManager.Inst.mapManager.ClearLastRoom();
    }

    public void OnBattleLose()
    {
        Time.timeScale = 0f;

        EnemyBaseDataSO currentEnemy = RunManager.Inst.battleManager.cardUseManager.CurrentEnemyData;

        int lifePenalty = currentEnemy.isElite ? 999 : 1 ; //TODO: Elite -> Boss
        bool isGameOver = RunManager.Inst.resourceManager.DecreaseLife(lifePenalty);
        if (isGameOver)
        {
            cardManager.CardRightClicked();
            
            RunManager.Inst.mapManager.FailLastRoom();
            RunManager.Inst.mapManager.SetVisible();
        }
        else
        {
            runEndCanvas.SetCanvas("패배했습니다.");
        }

    }

    public void OnRunEnd(string text)
    {
        runEndCanvas.SetCanvas("승리했습니다");
    }
}
