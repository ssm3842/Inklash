using UnityEngine;
using UnityEngine.UI;

public class BattleRewardController : MonoBehaviour
{
    [SerializeField] BattleRewardButton battleRewardButtonPrefab;
    [SerializeField] GameObject battleRewardButtonContainer;

    GameObject targetCardRewardButton = null;

    public void AddRewards()
    {
        AddGoldReward();
        AddCardReward();
        AddCardReward();
    }

    void AddGoldReward()
    {
        BattleRewardButton newGoldRewardButton = Instantiate(battleRewardButtonPrefab, battleRewardButtonContainer.transform);
        newGoldRewardButton.AddRewardButton(RewardType.Gold, Random.Range(50, 70));
    }
    
    void AddCardReward()
    {
        BattleRewardButton newCardRewardButton = Instantiate(battleRewardButtonPrefab, battleRewardButtonContainer.transform);
        newCardRewardButton.GetComponent<Button>().onClick.AddListener(() => ConnectCardRewardButton(newCardRewardButton.gameObject));
        newCardRewardButton.AddRewardButton(RewardType.Card, 0);
    }

    //누른 버튼과 카드 보상 캔버스를 연결.
    void ConnectCardRewardButton(GameObject cardRewardButton)
    {
        targetCardRewardButton = cardRewardButton;
    }

    public void CardRewardAccepted()
    {
        Destroy(targetCardRewardButton);
        targetCardRewardButton = null;
    }
}
