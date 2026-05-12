using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRewardUI : MonoBehaviour
{
    [SerializeField]BattleRewardController battleRewardController;

    [SerializeField]Transform cardRewardContainer;

    [SerializeField]GameObject cardPrefab;

    [SerializeField]GameObject cardDescPanel;

    [SerializeField]Button confirmButton;

    CardRewardCardUI rewardCard;

    public void ShowCardReward(List<CardDataSO> cardRewardList)
    {
        rewardCard = null;

        confirmButton.interactable = false;

        cardDescPanel.SetActive(false);

        foreach(Transform child in cardRewardContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(CardDataSO cardData in cardRewardList)
        {
            GameObject cardUI = Instantiate(cardPrefab, cardRewardContainer);
            cardUI.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            cardUI.GetComponent<CardRewardCardUI>().Setup(cardData.card);
            cardUI.GetComponent<Button>().onClick.AddListener(() => SelectCard(cardUI.GetComponent<CardRewardCardUI>()));
        }

        gameObject.SetActive(true);
    }

    void SelectCard(CardRewardCardUI card)
    {
        foreach(Transform child in cardRewardContainer)
        {
            child.gameObject.GetComponent<CardRewardCardUI>().SetCardDark(true);
            cardDescPanel.SetActive(true);
            cardDescPanel.GetComponent<RectTransform>().position = card.transform.position;
        }
        card.gameObject.GetComponent<CardRewardCardUI>().SetCardDark(false);

        rewardCard = card;
        confirmButton.interactable = true;
    }

    public void GetCard()
    {
        DeckManager.Inst.AddCardToDeck(rewardCard.cardContent);
        // battleRewardController.CardRewardAccepted();
        
        RunManager.Inst.mapManager.SetVisible();
        gameObject.SetActive(false);
    }
    public void SkipCard()
    {
        RunManager.Inst.mapManager.SetVisible();
        gameObject.SetActive(false);
    }
}
