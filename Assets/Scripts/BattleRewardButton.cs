using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewardButton : MonoBehaviour
{
    [SerializeField] Sprite[] iconImages;

    [SerializeField] Image rewardIcon;
    [SerializeField] TextMeshProUGUI rewardText;

    //골드 증가 확인용
    [SerializeField] private ResourceManager resourceManager;   
    private RewardType myType;
    private int myAmount;


    public void AddRewardButton(RewardType type, int amount)
    {
        //골드 증가 확인용
        myType = type;
        myAmount = amount;

        switch (type)
        {
            case RewardType.Gold:
                rewardIcon.sprite = iconImages[0];
                rewardText.text = amount + " Gold";
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
        if (myType == RewardType.Gold)
        {
            if (RunManager.Inst != null && resourceManager != null)
            {
                resourceManager.EarnGold(myAmount);
                Debug.Log($"{myAmount} 골드 획득!");
            }
        }
        else if (myType == RewardType.Card)
        {
 
        }

        Destroy(gameObject);
    }

}

public enum RewardType{
    Gold, Card, Artifact,
}
