using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRewardUI : MonoBehaviour
{
    [SerializeField]BattleRewardController battleRewardController;

    [SerializeField]Transform cardRewardContainer;

    [SerializeField]GameObject cardPrefab;

    [SerializeField]Button confirmButton;

    CardRewardCardUI rewardCard;

    public void ShowCardReward(List<CardDataSO> cardRewardList)
    {
        rewardCard = null;

        confirmButton.interactable = false;

        foreach(Transform child in cardRewardContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(CardDataSO cardData in cardRewardList)
        {
            GameObject cardUI = Instantiate(cardPrefab, cardRewardContainer);
            cardUI.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            cardUI.GetComponent<CardRewardCardUI>().Setup(cardData.card);
            cardUI.GetComponent<Button>().onClick.AddListener(() => selectCard(cardUI.GetComponent<CardRewardCardUI>()));
        }

        gameObject.SetActive(true);
    }

    void selectCard(CardRewardCardUI card)
    {
        foreach(Transform child in cardRewardContainer)
        {
            child.gameObject.GetComponent<CardRewardCardUI>().SetCardDark(true);
        }
        card.gameObject.GetComponent<CardRewardCardUI>().SetCardDark(false);

        rewardCard = card;
        confirmButton.interactable = true;
    }

    public void GetCard()
    {
        DeckManager.Inst.AddCardToDeck(rewardCard.cardContent);
        battleRewardController.CardRewardAccepted();
        gameObject.SetActive(false);
    }
}
