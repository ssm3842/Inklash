using UnityEngine;

public class BattleRewardController : MonoBehaviour
{
    [SerializeField] BattleRewardButton battleRewardButtonPrefab;
    [SerializeField] GameObject battleRewardButtonContainer;


    public void AddRewards()
    {
        BattleRewardButton newGoldRewardButton = Instantiate(battleRewardButtonPrefab, battleRewardButtonContainer.transform);
        newGoldRewardButton.AddRewardButton(RewardType.Gold, 70);

        BattleRewardButton newCardRewardButton = Instantiate(battleRewardButtonPrefab, battleRewardButtonContainer.transform);
        newCardRewardButton.AddRewardButton(RewardType.Card, 70);
    }
}
