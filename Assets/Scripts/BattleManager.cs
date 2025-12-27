using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{

    [SerializeField] GameObject battleUICanvas;
    [SerializeField] GameObject battleRewardCanvas;
    [SerializeField] GameObject battleRewardButtonContainer;
    [SerializeField] BattleRewardButton battleRewardButtonPrefab;

    [SerializeField] DeckSO tempEnemyPool;

    public CardManager cardManager;
    public CardUseManager cardUseManager;

    public UnityEvent CardUse;

    public void InitBattle()
    {
        battleRewardCanvas.gameObject.SetActive(false);

        cardManager.cardUseManager = cardUseManager;

        cardManager.Init();
        battleUICanvas.SetActive(true);

        Time.timeScale = 1f;
        gameObject.SetActive(true);

        cardUseManager.InitUnitManager(tempEnemyPool.startingDeck); //TODO: 방 정보를 읽어와 적 유닛풀 설정. 지금은 임시코드로 대체.
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
