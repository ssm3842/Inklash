using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddCardEvent : MonoBehaviour
{
    [SerializeField]GameObject cardPrefab;

    [SerializeField]Transform cardRewardContainer;

    [SerializeField]Button rerollButton;
    [SerializeField]Button getCardButton;
    [SerializeField]TextMeshProUGUI eventCostText;
    int eventRepeat;

    CardRewardCardUI rewardCard;
    public void SetEvent()
    {
        rewardCard = null;

        eventRepeat = 0;
        rerollButton.interactable = CheckEventAvailable();
        getCardButton.interactable = false;
    }

    public void GetNewRandomCards()
    {
        cardRewardContainer.gameObject.SetActive(true);

        List<CardDataSO> cardRewardList = new List<CardDataSO>();
        List<CardDataSO> allCardRewardPool = RunManager.Inst.unitDataManager.GetCardRewardPool();
        for(int i=0; i<3; i++)
        {
            int randomI = Random.Range(0, allCardRewardPool.Count);

            CardDataSO currentCardData = allCardRewardPool[randomI];
            cardRewardList.Add(currentCardData);
            allCardRewardPool.Remove(currentCardData); //선택지에 같은 카드가 나오는 것 방지.
        }

        foreach (Transform child in cardRewardContainer)
        {
            Destroy(child.gameObject);
        }
        foreach(CardDataSO cardDataSO in cardRewardList)
        {
            GameObject cardUI = Instantiate(cardPrefab, cardRewardContainer);
            cardUI.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            cardUI.GetComponent<CardRewardCardUI>().Setup(cardDataSO.card);
            cardUI.GetComponent<Button>().onClick.AddListener(() => selectCard(cardUI.GetComponent<CardRewardCardUI>()));
        }

        RunManager.Inst.resourceManager.SpendGold(50 * (eventRepeat + 1));
        eventRepeat++;
        CheckEventAvailable();
    }

    void selectCard(CardRewardCardUI card)
    {
        foreach(Transform child in cardRewardContainer)
        {
            child.gameObject.GetComponent<CanvasGroup>().alpha = 0.5f;
        }
        card.gameObject.GetComponent<CanvasGroup>().alpha = 1f;

        rewardCard = card;
        getCardButton.interactable = true;
    }

    bool CheckEventAvailable()
    {
        eventCostText.text = (50 * (eventRepeat + 1)).ToString();

        //충분한 골드를 소지하고 있을 경우
        if(RunManager.Inst.resourceManager.currentGold >= 50 * (eventRepeat + 1))
        {
            eventCostText.color = Color.white;
            return true;
        }
        else
        {
            eventCostText.color = Color.red;
            rerollButton.interactable = false;
            return false;
        }
    }

    public void GetCard()
    {
        DeckManager.Inst.AddCardToDeck(rewardCard.cardContent);
        getCardButton.interactable = false;
        cardRewardContainer.gameObject.SetActive(false);

        // eventManager._OnEventEnd();
    }
}
