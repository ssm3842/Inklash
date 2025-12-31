using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{

    [SerializeField] GameObject battleUICanvas;
    [SerializeField] GameObject battleRewardCanvas;
    [SerializeField] GameObject battleRewardButtonContainer;
    [SerializeField] BattleRewardButton battleRewardButtonPrefab;

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

        BattleRewardButton newRewardButton = Instantiate(battleRewardButtonPrefab, battleRewardButtonContainer.transform);
        newRewardButton.AddRewardButton(RewardType.Gold, 70);
        battleRewardCanvas.gameObject.SetActive(true);
    }

    public void OnCardUse()
    {
        CardUse?.Invoke();
    }
}
