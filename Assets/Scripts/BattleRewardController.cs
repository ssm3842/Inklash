using UnityEngine;
using UnityEngine.UI;

public class BattleRewardController : MonoBehaviour
{
    [SerializeField] BattleRewardButton battleRewardButtonPrefab;
    [SerializeField] GameObject battleRewardButtonContainer;

    GameObject targetCardRewardButton = null;

    public void AddRewards(int goldButtons, int cardButtons)
    {
        //이전에 남아 있는 보상버튼 삭제
        foreach(Transform reward in battleRewardButtonContainer.transform)
        {
            Destroy(reward.gameObject);
        }

        //골드 보상 생성.
        for(int i = 0; i < goldButtons; i++)
        {
            AddGoldReward();
        }

        //카드 보상 생성.
        for(int i = 0; i < cardButtons; i++)
        {
            AddCardReward();
        }

        gameObject.SetActive(true);
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
