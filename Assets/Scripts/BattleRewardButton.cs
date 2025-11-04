using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewardButton : MonoBehaviour
{
    [SerializeField] Sprite[] iconImages;

    [SerializeField] Image rewardIcon;
    [SerializeField] TextMeshProUGUI rewardText;

    public void AddRewardButton(RewardType type, int amount)
    {
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

    public void TryTakeReward()
    {
        Destroy(gameObject);
    }
}

public enum RewardType{
    Gold, Card, Artifact,
}
