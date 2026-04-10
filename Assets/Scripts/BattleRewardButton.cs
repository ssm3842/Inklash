using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewardButton : MonoBehaviour
{
    [SerializeField] Sprite[] iconImages;

    [SerializeField] Image rewardIcon;
    [SerializeField] TextMeshProUGUI rewardText;

    RewardType rewardType;
    int rewardContent;
    List<CardDataSO> cardRewardList;

    public void AddRewardButton(RewardType type, int amount)
    {
        rewardType = type; //버튼에 들어갈 보상 타입을 저장.
        switch (type)
        {
            case RewardType.Gold:
                rewardIcon.sprite = iconImages[0];
                rewardText.text = "금화 " + amount + "개 획득";

                rewardContent = amount;
                break;
            case RewardType.Card:
                rewardIcon.sprite = iconImages[1];
                rewardText.text = "새 단어 획득";

                //카드 보상 후보를 설정 및 3개 추출.
                cardRewardList = new List<CardDataSO>();
                List<CardDataSO> allCardRewardPool = RunManager.Inst.unitDataManager.GetCardRewardPool();
                for(int i=0; i<3; i++)
                {
                    int randomI = Random.Range(0, allCardRewardPool.Count);

                    CardDataSO currentCardData = allCardRewardPool[randomI];
                    cardRewardList.Add(currentCardData);
                }
                break;
            case RewardType.Artifact:
                break;
        }
    }

    public void TryTakeReward()
    {
        switch (rewardType)
        {
            case RewardType.Gold:
                RunManager.Inst.resourceManager?.EarnGold(rewardContent);
                Destroy(gameObject);
                break;
            case RewardType.Card:
                RunManager.Inst.cardRewardCanvas.GetComponent<CardRewardUI>().ShowCardReward(cardRewardList);
                break;
            case RewardType.Artifact:
                Destroy(gameObject);
                break;
        }
    }

}

public enum RewardType{
    Gold, Card, Artifact,
}
