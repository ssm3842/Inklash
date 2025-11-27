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

    public void AddRewardButton(RewardType type, int amount)
    {
        rewardType = type; //버튼에 들어갈 보상 타입을 저장.
        switch (type)
        {
            case RewardType.Gold:
                rewardIcon.sprite = iconImages[0];
                rewardText.text = amount + " Gold";

                rewardContent = amount;
                break;
            case RewardType.Card:
                rewardIcon.sprite = iconImages[1];
                rewardText.text = "Card";
                break;
            case RewardType.Artifact:
                break;
        }
    }

    //골드 증가 확인용
    private void Start()
    {
        if (resourceManager == null)
        {
            resourceManager = FindAnyObjectByType<ResourceManager>();
        }
    }
    public void TryTakeReward()
    {
        switch (rewardType)
        {
            case RewardType.Gold:
                RunManager.resourceManager?.EarnGold(rewardContent);
                break;
            case RewardType.Card:
                //TODO: 랜덤한 카드 3개를 선택. 및 표시.
                break;
            case RewardType.Artifact:
                break;
        }
        Destroy(gameObject);
    }

}

public enum RewardType{
    Gold, Card, Artifact,
}
